using MediatR;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Projects.UpdateProject;

public sealed record UpdateProjectCommand(int Id, string Name, string? Description) : IRequest<Result>;
