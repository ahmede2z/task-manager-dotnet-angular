using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common;

namespace TaskManager.Api.Controllers;

/// <summary>
/// Base controller providing shared Result-to-ProblemDetails mapping.
/// </summary>
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// Maps a failed Result to the corresponding ProblemDetails response.
    /// </summary>
    protected IActionResult MapResultToResponse(Result result)
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
