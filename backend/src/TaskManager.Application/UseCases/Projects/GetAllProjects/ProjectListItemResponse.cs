namespace TaskManager.Application.UseCases.Projects.GetAllProjects;

public sealed record ProjectListItemResponse(int Id, string Name, string? Description, DateTime CreatedAt);
