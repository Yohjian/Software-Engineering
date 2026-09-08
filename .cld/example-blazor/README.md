# ASP.NET + Blazor WebAssembly Template

A full-stack starter with:

| Layer | Stack |
|---|---|
| Backend | ASP.NET Web API (.NET 10), EF Core 10, SQLite, ASP.NET Identity, JWT |
| Frontend | Blazor WebAssembly (.NET 10), Tailwind CSS (CDN), custom JWT auth state |

---

## Project layout

```
/
├── backend/      ASP.NET Web API
└── frontend/     Blazor WebAssembly SPA
```

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- `aspnet-runtime` installed (Arch/CachyOS: `sudo pacman -S aspnet-runtime`)
- `dotnet-ef` global tool: `dotnet tool install --global dotnet-ef`

---

## Getting started

### 1 — Backend

```bash
cd backend

dotnet restore

# First run: create and apply the database migration
dotnet ef migrations add InitialCreate
dotnet ef database update

dotnet run
```

The API listens on **http://localhost:5000**.

### 2 — Frontend

```bash
cd frontend

dotnet run
```

The Blazor app opens at **http://localhost:5002**.  
Unlike Vite, there is no dev proxy — the browser talks directly to the backend, so CORS is already wired up.

---

## Configuration

### Backend — `backend/appsettings.json`

| Key | Purpose |
|---|---|
| `Jwt:Key` | **Change this** — random secret, ≥ 32 characters |
| `Jwt:Issuer` | Must match the backend URL |
| `Jwt:Audience` | Must match the frontend URL |
| `AllowedOrigins` | CORS whitelist — add your deployed frontend URL |

### Frontend — `frontend/Program.cs`

Change the `BaseAddress` of the `"Api"` HttpClient to match your deployed backend URL.

---

## Adding features

- **New API resource** → model in `backend/Models/`, register in `AppDbContext`, add a migration, write a controller.
- **New page** → add `MyPage.razor` in `frontend/Pages/` with `@page "/my-route"`.
- **New service** → add a class in `frontend/Services/` and register it in `frontend/Program.cs`.
