import { Link } from "react-router-dom";
import { apiFetch } from "../services/api";
import ProjectProgress from "./ProjectProgress";

export default function ProjectCard({ project, onDeleted }) {
  async function deleteProject() {
    if (!confirm("Delete this project?")) return;

    try {
      await apiFetch(`/projects/${project.id}`, { method: "DELETE" });
      if (onDeleted) onDeleted();
    } catch (err) {
      alert(err.message);
    }
  }

  return (
    <div
      style={{
        padding: "12px",
        border: "1px solid #ccc",
        borderRadius: "6px",
        marginBottom: "10px",
      }}
    >
      <div style={{ display: "flex", alignItems: "center", marginBottom: "8px" }}>
        <Link
          to={`/projects/${project.id}`}
          style={{
            textDecoration: "none",
            color: "inherit",
            flex: 1,
          }}
        >
          <strong>{project.title}</strong>
          {project.description && (
            <div style={{ color: "#555", fontSize: "0.9em" }}>
              {project.description}
            </div>
          )}
        </Link>

        <button
          onClick={deleteProject}
          style={{
            marginLeft: "12px",
            background: "#f44336",
            color: "#fff",
            border: "none",
            borderRadius: "4px",
            padding: "4px 8px",
            cursor: "pointer",
          }}
        >
          Delete
        </button>
      </div>

      <ProjectProgress projectId={project.id} />
    </div>
  );
}
