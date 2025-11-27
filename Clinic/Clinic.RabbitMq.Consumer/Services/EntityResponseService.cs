using Clinic.Application.Dtos.RabbitMq;
using Clinic.RabbitMq.Consumer.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

/// <summary>
/// Service for sending entity creation response messages
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
    /// Sends an entity created response message
    /// </summary>
    /// <param name="entityType">Type of the created entity</param>
    /// <param name="generatedId">Generated GUID for the entity</param>
    /// <param name="originalDataHash">Hash of the original data for correlation</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public async Task SendEntityCreatedResponse(string entityType, Guid generatedId, string originalDataHash)
    {
        try
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: options.Value.ResponseQueue,
                durable: true,
                exclusive: false,
                autoDelete: false);

            var response = new EntityCreatedResponse
            {
                EntityType = entityType,
                GeneratedId = generatedId,
                OriginalDataHash = originalDataHash
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

            logger.LogDebug("Sent response for {EntityType} with ID {Id} to {ResponseQueue}",
                entityType, generatedId, options.Value.ResponseQueue);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send response for {EntityType} to {ResponseQueue}",
                entityType, options.Value.ResponseQueue);
            throw;
        }
    }

    /// <summary>
    /// Computes SHA256 hash from an object for data integrity verification
    /// </summary>
    /// <param name="obj">Object to compute hash for</param>
    /// <returns>Base64 encoded hash string</returns>
    public static string ComputeDataHash(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(hash);
    }
}