namespace TaskManager.Application.UseCases.Tasks.GetTasksByStatus;

public sealed record TaskListItemResponse(
    int Id,
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate,
    int ProjectId);
