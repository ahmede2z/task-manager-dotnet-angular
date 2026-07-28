using MediatR;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Tasks.CreateTask;

public sealed record CreateTaskCommand(
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate,
    int ProjectId) : IRequest<Result<CreateTaskResponse>>;
