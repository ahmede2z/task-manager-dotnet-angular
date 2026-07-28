namespace TaskManager.Application.UseCases.Projects.GetProjectById;

public sealed record ProjectResponse(int Id, string Name, string? Description, DateTime CreatedAt);
