using System.Text;
using System.Text.Json;

using Clinic.Application.Dtos.RabbitMq;
using Clinic.RabbitMq.Consumer.Configuration;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Clinic.RabbitMq.Consumer.Services;

/// <summary>
/// Service responsible for sending responses about entity creation
/// to the RabbitMQ response queue.
/// </summary>
public class EntityResponseService(
    IConnectionFactory connectionFactory,
    IOptions<RabbitMqConsumerOptions> options,
    ILogger<EntityResponseService> logger)
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

    /// <summary>
    /// Sends a response for an entity creation attempt.
    /// </summary>
    /// <param name="entityType">Type of the entity (e.g., "doctor", "patient").</param>
    /// <param name="payloadHash">Hash of the payload for correlation.</param>
    /// <param name="success">Indicates if creation was successful.</param>
    /// <param name="generatedId">Generated ID if creation succeeded, otherwise null.</param>
    /// <param name="reason">Reason for failure if <paramref name="success"/> is false.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendEntityResponse(
    string entityType,
    string payloadHash,
    bool success,
    Guid? generatedId = null,
    string? reason = null,
    Dictionary<string, object>? additionalData = null)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: options.Value.ResponseQueue,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var response = new EntityResponse
        {
            EntityType = entityType,
            PayloadHash = payloadHash,
            Success = success,
            GeneratedId = generatedId,
            Reason = reason,
            AdditionalData = additionalData
        };

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response, _jsonOptions));

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: options.Value.ResponseQueue,
            mandatory: false,
            basicProperties: new BasicProperties
            {
                Persistent = options.Value.PersistentResponses
            },
            body: body);

        logger.LogDebug(
            success
                ? "Sent success response for {EntityType} with ID {Id}"
                : "Sent failure response for {EntityType}, reason: {Reason}",
            entityType, generatedId, reason);
    }

    /// <summary>
    /// Computes SHA256 hash from an object for data integrity verification.
    /// </summary>
    /// <param name="obj">Object to compute hash for.</param>
    /// <returns>Base64 encoded SHA256 hash string.</returns>
    public static string ComputeDataHash(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(hash);
    }
}
