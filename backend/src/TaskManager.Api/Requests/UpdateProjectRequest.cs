namespace TaskManager.Api.Requests;

/// <summary>
/// Request to update a project.
/// </summary>
public sealed record UpdateProjectRequest(string Name, string? Description);
