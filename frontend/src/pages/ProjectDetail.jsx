import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { apiFetch } from "../services/api";
import TaskItem from "../components/TaskItem";
import Header from "../components/Header";

export default function ProjectDetail() {
  const { id } = useParams();

  const [project, setProject] = useState(null); 
  const [title, setTitle] = useState(""); 
  const [description, setDescription] = useState(""); 
  const [dueDate, setDueDate] = useState("");
  const [loading, setLoading] = useState(true);
  const [creating, setCreating] = useState(false);
  const [error, setError] = useState(null);

  const [progress, setProgress] = useState({
    totalTasks: 0,
    completedTasks: 0,
    progressPercentage: 0,
  });

  useEffect(() => {
    loadAll();
  }, [id]);

  async function loadAll() {
    setLoading(true);
    await Promise.all([loadProject(), loadTasks(), loadProgress()]);
    setLoading(false);
  }

  async function loadProject() {
    try {
      const data = await apiFetch(`/projects/${id}`);
      setProject(data);
    } catch (err) {
      setError(err.message);
    }
  }

  async function loadTasks() {
    try {
      const data = await apiFetch(`/projects/${id}/tasks`);
      setTasks(data);
    } catch (err) {
      setError(err.message);
    }
  }

  async function loadProgress() {
    try {
      const data = await apiFetch(`/projects/${id}/progress`);
      setProgress(data);
    } catch {
      setProgress({
        totalTasks: 0,
        completedTasks: 0,
        progressPercentage: 0,
      });
    }
  }

  async function createTask(e) {
    e.preventDefault();
    if (!title.trim()) return;

    try {
      setCreating(true);
      const task = await apiFetch(`/projects/${id}/tasks`, {
        method: "POST",
        body: { title, description, dueDate: dueDate || null },
      });

      setTasks((prev) => [...prev, task]);
      setTitle("");
      setDescription("");
      setDueDate("");
      loadProgress();
    } catch (err) {
      alert(err.message);
    } finally {
      setCreating(false);
    }
  }

  if (loading) return <p>Loading project...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;

  return (
    <>
      <Header />

      <div className="container">
        {project && (
          <>
            <h2>{project.title}</h2>
            {project.description && (
              <p style={{ color: "#555", marginBottom: "12px" }}>
                {project.description}
              </p>
            )}
          </>
        )}

        <div style={{ marginBottom: "20px" }}>
          <div
            style={{
              background: "#eee",
              borderRadius: "6px",
              height: "14px",
              overflow: "hidden",
            }}
          >
            <div
              style={{
                width: `${progress.progressPercentage}%`,
                height: "100%",
                background:
                  progress.progressPercentage === 100 ? "#4caf50" : "#2196f3",
                transition: "width 0.3s ease",
              }}
            />
          </div>
          <small style={{ color: "#555" }}>
            {progress.completedTasks} / {progress.totalTasks} tasks completed (
            {progress.progressPercentage}%)
          </small>
        </div>

        <form onSubmit={createTask} style={{ marginBottom: "12px" }}>
          <input
            placeholder="Task title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
          />
          <input
            placeholder="Description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
          <input
            type="date"
            value={dueDate}
            onChange={(e) => setDueDate(e.target.value)}
          />
          <button type="submit" disabled={!title.trim() || creating}>
            {creating ? "Adding..." : "Add Task"}
          </button>
        </form>

        {tasks.length === 0 && <p>No tasks yet.</p>}

        {tasks.map((t) => (
          <TaskItem
            key={t.id}
            task={{ ...t, projectId: id }}
            onChanged={() => {
              loadTasks();
              loadProgress();
            }}
          />
        ))}
      </div>
    </>
  );
}
