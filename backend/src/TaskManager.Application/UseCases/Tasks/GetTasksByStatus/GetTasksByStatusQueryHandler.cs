using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Tasks;

namespace TaskManager.Application.UseCases.Tasks.GetTasksByStatus;

public sealed class GetTasksByStatusQueryHandler(
    ITaskItemRepository taskItemRepository,
    IProjectRepository projectRepository) : IRequestHandler<GetTasksByStatusQuery, Result<IReadOnlyList<TaskListItemResponse>>>
{
    public async Task<Result<IReadOnlyList<TaskListItemResponse>>> Handle(
        GetTasksByStatusQuery request,
        CancellationToken cancellationToken)
    {
        var tasks = await taskItemRepository.ListAsync(t => t.Status == request.Status, cancellationToken);

        var projectIds = tasks.Select(t => t.ProjectId).Distinct().ToList();
        var projects = await projectRepository.ListAsync(p => projectIds.Contains(p.Id), cancellationToken);
        var projectNames = projects.ToDictionary(p => p.Id, p => p.Name);

        var response = tasks
            .Select(t => new TaskListItemResponse(t.Id, t.Title, t.Description, t.Status, t.DueDate, projectNames[t.ProjectId]))
            .ToList();

        return Result.Success<IReadOnlyList<TaskListItemResponse>>(response);
    }
}
