import { useEffect, useState } from "react";
import { apiRequest } from "../services/api";
import ProjectCard from "../components/ProjectCard";

export default function Projects() {
  const [projects, setProjects] = useState([]);

  useEffect(() => {
    apiRequest("/projects").then(setProjects);
  }, []);

  return (
    <div>
      <h2>My Projects</h2>

      {projects.map((p) => (
        <ProjectCard key={p.id} project={p} />
      ))}
    </div>
  );
}
