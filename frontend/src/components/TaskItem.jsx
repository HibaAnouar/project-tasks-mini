export default function TaskItem({ task }) {
  return (
    <div>
      <input type="checkbox" checked={task.isCompleted} readOnly />
      {task.title}
    </div>
  );
}
