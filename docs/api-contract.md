# API contract

Extracted from `backend/src/TaskManager.Api/Controllers/*.cs` and the DTOs they reference
(2026-07-28, re-synced after subtask 5b). Base URL in Development: `http://localhost:5086`. All
responses are JSON; `TaskStatus` is serialized as a string: `"ToDo" | "InProgress" | "Done"`.
Errors are RFC 7807 `ProblemDetails`.

## Projects

### GET /api/projects
- Response (200): `ProjectListItemResponse[]`
  - `ProjectListItemResponse`: `{ id: number, name: string, description: string | null, createdAt: string /* ISO 8601 UTC */ }`

### GET /api/projects/{id}
- Path params: `id: number` (min 1)
- Response (200): `ProjectResponse`
  - `ProjectResponse`: `{ id: number, name: string, description: string | null, createdAt: string /* ISO 8601 UTC */ }`
- Response (404): ProblemDetails

### POST /api/projects
- Request body: `{ name: string /* required, max 100 */, description?: string /* max 1000 */ }`
- Response (201): `CreateProjectResponse`
  - `CreateProjectResponse`: `{ id: number, name: string, description: string | null, createdAt: string /* ISO 8601 UTC */ }`
- Response (400): ProblemDetails, per-field errors

### PUT /api/projects/{id}
- Path params: `id: number` (min 1)
- Request body: `{ name: string /* required, max 100 */, description?: string /* max 1000 */ }`
- Response (204): no body
- Response (400): ProblemDetails, per-field errors
- Response (404): ProblemDetails

### DELETE /api/projects/{id}
- Path params: `id: number` (min 1)
- Response (204): no body — cascade-deletes the project's tasks
- Response (404): ProblemDetails

## Tasks

### GET /api/tasks
- Query params: `status?: 'ToDo' | 'InProgress' | 'Done'`
- Response (200): `TaskListItemResponse[]`
  - `TaskListItemResponse`: `{ id: number, title: string, description: string | null, status: 'ToDo' | 'InProgress' | 'Done', dueDate: string /* YYYY-MM-DD */, projectName: string }`
- Response (400): ProblemDetails — invalid `status` value

### GET /api/projects/{projectId}/tasks
- Path params: `projectId: number` (min 1)
- Query params: `status?: 'ToDo' | 'InProgress' | 'Done'`
- Response (200): `TaskListItemResponse[]` (see shape above)
- Response (400): ProblemDetails — invalid `status` value
- Response (404): ProblemDetails — project does not exist

### GET /api/tasks/{id}
- Path params: `id: number` (min 1)
- Response (200): `TaskResponse`
  - `TaskResponse`: `{ id: number, title: string, description: string | null, status: 'ToDo' | 'InProgress' | 'Done', dueDate: string /* YYYY-MM-DD */, projectName: string }`
- Response (404): ProblemDetails

### POST /api/tasks
- Request body: `{ title: string /* required, max 200 */, description?: string /* max 1000 */, status: 'ToDo' | 'InProgress' | 'Done', dueDate: string /* YYYY-MM-DD, required */, projectId: number /* required, must reference an existing project */ }`
- Response (201): `CreateTaskResponse`
  - `CreateTaskResponse`: `{ id: number, title: string, description: string | null, status: 'ToDo' | 'InProgress' | 'Done', dueDate: string /* YYYY-MM-DD */, projectId: number }` — note: create/update echo `projectId` (the field the client just sent), not `projectName`; only the read endpoints above resolve the name (D29).
- Response (400): ProblemDetails, per-field errors
- Response (404): ProblemDetails — `projectId` does not reference an existing project

### PUT /api/tasks/{id}
- Path params: `id: number` (min 1)
- Request body: `{ title: string /* required, max 200 */, description?: string /* max 1000 */, status: 'ToDo' | 'InProgress' | 'Done', dueDate: string /* YYYY-MM-DD, required */, projectId: number /* required, must reference an existing project */ }`
- Response (204): no body
- Response (400): ProblemDetails, per-field errors
- Response (404): ProblemDetails — task or `projectId` not found

### DELETE /api/tasks/{id}
- Path params: `id: number` (min 1)
- Response (204): no body
- Response (404): ProblemDetails
