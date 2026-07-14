const session = requireLogin();
document.getElementById("logout-btn").addEventListener("click", logout);

if (session.role !== "Admin") {
  window.location.href = "events.html"; // members don't get this page
}

const params = new URLSearchParams(window.location.search);
const eventId = params.get("eventId");

async function loadAttendance() {
  const [membersRes, attendanceRes, eventRes] = await Promise.all([
    fetch(`${API_BASE}/members`),
    fetch(`${API_BASE}/attendance/event/${eventId}`),
    fetch(`${API_BASE}/events/${eventId}`)
  ]);

  const members = await membersRes.json();
  const attendance = await attendanceRes.json();
  const eventInfo = await eventRes.json();

  document.getElementById("event-title").textContent = `Attendance — ${eventInfo.title}`;

  const tbody = document.querySelector("#attendance-table tbody");
  tbody.innerHTML = "";

  members.forEach(m => {
    const record = attendance.find(a => a.memberId === m.id);
    const row = document.createElement("tr");
    row.innerHTML = `
      <td>${m.fullName}</td>
      <td>${m.studentNumber}</td>
      <td>${record?.attended ? "Present" : "Absent"}</td>
    `;
    tbody.appendChild(row);
  });
}

loadAttendance();