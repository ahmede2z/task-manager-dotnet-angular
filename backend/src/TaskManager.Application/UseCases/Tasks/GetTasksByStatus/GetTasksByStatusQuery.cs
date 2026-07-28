using MediatR;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Tasks;

namespace TaskManager.Application.UseCases.Tasks.GetTasksByStatus;

public sealed record GetTasksByStatusQuery(TaskStatus Status) : IRequest<Result<IReadOnlyList<TaskListItemResponse>>>;
