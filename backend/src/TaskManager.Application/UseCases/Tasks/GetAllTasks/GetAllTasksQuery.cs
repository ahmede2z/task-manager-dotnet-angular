using MediatR;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Tasks.GetAllTasks;

public sealed record GetAllTasksQuery : IRequest<Result<IReadOnlyList<TaskListItemResponse>>>;
