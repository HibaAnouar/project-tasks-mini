import { useState } from "react";
import { apiFetch } from "../services/api";
import { toast } from "react-toastify";

export default function TaskItem({ task, onChanged }) {
  const [loading, setLoading] = useState(false);
  const [completed, setCompleted] = useState(task.isCompleted);

  async function completeTask() {
    if (completed) return;

    try {
      setLoading(true);
      setCompleted(true);

      await apiFetch(
        `/projects/${task.projectId}/tasks/${task.id}/complete`,
        { method: "PUT" }
      );

      toast.success("Task completed");
      onChanged?.();
    } catch (err) {
      setCompleted(false);
      toast.error(err.message);
    } finally {
      setLoading(false);
    }
  }

  async function deleteTask() {
    if (!confirm("Delete this task?")) return;

    try {
      setLoading(true);
      await apiFetch(
        `/projects/${task.projectId}/tasks/${task.id}`,
        { method: "DELETE" }
      );
      toast.success("Task deleted");
      onChanged?.();
    } catch (err) {
      toast.error(err.message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        gap: 4,
        marginBottom: 8,
        opacity: loading ? 0.6 : 1,
        textDecoration: completed ? "line-through" : "none",
        border: "1px solid #ccc",
        padding: "6px",
        borderRadius: "4px",
      }}
    >
      <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
        <span style={{ flex: 1, fontWeight: "bold" }}>{task.title}</span>

        {!completed && (
          <button onClick={completeTask} disabled={loading}>
            ✓
          </button>
        )}
        <button onClick={deleteTask} disabled={loading}>
          🗑
        </button>
      </div>

      {task.description && (
        <small style={{ color: "#555" }}>{task.description}</small>
      )}
      {task.dueDate && (
        <small style={{ color: "#999" }}>
          Due: {new Date(task.dueDate).toLocaleDateString()}
        </small>
      )}
    </div>
  );
}
