namespace Clinic.RabbitMq.Producer.Configuration;

/// <summary>
/// Configuration options for RabbitMQ publishing behavior.
/// </summary>
public class RabbitMqOptions
{
    /// <summary>
    /// Exchange name to publish messages into.
    /// </summary>
    public string Exchange { get; set; } = "clinic-exchange";

    /// <summary>
    /// Exchange type.
    /// </summary>
    public string ExchangeType { get; set; } = "direct";

    /// <summary>
    /// Maximum retry attempts for batch publishing.
    /// </summary>
    public int PublishMaxRetries { get; set; } = 3;

    /// <summary>
    /// Batch size for generated messages.
    /// </summary>
    public int BatchSize { get; set; } = 10;

    /// <summary>
    /// Time interval between message generation batches in milliseconds.
    /// </summary>
    public int BatchIntervalMs { get; set; } = 5000;

    /// <summary>
    /// Recovery interval in seconds.
    /// </summary>
    public int NetworkRecoveryIntervalSeconds { get; set; } = 5;

    /// <summary>
    /// Timeout for publisher confirms in milliseconds.
    /// </summary>
    public int PublisherConfirmTimeoutMs { get; set; } = 5000;

    /// <summary>
    /// Queue name for reponse messages.
    /// </summary>
    public string ResponseQueue { get; set; } = "entity-generation-responses";
}