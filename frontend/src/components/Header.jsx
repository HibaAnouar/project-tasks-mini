import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

export default function Header() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate("/login");
  }

  return (
    <div className="header">
      <h3>Project Tasks</h3>
      <button className="secondary" onClick={handleLogout}>
        Logout
      </button>
    </div>
  );
}
