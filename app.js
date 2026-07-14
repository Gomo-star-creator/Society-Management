const API_BASE = "https://localhost:7274/api"; 

async function loadMembers() {
  const res = await fetch(`${API_BASE}/members`);
  const members = await res.json();
  const tbody = document.querySelector("#members-table tbody");
  tbody.innerHTML = "";
  members.forEach(m => {
    const row = document.createElement("tr");
    row.innerHTML = `
      <td>${m.fullName}</td>
      <td>${m.email}</td>
      <td>${m.studentNumber}</td>
      <td><button onclick="deleteMember(${m.id})">Delete</button></td>
    `;
    tbody.appendChild(row);
  });
}

async function deleteMember(id) {
  await fetch(`${API_BASE}/members/${id}`, { method: "DELETE" });
  loadMembers();
}

document.getElementById("member-form").addEventListener("submit", async (e) => {
  e.preventDefault();
  const newMember = {
    fullName: document.getElementById("fullName").value,
    email: document.getElementById("email").value,
    studentNumber: document.getElementById("studentNumber").value,
    joinDate: new Date().toISOString()
  };
  await fetch(`${API_BASE}/members`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(newMember)
  });
  e.target.reset();
  loadMembers();
});

loadMembers();