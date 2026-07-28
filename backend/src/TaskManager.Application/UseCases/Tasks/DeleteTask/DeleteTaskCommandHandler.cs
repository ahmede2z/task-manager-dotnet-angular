using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Tasks.DeleteTask;

public sealed class DeleteTaskCommandHandler(ITaskItemRepository taskItemRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTaskCommand, Result>
{
    public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.Id));
        }

        taskItemRepository.Remove(task);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
