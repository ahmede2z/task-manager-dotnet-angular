using MediatR;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Projects.GetAllProjects;

public sealed record GetAllProjectsQuery : IRequest<Result<IReadOnlyList<ProjectListItemResponse>>>;
