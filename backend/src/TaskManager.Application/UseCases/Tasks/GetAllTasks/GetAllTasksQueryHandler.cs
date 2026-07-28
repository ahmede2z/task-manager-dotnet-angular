using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Tasks;

namespace TaskManager.Application.UseCases.Tasks.GetAllTasks;

public sealed class GetAllTasksQueryHandler(
    ITaskItemRepository taskItemRepository,
    IProjectRepository projectRepository) : IRequestHandler<GetAllTasksQuery, Result<IReadOnlyList<TaskListItemResponse>>>
{
    public async Task<Result<IReadOnlyList<TaskListItemResponse>>> Handle(
        GetAllTasksQuery request,
        CancellationToken cancellationToken)
    {
        var tasks = await taskItemRepository.ListAsync(predicate: null, cancellationToken);

        var projectIds = tasks.Select(t => t.ProjectId).Distinct().ToList();
        var projects = await projectRepository.ListAsync(p => projectIds.Contains(p.Id), cancellationToken);
        var projectNames = projects.ToDictionary(p => p.Id, p => p.Name);

        var response = tasks
            .Select(t => new TaskListItemResponse(t.Id, t.Title, t.Description, t.Status, t.DueDate, projectNames[t.ProjectId]))
            .ToList();

        return Result.Success<IReadOnlyList<TaskListItemResponse>>(response);
    }
}
