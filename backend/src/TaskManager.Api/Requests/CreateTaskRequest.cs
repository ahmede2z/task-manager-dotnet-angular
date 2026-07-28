namespace TaskManager.Api.Requests;

/// <summary>
/// Request to create a new task.
/// </summary>
public sealed record CreateTaskRequest(
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate,
    int ProjectId);
