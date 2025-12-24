import { useEffect, useState } from "react";
import { apiFetch } from "../services/api";

export default function ProjectProgress({ projectId }) {
  const [progress, setProgress] = useState({
    totalTasks: 0,
    completedTasks: 0,
    progressPercentage: 0,
  });

  useEffect(() => {
    loadProgress();
  }, [projectId]);

  async function loadProgress() {
    try {
      const data = await apiFetch(`/projects/${projectId}/progress`);
      setProgress(data);
    } catch {
      setProgress({
        totalTasks: 0,
        completedTasks: 0,
        progressPercentage: 0,
      });
    }
  }

  return (
    <div style={{ marginTop: "6px" }}>
      <div
        style={{
          background: "#eee",
          borderRadius: "6px",
          height: "10px",
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
  );
}
