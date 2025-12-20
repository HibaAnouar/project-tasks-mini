import { Link } from "react-router-dom";

export default function ProjectCard({ project }) {
  return (
    <div>
      <Link to={`/projects/${project.id}`}>
        <h3>{project.title}</h3>
      </Link>
    </div>
  );
}
