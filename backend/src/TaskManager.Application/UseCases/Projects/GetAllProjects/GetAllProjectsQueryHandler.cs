using MediatR;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Projects.GetAllProjects;

public sealed class GetAllProjectsQueryHandler(IProjectRepository projectRepository)
    : IRequestHandler<GetAllProjectsQuery, Result<IReadOnlyList<ProjectListItemResponse>>>
{
    public async Task<Result<IReadOnlyList<ProjectListItemResponse>>> Handle(
        GetAllProjectsQuery request,
        CancellationToken cancellationToken)
    {
        var projects = await projectRepository.ListAsync(predicate: null, cancellationToken);

        var response = projects
            .Select(p => new ProjectListItemResponse(p.Id, p.Name, p.Description, p.CreatedAt))
            .ToList();

        return Result.Success<IReadOnlyList<ProjectListItemResponse>>(response);
    }
}
