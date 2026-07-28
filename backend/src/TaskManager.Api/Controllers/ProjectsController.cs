using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Requests;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Projects.CreateProject;
using TaskManager.Application.UseCases.Projects.DeleteProject;
using TaskManager.Application.UseCases.Projects.GetAllProjects;
using TaskManager.Application.UseCases.Projects.GetProjectById;
using TaskManager.Application.UseCases.Projects.UpdateProject;

namespace TaskManager.Api.Controllers;

/// <summary>
/// Manages project operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ISender _mediator;

    /// <summary>
    /// Initializes a new instance of the ProjectsController class.
    /// </summary>
    public ProjectsController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all projects.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ProjectListItemResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllProjectsQuery(), cancellationToken);
        return Ok(result.Value);
    }

    /// <summary>
    /// Get a project by ID.
    /// </summary>
    [HttpGet("{id:int:min(1)}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            return MapResultToResponse(result);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Create a new project.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        CreateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProjectCommand(request.Name, request.Description);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return MapResultToResponse(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Update a project.
    /// </summary>
    [HttpPut("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        UpdateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProjectCommand(id, request.Name, request.Description);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return MapResultToResponse(result);
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a project.
    /// </summary>
    [HttpDelete("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteProjectCommand(id), cancellationToken);

        if (result.IsFailure)
        {
            return MapResultToResponse(result);
        }

        return NoContent();
    }

    private IActionResult MapResultToResponse(Result result)
    {
        return result.Error!.Type switch
        {
            ErrorType.NotFound => NotFound(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Not found",
                Status = StatusCodes.Status404NotFound,
                Detail = result.Error.Message
            }),
            ErrorType.Validation => BadRequest(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Validation failed",
                Status = StatusCodes.Status400BadRequest,
                Detail = result.Error.Message
            }),
            ErrorType.Conflict => Conflict(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                Title = "Conflict",
                Status = StatusCodes.Status409Conflict,
                Detail = result.Error.Message
            }),
            ErrorType.Unauthorized => Unauthorized(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                Title = "Unauthorized",
                Status = StatusCodes.Status401Unauthorized,
                Detail = result.Error.Message
            }),
            ErrorType.Forbidden => Forbid(),
            _ => StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "An error occurred",
                Status = StatusCodes.Status500InternalServerError,
                Detail = result.Error.Message
            })
        };
    }
}
