# Student Society Management App

A full-stack web application for managing a student society's members, events, and attendance — built with **ASP.NET Core**, **MySQL**, and a vanilla **HTML/CSS/JS** front end. Includes role-based access: admins manage events and view attendance, members view events and mark their own attendance.

## Features

- **Authentication** — sign up as a member, sign in, role-based session handling
- **Members** — view all society members
- **Events** — admins create events; all users view them
- **Attendance** — members toggle their own attendance per event; admins view attendance for any event
- **Role-based UI** — the frontend renders different views for Admin vs Member without any page reload

## Tech stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 8 Web API, Entity Framework Core |
| Database | MySQL (via Pomelo.EntityFrameworkCore.MySql) |
| Auth | BCrypt password hashing |
| Frontend | HTML, CSS, vanilla JavaScript (fetch API) |
| API docs | Swagger / OpenAPI |

## Architecture

```
MySQL database  <---- connection string (EF Core) ---->  ASP.NET Core API  <---- fetch + CORS ---->  Frontend (browser)
   SocietyDB              port 3306                    Controllers + EF Core        port 7001              HTML/CSS/JS
```

The frontend never talks to the database directly — all data flows through the API, which is the only layer with a MySQL connection string.

## Project structure

```
society-management/
├── SocietyApi/              # ASP.NET Core Web API
│   ├── Controllers/         # MembersController, EventsController, AttendanceController, AuthController
│   ├── Models/               # Member, Event, Attendance, User
│   ├── Data/                 # SocietyDbContext
│   └── appsettings.json     # connection string (not committed with real credentials)
├── SocietyFrontend/          # Static frontend
│   ├── assets/                # images
│   ├── index.html            # landing page
│   ├── signin.html
│   ├── signup.html
│   ├── events.html
│   ├── attendance.html
│   ├── style.css
│   ├── auth.js
│   ├── events.js
│   └── attendance.js
└── README.md
```

## Getting started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- MySQL Server + MySQL Workbench
- A code editor (Visual Studio 2022 recommended for the API, VS Code for the frontend)

### 1. Set up the database

Run the schema SQL in MySQL Workbench to create the `SocietyDB` database and its tables (`Users`, `Members`, `Events`, `Attendances`).

### 2. Configure and run the API

In `SocietyApi/appsettings.json`, set your own MySQL connection string:

```json
"ConnectionStrings": {
  "SocietyDB": "server=localhost;port=3306;database=SocietyDB;user=root;password=YOUR_PASSWORD;"
}
```

Then:

```bash
cd SocietyApi
dotnet restore
dotnet run
```

The API will start on a local port (check the console output, e.g. `https://localhost:7001`). Swagger docs are available at `/swagger`.

### 3. Seed a test admin account

Call this endpoint once via Swagger, then remove it from the code — it's scaffolding, not meant for production:

```
POST /api/auth/seed-test-users
```

Creates:
- `admin` / `admin123` → Admin role
- `member1` / `member123` → Member role

### 4. Run the frontend

Open `SocietyFrontend/index.html` with a local server (e.g. VS Code's **Live Server** extension). In `auth.js`, make sure `API_BASE` matches the port your API is actually running on.

## API endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/login` | Sign in, returns role + session info |
| POST | `/api/auth/register` | Self-register as a Member |
| GET | `/api/members` | List all members |
| GET/POST/PUT/DELETE | `/api/events` | Manage events |
| GET | `/api/attendance/event/{eventId}` | Attendance for one event |
| POST | `/api/attendance/mine` | Member sets their own attendance |

## Known limitations / next steps

- Login currently returns role info without a secure token — a production version would use JWT so the API can verify requests rather than trusting the frontend.
- Admin accounts must be created via the seed endpoint or directly in the database; there's no in-app "promote to admin" feature yet.
- No automated tests yet — a good next addition for the portfolio version.

## Author

Gomolemo Motsebe — built as a portfolio project demonstrating ASP.NET Core, MySQL, and full-stack web development.
