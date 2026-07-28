using MediatR;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Tasks.GetTasksByProject;

public sealed record GetTasksByProjectQuery(int ProjectId) : IRequest<Result<IReadOnlyList<TaskListItemResponse>>>;
