const session = requireLogin();
document.getElementById("welcome").textContent = `${session.fullName} (${session.role})`;
document.getElementById("logout-btn").addEventListener("click", logout);

const isAdmin = session.role === "Admin";
if (isAdmin) document.getElementById("event-form").style.display = "block";

async function loadEvents() {
  const res = await fetch(`${API_BASE}/events`);
  const events = await res.json();
  const list = document.getElementById("events-list");
  list.innerHTML = "";

  for (const ev of events) {
    const card = document.createElement("div");
    card.className = "event-card";

    const actionsHtml = isAdmin
      ? `<a href="attendance.html?eventId=${ev.id}" class="link">View attendance</a>`
      : `<button class="attend-btn" data-event="${ev.id}">Toggle my attendance</button>
         <span id="status-${ev.id}" class="status"></span>`;

    card.innerHTML = `
      <div>
        <p class="event-title">${ev.title}</p>
        <p class="event-meta">${new Date(ev.eventDate).toLocaleString()} · ${ev.location ?? ""}</p>
      </div>
      <div>${actionsHtml}</div>
    `;
    list.appendChild(card);

    if (!isAdmin) refreshMyStatus(ev.id);
  }

  if (!isAdmin) {
    document.querySelectorAll(".attend-btn").forEach(btn => {
      btn.addEventListener("click", () => toggleAttendance(btn.dataset.event));
    });
  }
}

async function refreshMyStatus(eventId) {
  const res = await fetch(`${API_BASE}/attendance/event/${eventId}`);
  const records = await res.json();
  const mine = records.find(r => r.memberId === session.memberId);
  const el = document.getElementById(`status-${eventId}`);
  if (el) el.textContent = mine?.attended ? "Present" : "Not marked";
}

async function toggleAttendance(eventId) {
  const res = await fetch(`${API_BASE}/attendance/event/${eventId}`);
  const records = await res.json();
  const mine = records.find(r => r.memberId === session.memberId);
  const newStatus = !(mine?.attended);

  await fetch(`${API_BASE}/attendance/mine`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ eventId: Number(eventId), memberId: session.memberId, attended: newStatus })
  });

  refreshMyStatus(eventId);
}

loadEvents();

if (isAdmin) {
  document.getElementById("event-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const newEvent = {
      title: document.getElementById("title").value,
      eventDate: document.getElementById("eventDate").value,
      location: document.getElementById("location").value
    };
    await fetch(`${API_BASE}/events`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(newEvent)
    });
    e.target.reset();
    loadEvents();
  });
}