using MediatR;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Projects.GetProjectById;

public sealed record GetProjectByIdQuery(int Id) : IRequest<Result<ProjectResponse>>;
