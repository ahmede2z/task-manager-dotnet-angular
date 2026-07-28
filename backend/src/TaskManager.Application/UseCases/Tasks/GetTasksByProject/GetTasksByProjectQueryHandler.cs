using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Projects;
using TaskManager.Application.UseCases.Tasks;

namespace TaskManager.Application.UseCases.Tasks.GetTasksByProject;

public sealed class GetTasksByProjectQueryHandler(
    ITaskItemRepository taskItemRepository,
    IProjectRepository projectRepository) : IRequestHandler<GetTasksByProjectQuery, Result<IReadOnlyList<TaskListItemResponse>>>
{
    public async Task<Result<IReadOnlyList<TaskListItemResponse>>> Handle(
        GetTasksByProjectQuery request,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
        {
            return Result.Failure<IReadOnlyList<TaskListItemResponse>>(ProjectErrors.NotFound(request.ProjectId));
        }

        var tasks = await taskItemRepository.ListAsync(t => t.ProjectId == request.ProjectId, cancellationToken);

        var response = tasks
            .Select(t => new TaskListItemResponse(t.Id, t.Title, t.Description, t.Status, t.DueDate, project.Name))
            .ToList();

        return Result.Success<IReadOnlyList<TaskListItemResponse>>(response);
    }
}
