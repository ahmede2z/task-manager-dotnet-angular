using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Projects;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Tasks.CreateTask;

public sealed class CreateTaskCommandHandler(
    ITaskItemRepository taskItemRepository,
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTaskCommand, Result<CreateTaskResponse>>
{
    public async Task<Result<CreateTaskResponse>> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken)
    {
        if (!await projectRepository.ExistsAsync(request.ProjectId, cancellationToken))
        {
            return Result.Failure<CreateTaskResponse>(ProjectErrors.NotFound(request.ProjectId));
        }

        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId
        };

        await taskItemRepository.AddAsync(task, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreateTaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.DueDate,
            task.ProjectId));
    }
}
