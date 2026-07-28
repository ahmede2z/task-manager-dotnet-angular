using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Tasks;

public static class TaskErrors
{
    public static Error NotFound(int id) => new(
        "TaskItem.NotFound",
        $"Task with id '{id}' was not found.",
        ErrorType.NotFound);
}
