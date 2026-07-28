namespace TaskManager.Application.UseCases.Tasks.GetTaskById;

public sealed record TaskResponse(
    int Id,
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate,
    string ProjectName);
