namespace Clinic.Application.Dtos.RabbitMq;

/// <summary>
/// Standardized message envelope for publish operations
/// </summary>
public record MessageEnvelope
{
    /// <summary>
    /// Unique message identifier
    /// </summary>
    public string MessageId { get; init; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Logical type name of the payload
    /// </summary>
    public string MessageType { get; init; } = string.Empty;

    /// <summary>
    /// Timestamp of message creation
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Payload object to publish
    /// </summary>
    public object Payload { get; init; } = new { };

    /// <summary>
    /// Hash of the payload for data integrity verification and tracking
    /// </summary>
    public string? PayloadHash { get; set; } = string.Empty;
}