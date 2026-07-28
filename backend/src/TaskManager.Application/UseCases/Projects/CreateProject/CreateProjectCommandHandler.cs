using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Projects.CreateProject;

public sealed class CreateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProjectCommand, Result<CreateProjectResponse>>
{
    public async Task<Result<CreateProjectResponse>> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Name = request.Name,
            Description = request.Description
        };

        await projectRepository.AddAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreateProjectResponse(project.Id, project.Name, project.Description, project.CreatedAt));
    }
}
