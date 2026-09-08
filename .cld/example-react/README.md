# ASP.NET + React + Vite Template

A full-stack starter with:

| Layer | Stack |
|---|---|
| Backend | ASP.NET Web API (.NET 8), EF Core 8, SQLite, ASP.NET Identity, JWT |
| Frontend | React 18, Vite 5, TypeScript, Tailwind CSS 3, React Router 6, Axios |

---

## Project layout

```
/
├── backend/      ASP.NET Web API
└── frontend/     React + Vite SPA
```

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/) (LTS recommended)

---

## Getting started

### 1 — Backend

```bash
cd backend

# Restore NuGet packages
dotnet restore

# Apply the initial database migration
dotnet ef database update

# Run in development (https on 5001, http on 5000)
dotnet run
```

> **First time?** Create a migration first:
> ```bash
> dotnet ef migrations add InitialCreate
> dotnet ef database update
> ```

Swagger UI is available at **https://localhost:5001/swagger** while in development.

### 2 — Frontend

```bash
cd frontend

# Install npm dependencies
npm install

# Start the dev server (http://localhost:5173)
npm run dev
```

The Vite dev server proxies `/api/*` requests to `https://localhost:5001`, so no CORS config is needed during development.

---

## Configuration

### Backend — `backend/appsettings.json`

| Key | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQLite file path (default `app.db`) |
| `Jwt:Key` | **Change this** — long random secret, ≥ 32 characters |
| `Jwt:Issuer` | Token issuer URL |
| `Jwt:Audience` | Token audience URL |
| `Jwt:ExpiresInMinutes` | Token lifetime |
| `AllowedOrigins` | CORS whitelist — add your frontend URL in production |

### Frontend — environment variables

Create a `.env.local` file inside `frontend/` for local overrides:

```env
# Override the API base URL (optional — default is the Vite proxy /api)
VITE_API_BASE_URL=https://your-api.example.com/api
```

---

## Building for production

```bash
# Backend
cd backend && dotnet publish -c Release -o publish/

# Frontend
cd frontend && npm run build   # output goes to frontend/dist/
```

Deploy `backend/publish/` behind a reverse proxy (nginx, Caddy, Azure App Service, …) and serve `frontend/dist/` as a static site. Point the proxy to forward `/api/*` to the backend.

---

## Adding features

- **New API resource** → add a model in `backend/Models/`, register it in `AppDbContext`, add a migration, write a controller in `backend/Controllers/`.
- **New page** → add a component in `frontend/src/pages/`, wire it up in `frontend/src/App.tsx`.
- **New API call** → add a typed helper in `frontend/src/api/client.ts`.
