namespace Clinic.RabbitMq.Consumer.Configuration;

/// <summary>
/// Configuration options for RabbitMQ consumer behavior and queue mappings.
/// </summary>
public class RabbitMqConsumerOptions
{
    /// <summary>
    /// Gets or sets the RabbitMQ exchange name for message routing.
    /// Defaults to "clinic-exchange".
    /// </summary>
    public string Exchange { get; set; } = "clinic-exchange";

    /// <summary>
    /// Gets or sets the RabbitMQ exchange type for message routing.
    /// Defaults to "direct".
    /// </summary>
    public string ExchangeType { get; set; } = "direct";

    /// <summary>
    /// Gets or sets the mapping of queue names to entity types.
    /// Key represents the queue name, value represents the entity type.
    /// </summary>
    public Dictionary<string, string> Queues { get; set; } = new()
    {
        ["specialization.create"] = "specialization",
        ["patient.create"] = "patient",
        ["doctor.create"] = "doctor",
        ["appointment.create"] = "appointment"
    };

    /// <summary>
    /// Gets or sets the response queue name for entity creation confirmations.
    /// Defaults to "entity-generation-responses".
    /// </summary>
    public string ResponseQueue { get; set; } = "entity-generation-responses";

    /// <summary>
    /// Gets or sets whether response messages should be persistent.
    /// Defaults to true.
    /// </summary>
    public bool PersistentResponses { get; set; } = true;
}