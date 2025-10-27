using Clinic.Application.Attributes;
using Clinic.Application.Services;
using Clinic.Application.DTOs;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

public class LoggingServiceDecorator<TService, TDto, TCreateDto, TUpdateDto>(TService decorated, ILogger<TService> logger) : DispatchProxy
    where TService : ICrudService<TDto, TCreateDto, TUpdateDto>
{
    private TService _decorated = decorated;
    private ILogger<TService> _logger = logger;

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        var loggingAttribute = targetMethod.GetCustomAttribute<LoggingAttribute>();
        if (loggingAttribute == null)
        {
            return targetMethod.Invoke(_decorated, args);
        }

        return InvokeWithLogging(targetMethod, args, loggingAttribute);
    }

    private object InvokeWithLogging(MethodInfo targetMethod, object[] args, LoggingAttribute attribute)
    {
        var operationName = attribute.OperationName;
        var serviceName = typeof(TService).Name;

        try
        {
            _logger.Log(attribute.LogLevel, "Starting {OperationName} in {ServiceName}",
                       operationName, serviceName);

            var stopwatch = attribute.LogExecutionTime ? Stopwatch.StartNew() : null;

            var result = targetMethod.Invoke(_decorated, args);

            if (result is Task task)
            {
                return HandleAsyncMethod(task, stopwatch, operationName, serviceName, attribute);
            }

            stopwatch?.Stop();
            if (attribute.LogExecutionTime)
            {
                _logger.Log(attribute.LogLevel,
                           "Completed {OperationName} in {ServiceName} in {ElapsedMs}ms",
                           operationName, serviceName, stopwatch?.ElapsedMilliseconds);
            }
            else
            {
                _logger.Log(attribute.LogLevel, "Completed {OperationName} in {ServiceName}",
                           operationName, serviceName);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during {OperationName} in {ServiceName}",
                            operationName, serviceName);
            throw;
        }
    }

    private async Task HandleAsyncMethod(Task task, Stopwatch stopwatch, string operationName,
                                       string serviceName, LoggingAttribute attribute)
    {
        try
        {
            await task.ConfigureAwait(false);
            stopwatch?.Stop();

            if (attribute.LogExecutionTime)
            {
                _logger.Log(attribute.LogLevel,
                           "Completed {OperationName} in {ServiceName} in {ElapsedMs}ms",
                           operationName, serviceName, stopwatch?.ElapsedMilliseconds);
            }
            else
            {
                _logger.Log(attribute.LogLevel, "Completed {OperationName} in {ServiceName}",
                           operationName, serviceName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during {OperationName} in {ServiceName}",
                            operationName, serviceName);
            throw;
        }
    }

    private async Task<T> HandleAsyncMethod<T>(Task<T> task, Stopwatch stopwatch, string operationName,
                                             string serviceName, LoggingAttribute attribute)
    {
        try
        {
            var result = await task.ConfigureAwait(false);
            stopwatch?.Stop();

            if (attribute.LogExecutionTime)
            {
                _logger.Log(attribute.LogLevel,
                           "Completed {OperationName} in {ServiceName} in {ElapsedMs}ms",
                           operationName, serviceName, stopwatch?.ElapsedMilliseconds);
            }
            else
            {
                _logger.Log(attribute.LogLevel, "Completed {OperationName} in {ServiceName}",
                           operationName, serviceName);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during {OperationName} in {ServiceName}",
                            operationName, serviceName);
            throw;
        }
    }

    public static TService Create(TService decorated, ILogger<TService> logger)
    {
        object proxy = Create<TService, LoggingServiceDecorator<TService, TDto, TCreateDto, TUpdateDto>>();
        ((LoggingServiceDecorator<TService, TDto, TCreateDto, TUpdateDto>)proxy)._decorated = decorated;
        ((LoggingServiceDecorator<TService, TDto, TCreateDto, TUpdateDto>)proxy)._logger = logger;

        return (TService)proxy;
    }
}