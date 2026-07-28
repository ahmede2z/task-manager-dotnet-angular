using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Tasks;

namespace TaskManager.Application.UseCases.Tasks.GetTasksByStatus;

public sealed class GetTasksByStatusQueryHandler(ITaskItemRepository taskItemRepository)
    : IRequestHandler<GetTasksByStatusQuery, Result<IReadOnlyList<TaskListItemResponse>>>
{
    public async Task<Result<IReadOnlyList<TaskListItemResponse>>> Handle(
        GetTasksByStatusQuery request,
        CancellationToken cancellationToken)
    {
        var tasks = await taskItemRepository.ListAsync(t => t.Status == request.Status, cancellationToken);

        var response = tasks
            .Select(t => new TaskListItemResponse(t.Id, t.Title, t.Description, t.Status, t.DueDate, t.ProjectId))
            .ToList();

        return Result.Success<IReadOnlyList<TaskListItemResponse>>(response);
    }
}
