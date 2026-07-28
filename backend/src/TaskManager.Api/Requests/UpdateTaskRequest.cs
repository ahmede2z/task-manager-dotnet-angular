namespace TaskManager.Api.Requests;

/// <summary>
/// Request to update a task.
/// </summary>
public sealed record UpdateTaskRequest(
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate,
    int ProjectId);
