using System.Diagnostics;

namespace Clinic.Api.Middleware;

/// <summary>
/// Middleware for logging HTTP requests and responses.
/// Logs request details, execution time, and any exceptions that occur during request processing.
/// </summary>
public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    /// <summary>
    /// Processes an HTTP request and logs details about the request, response, and execution time.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString("N")[..8];
        var stopwatch = Stopwatch.StartNew();

        try
        {
            logger.LogInformation("REQUEST_START | ID:{RequestId} | Method:{Method} | Path:{Path} | RemoteIP:{RemoteIp}",
                requestId, context.Request.Method, context.Request.Path, context.Connection.RemoteIpAddress);

            await next(context);

            stopwatch.Stop();

            logger.LogInformation("REQUEST_END | ID:{RequestId} | Method:{Method} | Path:{Path} | Status:{StatusCode} | Time:{ElapsedMs}ms",
                requestId, context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex, "REQUEST_ERROR | ID:{RequestId} | Method:{Method} | Path:{Path} | Status:500 | Time:{ElapsedMs}ms | Error:{ErrorMessage}",
                requestId, context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds, ex.Message);
            throw;
        }
    }
}