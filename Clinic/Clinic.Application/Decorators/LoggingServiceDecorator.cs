using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Clinic.Application.Attributes;

namespace Clinic.Application.Filters;

public class LoggingActionFilter(ILogger<LoggingActionFilter> logger) : IActionFilter
{
    private readonly ILogger<LoggingActionFilter> _logger = logger;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var attribute = GetLoggingAttribute(context);
        if (attribute == null) return;

        var entityName = GetEntityName(context);
        var operationName = attribute.OperationName.Replace("{Entity}", entityName);

        context.HttpContext.Items["LoggingStopwatch"] = attribute.LogExecutionTime ? Stopwatch.StartNew() : null;
        context.HttpContext.Items["LoggingOperationName"] = operationName;
        context.HttpContext.Items["LoggingAttribute"] = attribute;

        _logger.Log(attribute.LogLevel, "🚀 Starting: {OperationName}", operationName);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var operationName = context.HttpContext.Items["LoggingOperationName"] as string;
        var attribute = context.HttpContext.Items["LoggingAttribute"] as LoggingAttribute;
        if (attribute == null || operationName == null) return;

        var stopwatch = context.HttpContext.Items["LoggingStopwatch"] as Stopwatch;

        if (context.Exception != null)
        {
            _logger.LogError(context.Exception, "Error in: {OperationName}", operationName);
        }
        else
        {
            if (attribute.LogExecutionTime && stopwatch != null)
            {
                stopwatch.Stop();
                _logger.Log(attribute.LogLevel, "Completed: {OperationName} in {ElapsedMs}ms",
                    operationName, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.Log(attribute.LogLevel, "Completed: {OperationName}", operationName);
            }
        }
    }

    private static LoggingAttribute? GetLoggingAttribute(FilterContext context)
    {
        if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
        {
            var methodAttribute = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(LoggingAttribute), false)
                .FirstOrDefault() as LoggingAttribute;

            if (methodAttribute != null) return methodAttribute;

            var controllerAttribute = actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(LoggingAttribute), false)
                .FirstOrDefault() as LoggingAttribute;

            return controllerAttribute;
        }

        return null;
    }

    private static string GetEntityName(FilterContext context)
    {
        if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
        {
            var controllerName = actionDescriptor.ControllerTypeInfo.Name;
            return controllerName.Replace("Controller", "");
        }

        return "Unknown";
    }
}