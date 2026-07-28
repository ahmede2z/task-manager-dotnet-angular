using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Tasks.GetTaskById;

public sealed class GetTaskByIdQueryHandler(
    ITaskItemRepository taskItemRepository,
    IProjectRepository projectRepository) : IRequestHandler<GetTaskByIdQuery, Result<TaskResponse>>
{
    public async Task<Result<TaskResponse>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await taskItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (task is null)
        {
            return Result.Failure<TaskResponse>(TaskErrors.NotFound(request.Id));
        }

        var project = await projectRepository.GetByIdAsync(task.ProjectId, cancellationToken);

        return Result.Success(new TaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.DueDate,
            project!.Name));
    }
}
