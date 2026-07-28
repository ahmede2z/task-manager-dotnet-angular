namespace TaskManager.Application.UseCases.Tasks.CreateTask;

public sealed record CreateTaskResponse(
    int Id,
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate,
    int ProjectId);
