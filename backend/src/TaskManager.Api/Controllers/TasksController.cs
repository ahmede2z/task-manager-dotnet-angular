using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Requests;
using TaskManager.Application.Common;
using TaskManager.Application.UseCases.Tasks;
using TaskManager.Application.UseCases.Tasks.CreateTask;
using TaskManager.Application.UseCases.Tasks.DeleteTask;
using TaskManager.Application.UseCases.Tasks.GetAllTasks;
using TaskManager.Application.UseCases.Tasks.GetTaskById;
using TaskManager.Application.UseCases.Tasks.GetTasksByProject;
using TaskManager.Application.UseCases.Tasks.GetTasksByStatus;
using TaskManager.Application.UseCases.Tasks.UpdateTask;

namespace TaskManager.Api.Controllers;

/// <summary>
/// Manages task operations.
/// </summary>
[ApiController]
[Route("api/tasks")]
public class TasksController : ApiControllerBase
{
    private readonly ISender _mediator;

    /// <summary>
    /// Initializes a new instance of the TasksController class.
    /// </summary>
    public TasksController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all tasks, optionally filtered by status.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(TaskListItemResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] TaskStatus? status, CancellationToken cancellationToken)
    {
        if (status is null)
        {
            var result = await _mediator.Send(new GetAllTasksQuery(), cancellationToken);
            return Ok(result.Value);
        }

        var byStatus = await _mediator.Send(new GetTasksByStatusQuery(status.Value), cancellationToken);
        if (byStatus.IsFailure)
        {
            return MapResultToResponse(byStatus);
        }

        return Ok(byStatus.Value);
    }

    /// <summary>
    /// Get all tasks for a project.
    /// </summary>
    [HttpGet("/api/projects/{projectId:int:min(1)}/tasks")]
    [ProducesResponseType(typeof(TaskListItemResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProject(int projectId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTasksByProjectQuery(projectId), cancellationToken);
        if (result.IsFailure)
        {
            return MapResultToResponse(result);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Get a task by ID.
    /// </summary>
    [HttpGet("{id:int:min(1)}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTaskByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            return MapResultToResponse(result);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Create a new task.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateTaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(
        CreateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTaskCommand(
            request.Title,
            request.Description,
            request.Status,
            request.DueDate,
            request.ProjectId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return MapResultToResponse(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Update a task.
    /// </summary>
    [HttpPut("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        UpdateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTaskCommand(
            id,
            request.Title,
            request.Description,
            request.Status,
            request.DueDate,
            request.ProjectId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return MapResultToResponse(result);
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a task.
    /// </summary>
    [HttpDelete("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteTaskCommand(id), cancellationToken);

        if (result.IsFailure)
        {
            return MapResultToResponse(result);
        }

        return NoContent();
    }
}
