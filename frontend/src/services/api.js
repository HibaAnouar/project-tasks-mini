const API_URL = "http://localhost:5078/api";

export async function apiFetch(path, options = {}) {
  const token = localStorage.getItem("token");

  const headers = {
    "Content-Type": "application/json",
  };

  if (token && !path.startsWith("/auth")) {
    headers.Authorization = `Bearer ${token}`;
  }

  const res = await fetch(`${API_URL}${path}`, {
    method: options.method || "GET",
    headers,
    body: options.body ? JSON.stringify(options.body) : undefined,
  });

  if (!res.ok) {
    let message = "API error";
    try {
      const error = await res.json();
      message = error.message || message;
    } catch {}

    throw new Error(message);
  }

  return res.status === 204 ? null : res.json();
}
