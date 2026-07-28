using TaskManager.Application.Common;

namespace TaskManager.Application.UseCases.Projects;

public static class ProjectErrors
{
    public static Error NotFound(int id) => new(
        "Project.NotFound",
        $"Project with id '{id}' was not found.",
        ErrorType.NotFound);
}
