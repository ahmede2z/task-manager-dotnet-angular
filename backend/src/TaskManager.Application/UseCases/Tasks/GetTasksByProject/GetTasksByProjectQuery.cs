using MediatR;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Tasks;

namespace TaskManager.Application.UseCases.Tasks.GetTasksByProject;

public sealed record GetTasksByProjectQuery(int ProjectId, TaskStatus? Status = null) : IRequest<Result<IReadOnlyList<TaskListItemResponse>>>;
