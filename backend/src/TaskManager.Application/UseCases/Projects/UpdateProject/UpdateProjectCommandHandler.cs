using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Projects.UpdateProject;

public sealed class UpdateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProjectCommand, Result>
{
    public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.Id, cancellationToken);

        if (project is null)
        {
            return Result.Failure(ProjectErrors.NotFound(request.Id));
        }

        project.Name = request.Name;
        project.Description = request.Description;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
