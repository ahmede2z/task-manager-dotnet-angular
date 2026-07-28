namespace TaskManager.Api.Requests;

/// <summary>
/// Request to create a new project.
/// </summary>
public sealed record CreateProjectRequest(string Name, string? Description);
