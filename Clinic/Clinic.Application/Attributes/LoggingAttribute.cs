using Microsoft.Extensions.Logging;

namespace Clinic.Application.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class LoggingAttribute : Attribute
{
    public string OperationName { get; }
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
    public bool LogExecutionTime { get; set; } = true;

    public LoggingAttribute(string operationName)
    {
        OperationName = operationName;
    }
}