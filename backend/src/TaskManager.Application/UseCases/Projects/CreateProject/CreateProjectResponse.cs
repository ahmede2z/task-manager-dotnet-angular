namespace TaskManager.Application.UseCases.Projects.CreateProject;

public sealed record CreateProjectResponse(int Id, string Name, string? Description, DateTime CreatedAt);
