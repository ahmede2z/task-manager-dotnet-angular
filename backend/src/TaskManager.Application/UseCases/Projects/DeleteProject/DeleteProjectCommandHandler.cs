using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Projects.DeleteProject;

public sealed class DeleteProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProjectCommand, Result>
{
    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.Id, cancellationToken);

        if (project is null)
        {
            return Result.Failure(ProjectErrors.NotFound(request.Id));
        }

        projectRepository.Remove(project);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
