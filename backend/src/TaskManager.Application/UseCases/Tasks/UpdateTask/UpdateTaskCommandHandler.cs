using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Projects;

namespace TaskManager.Application.UseCases.Tasks.UpdateTask;

public sealed class UpdateTaskCommandHandler(
    ITaskItemRepository taskItemRepository,
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateTaskCommand, Result>
{
    public async Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.Id));
        }

        if (request.ProjectId != task.ProjectId
            && !await projectRepository.ExistsAsync(request.ProjectId, cancellationToken))
        {
            return Result.Failure(ProjectErrors.NotFound(request.ProjectId));
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.DueDate = request.DueDate;
        task.ProjectId = request.ProjectId;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
