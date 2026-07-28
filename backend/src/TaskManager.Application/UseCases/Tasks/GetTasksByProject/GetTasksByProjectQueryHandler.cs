using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Projects;

namespace TaskManager.Application.UseCases.Tasks.GetTasksByProject;

public sealed class GetTasksByProjectQueryHandler(
    ITaskItemRepository taskItemRepository,
    IProjectRepository projectRepository) : IRequestHandler<GetTasksByProjectQuery, Result<IReadOnlyList<TaskListItemResponse>>>
{
    public async Task<Result<IReadOnlyList<TaskListItemResponse>>> Handle(
        GetTasksByProjectQuery request,
        CancellationToken cancellationToken)
    {
        if (!await projectRepository.ExistsAsync(request.ProjectId, cancellationToken))
        {
            return Result.Failure<IReadOnlyList<TaskListItemResponse>>(ProjectErrors.NotFound(request.ProjectId));
        }

        var tasks = await taskItemRepository.ListAsync(t => t.ProjectId == request.ProjectId, cancellationToken);

        var response = tasks
            .Select(t => new TaskListItemResponse(t.Id, t.Title, t.Description, t.Status, t.DueDate, t.ProjectId))
            .ToList();

        return Result.Success<IReadOnlyList<TaskListItemResponse>>(response);
    }
}
