import { useEffect, useState } from "react";
import { apiFetch } from "../services/api";
import ProjectCard from "../components/ProjectCard";
import Header from "../components/Header";

export default function Projects() {
  const [projects, setProjects] = useState([]);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState(""); 
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadProjects();
  }, []);

  async function loadProjects() {
    try {
      setLoading(true);
      const data = await apiFetch("/projects");
      setProjects(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  async function createProject(e) {
    e.preventDefault();
    if (!title.trim()) {
      setError("Project title cannot be empty");
      return;
    }

    const project = await apiFetch("/projects", {
      method: "POST",
      body: { title, description: description || null }, 
    });

    setProjects((prev) => [...prev, project]);
    setTitle("");
    setDescription("");
  }

  if (loading) return <p>Loading projects...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;

  return (
    <>
      <Header />
      <div className="container">
        <h1>My Projects</h1>

        <form onSubmit={createProject} style={{ marginBottom: "16px" }}>
          <input
            placeholder="Project title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
          />
          <input
            placeholder="Project description (optional)"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
          <button disabled={!title.trim() || loading}>
            {loading ? "Creating..." : "Create"}
          </button>
        </form>

        {projects.length === 0 && <p>No projects yet.</p>}

        {projects.map((p) => (
          <ProjectCard key={p.id} project={p} onDeleted={loadProjects} />
        ))}
      </div>
    </>
  );
}
