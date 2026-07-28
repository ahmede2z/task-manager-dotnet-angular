using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Tasks.GetAllTasks;

public sealed class GetAllTasksQueryHandler(ITaskItemRepository taskItemRepository)
    : IRequestHandler<GetAllTasksQuery, Result<IReadOnlyList<TaskListItemResponse>>>
{
    public async Task<Result<IReadOnlyList<TaskListItemResponse>>> Handle(
        GetAllTasksQuery request,
        CancellationToken cancellationToken)
    {
        var tasks = await taskItemRepository.ListAsync(predicate: null, cancellationToken);

        var response = tasks
            .Select(t => new TaskListItemResponse(t.Id, t.Title, t.Description, t.Status, t.DueDate, t.ProjectId))
            .ToList();

        return Result.Success<IReadOnlyList<TaskListItemResponse>>(response);
    }
}
