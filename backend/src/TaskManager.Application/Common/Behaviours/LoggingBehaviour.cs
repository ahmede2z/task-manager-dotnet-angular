using MediatR;
using Microsoft.Extensions.Logging;

namespace TaskManager.Application.Common.Behaviours;

public sealed class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Handling {RequestName}", requestName);

        var response = await next();

        if (response is Result { IsFailure: true } result)
        {
            logger.LogWarning(
                "{RequestName} failed with {ErrorCode}",
                requestName,
                result.Error?.Code);
        }
        else
        {
            logger.LogInformation("Handled {RequestName}", requestName);
        }

        return response;
    }
}
