using MediatR;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Projects.CreateProject;

public sealed record CreateProjectCommand(string Name, string? Description) : IRequest<Result<CreateProjectResponse>>;
