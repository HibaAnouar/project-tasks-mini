import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { apiRequest } from "../services/api";
import TaskItem from "../components/TaskItem";

export default function ProjectDetail() {
  const { id } = useParams();
  const [tasks, setTasks] = useState([]);

  useEffect(() => {
    apiRequest(`/projects/${id}/tasks`).then(setTasks);
  }, [id]);

  return (
    <div>
      <h2>Project Tasks</h2>

      {tasks.map((t) => (
        <TaskItem key={t.id} task={t} />
      ))}
    </div>
  );
}
