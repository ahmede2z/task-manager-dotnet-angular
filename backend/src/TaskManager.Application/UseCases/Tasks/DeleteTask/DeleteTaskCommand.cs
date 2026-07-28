using MediatR;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Tasks.DeleteTask;

public sealed record DeleteTaskCommand(int Id) : IRequest<Result>;
