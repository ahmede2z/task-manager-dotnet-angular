using MediatR;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Tasks.UpdateTask;

public sealed record UpdateTaskCommand(
    int Id,
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate,
    int ProjectId) : IRequest<Result>;
