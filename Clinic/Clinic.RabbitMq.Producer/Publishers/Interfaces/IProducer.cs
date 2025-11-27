namespace Clinic.RabbitMq.Producer.Publishers.Interfaces;
/// <summary>
/// Abstraction for publishing messages in atomic batches.
/// </summary>
public interface IProducer
{
    /// <summary>
    /// Sends a batch of messages.
    /// </summary>
    Task PublishBatchAsync<T>(
        IEnumerable<T> items,
        string routingKey,
        CancellationToken cancellationToken = default);
}