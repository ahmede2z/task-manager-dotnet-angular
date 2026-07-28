using MediatR;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Tasks;

namespace TaskManager.Application.UseCases.Tasks.GetAllTasks;

public sealed record GetAllTasksQuery : IRequest<Result<IReadOnlyList<TaskListItemResponse>>>;
