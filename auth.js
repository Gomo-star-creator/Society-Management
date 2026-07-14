const API_BASE = "https://localhost:7274/api"; 

function saveSession(session) {
  localStorage.setItem("society_session", JSON.stringify(session));
}
function getSession() {
  const raw = localStorage.getItem("society_session");
  return raw ? JSON.parse(raw) : null;
}
function logout() {
  localStorage.removeItem("society_session");
  window.location.href = "signin.html";
}
function requireLogin() {
  const session = getSession();
  if (!session) window.location.href = "signin.html";
  return session;
}