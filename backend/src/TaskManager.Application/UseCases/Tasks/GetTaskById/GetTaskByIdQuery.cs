using MediatR;
using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Tasks.GetTaskById;

public sealed record GetTaskByIdQuery(int Id) : IRequest<Result<TaskResponse>>;
