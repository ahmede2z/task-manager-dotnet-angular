# ElectroPi Task Manager

A small full-stack task manager: projects contain tasks, each task has a status (`ToDo` /
`InProgress` / `Done`) and a due date. Backend is ASP.NET Core (.NET 10) with EF Core Code First
against SQL Server; frontend is Angular 22 with Material.

## Prerequisites

- **.NET 10 SDK** (backend, local run)
- **Node.js 22+** and npm (frontend, local run)
- **SQL Server**, reachable with Windows Integrated Security (local run) — or
- **Docker Desktop** (Docker run — no local SQL Server, .NET SDK, or Node install needed)

Pick one of the two options below; they use different ports and don't collide, so you can even run
both at once.

## Option A — Run locally

### Backend

```
cd backend/src/TaskManager.Api
dotnet run --launch-profile https
```

The API starts on `https://localhost:7287` (also `http://localhost:5086`) and opens Swagger at
`/swagger`. On startup in Development it applies pending EF Core migrations and seeds a few sample
projects and tasks automatically — no manual database step required.

The connection string is `Server=localhost;Database=TaskManagerDb;Integrated Security=True;...`
(`backend/src/TaskManager.Api/appsettings.Development.json`), so your SQL Server instance must
accept Windows Integrated Security and the account running `dotnet run` must have rights to create
the `TaskManagerDb` database.

### Frontend

```
cd frontend
npm install
npm start
```

Serves on `http://localhost:4201` and calls the API at `https://localhost:7287/api`. The first
request to the API over HTTPS may prompt you to trust the ASP.NET Core dev certificate
(`dotnet dev-certs https --trust`) if you haven't already.

### Database notes

- Schema is EF Core Code First; the committed migration is
  `backend/src/TaskManager.Infrastructure/Persistence/Migrations/20260728145927_InitialCreate.cs`.
- Auto-migrate + seed on startup only runs in Development (see above) — this is the intended path.
- To apply migrations manually instead (e.g. against a non-Development environment), run from
  `backend/src/TaskManager.Api`:
  ```
  dotnet ef database update --project ../TaskManager.Infrastructure
  ```

## Option B — Run with Docker

Brings up SQL Server, the API, and the frontend together — nothing besides Docker Desktop needs to
be installed on the host.

```
cp .env.example .env
docker compose up --build
```

On Windows without a `cp` command, copy `.env.example` to `.env` manually, or run
`Copy-Item .env.example .env` in PowerShell. Edit the `MSSQL_SA_PASSWORD` value in `.env` if you
want a password other than the placeholder (it must satisfy SQL Server's complexity rules: 8+
characters, upper+lower+digit+symbol).

Once all three services report healthy:

- Frontend: `http://localhost:4200`
- API: `http://localhost:8080` (Swagger at `http://localhost:8080/swagger`)
- SQL Server is only reachable inside the compose network, not published to the host

The API container runs with `ASPNETCORE_ENVIRONMENT=Development`, so it auto-migrates and seeds the
database on startup the same way the local run does — `docker compose up` alone produces a fully
working, seeded stack.

To stop and remove the containers:

```
docker compose down
```

Add `-v` to also drop the `sqlserver_data` volume and start from an empty database next time.

## API overview

REST API over `Project` (one-to-many) `TaskItem`:

- `GET/POST /api/projects`, `GET/PUT/DELETE /api/projects/{id}`
- `GET/POST /api/tasks`, `GET/PUT/DELETE /api/tasks/{id}`
- `GET /api/projects/{projectId}/tasks` — a project's tasks, optionally filtered with `?status=`
- `GET /api/tasks?status=` — all tasks filtered by status

Full request/response shapes are in [docs/api-contract.md](docs/api-contract.md). Errors are RFC
7807 `ProblemDetails`; validation failures return 400 with per-field messages.

A ready-to-import Postman collection is at
[TaskManager.postman_collection.json](TaskManager.postman_collection.json) — every request is
pre-filled and works against either the local (`http://localhost:5086`) or Docker
(`http://localhost:8080`) API without edits; just update the collection's base URL variable if you
switch between them.

## Assumptions and design decisions

- **No authentication.** The spec never mentions it, so none was added.
- **No tests.** Out of scope for the time given — see "with more time" below.
- **Clean Architecture** (Domain / Application / Infrastructure / API) with CQRS via MediatR and a
  Result pattern for expected failures (e.g. not-found); unhandled exceptions are caught by one
  global middleware and turned into `ProblemDetails`.
- **Projects and Tasks are kept as independent feature slices.** `TaskItem` references its project
  only by `ProjectId` — there is no EF navigation property between the two entities in either
  direction, only a database-level `ON DELETE CASCADE` foreign key. Either feature could be split
  into its own service and database later without touching the other.
- **Deleting a project cascade-deletes its tasks.** The confirmation step lives in the UI; the API
  itself performs the cascade unconditionally.
- **`TaskStatus` is stored as an `int` in the database** and serialized as a string on the wire
  (`"ToDo" | "InProgress" | "Done"`) for a readable API contract.
- **`DueDate` is a required date-only value** (SQL `date`, not `datetime`).
- **Task read responses carry `ProjectName`, not `ProjectId`** — resolved server-side (batched for
  list endpoints, not one query per task) since the frontend only ever needs the name, not a link
  back to the project by id.
- **No pagination, sorting, or response envelopes** on list endpoints — the data volumes here don't
  need them and the spec doesn't ask for them.
- **Docker support was added after the initial local-only setup**, as a second, independent way to
  run the project — not a replacement for the local path.

## What I'd do with more time

- Unit and integration tests (handlers, validators, controllers against an in-memory or
  test-container database).
- Pagination and sorting on the list endpoints once data volumes justified it.
- Authentication and authorization.
- CI (build, and once tests exist, run them) on push.
