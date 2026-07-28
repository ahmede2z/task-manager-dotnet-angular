using MediatR;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Projects.DeleteProject;

public sealed record DeleteProjectCommand(int Id) : IRequest<Result>;
