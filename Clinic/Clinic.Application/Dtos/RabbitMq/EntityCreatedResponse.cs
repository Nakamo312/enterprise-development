namespace Clinic.Application.Dtos.RabbitMq;

/// <summary>
/// Unified response for entity creation
/// </summary>
public class EntityResponse
{
    /// <summary>
    /// Type of the entity (e.g., "specialization", "doctor")
    /// </summary>
    public required string EntityType { get; set; }

    /// <summary>
    /// Whether the creation succeeded
    /// </summary>
    public required bool Success { get; set; }

    /// <summary>
    /// Generated ID if success
    /// </summary>
    public Guid? GeneratedId { get; set; }

    /// <summary>
    /// Hash of the payload for correlation
    /// </summary>
    public required string PayloadHash { get; set; }

    /// <summary>
    /// Reason for failure if Success == false
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// AdditionalData
    /// </summary>
    public Dictionary<string, object>? AdditionalData { get; set; }
}
