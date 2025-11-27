using Clinic.RabbitMq.Producer.Configuration;
using Clinic.Application.Dtos.RabbitMq;
using Clinic.DataGenerator.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Clinic.RabbitMq.Producer.Publishers;

/// <summary>
/// RabbitMQ producer service for generating and publishing test data
/// </summary>
public class RabbitMqProducer(
        IConnectionFactory connectionFactory,
        ILogger<RabbitMqProducer> logger,
        IOptions<RabbitMqOptions> options,
        DataGeneratorService dataGeneratorService,
        EntityIdTracker idTracker) : BackgroundService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

/// <summary>
/// Executes the main producer loop
/// </summary>
/// <param name="stoppingToken">Cancellation token</param>
/// <returns>Task representing the asynchronous operation</returns>
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var connection = await connectionFactory.CreateConnectionAsync(stoppingToken);
                await using var responseChannel = await connection.CreateChannelAsync(null, stoppingToken);
                var publishChannels = new Dictionary<string, IChannel>();

                try
                {
                    await SetupResponseConsumer(responseChannel, stoppingToken);

                    publishChannels = await CreatePublishChannels(connection, stoppingToken);

                    await GenerateAndPublishSequence(publishChannels, stoppingToken);

                    logger.LogInformation("Completed generation cycle, waiting for next batch");
                    await Task.Delay(options.Value.BatchIntervalMs, stoppingToken);
                }
                finally
                {
                    await CleanupChannels(publishChannels);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Generation cycle failed, retrying in 10 seconds");
                await Task.Delay(10000, stoppingToken);
            }
        }
    }

    /// <summary>
    /// Sets up the response consumer for receiving entity creation confirmations
    /// </summary>
    /// <param name="channel">Channel for response consumption</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    private async Task SetupResponseConsumer(IChannel channel, CancellationToken cancellationToken)
    {
        await channel.QueueDeclareAsync(
            queue: options.Value.ResponseQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var response = JsonSerializer.Deserialize<EntityCreatedResponse>(ea.Body.Span, _jsonOptions);
                if (response != null)
                {
                    idTracker.RegisterCreatedId(response.EntityType, response.GeneratedId, response.OriginalDataHash);
                    logger.LogInformation("Received response for {EntityType} with ID {Id}", response.EntityType, response.GeneratedId);
                }

                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process response message");
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: cancellationToken);
            }
        };

        await channel.BasicConsumeAsync(
            queue: options.Value.ResponseQueue,
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Creates publish channels for different entity types
    /// </summary>
    /// <param name="connection">RabbitMQ connection</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Dictionary of routing keys to channels</returns>
    private async Task<Dictionary<string, IChannel>> CreatePublishChannels(IConnection connection, CancellationToken cancellationToken)
    {
        var channels = new Dictionary<string, IChannel>();
        var entityTypes = new[] { "specialization.create", "patient.create", "doctor.create", "appointment.create" };

        foreach (var routing in entityTypes)
        {
            var channel = await connection.CreateChannelAsync(null, cancellationToken);

            await channel.ExchangeDeclareAsync(
                exchange: options.Value.Exchange,
                type: options.Value.ExchangeType,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken);

            channels[routing] = channel;
        }

        return channels;
    }

    /// <summary>
    /// Generates and publishes entities in the correct dependency order
    /// </summary>
    /// <param name="channels">Publish channels dictionary</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    private async Task GenerateAndPublishSequence(Dictionary<string, IChannel> channels, CancellationToken cancellationToken)
    {
        await ProcessSpecializations(channels["specialization.create"], cancellationToken);
        await ProcessPatients(channels["patient.create"], cancellationToken);
        await ProcessDoctors(channels["doctor.create"], cancellationToken);
        await ProcessAppointments(channels["appointment.create"], cancellationToken);
    }

    /// <summary>
    /// Processes and publishes specialization entities
    /// </summary>
    /// <param name="channel">Publish channel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    private async Task ProcessSpecializations(IChannel channel, CancellationToken cancellationToken)
    {
        var availableCount = dataGeneratorService.SpecializationsRemainingCount;
        if (availableCount == 0)
        {
            logger.LogInformation("All unique specializations have been generated, skipping");
            return;
        }

        var toGenerate = Math.Min(options.Value.BatchSize, availableCount);
        logger.LogInformation("Generating {Count} of {Available} available specializations",
            toGenerate, availableCount);

        var specializations = dataGeneratorService.GenerateSpecializations(toGenerate);

        if (specializations.Any())
        {
            await PublishBatchWithTracking(channel, "specialization", "specialization.create", specializations, cancellationToken);
            await WaitForEntityCreation("specialization", toGenerate, specializations, cancellationToken);
        }
    }

    /// <summary>
    /// Processes and publishes patient entities
    /// </summary>
    /// <param name="channel">Publish channel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    private async Task ProcessPatients(IChannel channel, CancellationToken cancellationToken)
    {
        var patients = dataGeneratorService.GeneratePatients(options.Value.BatchSize);
        await PublishBatchWithTracking(channel, "patient", "patient.create", patients, cancellationToken);
        await WaitForEntityCreation("patient", options.Value.BatchSize, patients, cancellationToken);
    }


    /// <summary>
    /// Processes and publishes doctor entities
    /// </summary>
    /// <param name="channel">Publish channel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    private async Task ProcessDoctors(IChannel channel, CancellationToken cancellationToken)
    {
        var specIds = idTracker.GetIds("specialization");
        logger.LogInformation("Available specialization IDs for doctors: {Count}", specIds.Count);

        if (!specIds.Any())
        {
            logger.LogWarning("No specialization IDs available for generating doctors");
            return;
        }

        var doctors = dataGeneratorService.GenerateDoctors(options.Value.BatchSize);
        await PublishBatchWithTracking(channel, "doctor", "doctor.create", doctors, cancellationToken);
        await WaitForEntityCreation("doctor", options.Value.BatchSize, doctors, cancellationToken);
    }

    /// <summary>
    /// Processes and publishes appointment entities
    /// </summary>
    /// <param name="channel">Publish channel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    private async Task ProcessAppointments(IChannel channel, CancellationToken cancellationToken)
    {
        var doctorIds = idTracker.GetIds("doctor");
        var patientIds = idTracker.GetIds("patient");

        if (!doctorIds.Any() || !patientIds.Any())
        {
            logger.LogWarning("Cannot create appointments - missing dependencies. Doctors: {DoctorCount}, Patients: {PatientCount}",
                doctorIds.Count, patientIds.Count);
            return;
        }

        logger.LogInformation("Creating appointments with {DoctorCount} doctors and {PatientCount} patients",
            doctorIds.Count, patientIds.Count);

        var appointments = dataGeneratorService.GenerateAppointments(options.Value.BatchSize);
        await PublishBatchWithTracking(channel, "appointment", "appointment.create", appointments, cancellationToken);
        await WaitForEntityCreation("appointment", options.Value.BatchSize, appointments, cancellationToken);
    }

    /// <summary>
    /// Publishes a batch of entities with tracking for response correlation
    /// </summary>
    /// <param name="channel">Publish channel</param>
    /// <param name="entityType">Type of entity being published</param>
    /// <param name="routingKey">Routing key for publishing</param>
    /// <param name="batch">Batch of entities to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    private async Task PublishBatchWithTracking(IChannel channel, string entityType, string routingKey, List<object> batch, CancellationToken cancellationToken)
    {
        if (!batch.Any()) return;

        try
        {
            await channel.TxSelectAsync(cancellationToken);

            foreach (var item in batch)
            {
                var payloadJson = JsonSerializer.Serialize(item, _jsonOptions);
                var hash = ComputeDataHashFromString(payloadJson);

                idTracker.RegisterExpectedEntity(entityType, hash, item);

                var envelope = new MessageEnvelope
                {
                    MessageId = Guid.NewGuid().ToString(),
                    MessageType = item.GetType().Name,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Payload = JsonSerializer.Deserialize<object>(payloadJson, _jsonOptions)!,
                    PayloadHash = hash
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(envelope, _jsonOptions));

                await channel.BasicPublishAsync(
                    exchange: options.Value.Exchange,
                    routingKey: routingKey,
                    mandatory: false,
                    basicProperties: new BasicProperties
                    {
                        Persistent = true,
                        Headers = new Dictionary<string, object?>
                        {
                            ["ResponseQueue"] = options.Value.ResponseQueue
                        }
                    },
                    body: body,
                    cancellationToken: cancellationToken);
            }

            await channel.TxCommitAsync(cancellationToken);
            logger.LogInformation("Published batch of {Count} messages for {RoutingKey}", batch.Count, routingKey);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            foreach (var item in batch)
            {
                var payloadJson = JsonSerializer.Serialize(item, _jsonOptions);
                var hash = ComputeDataHashFromString(payloadJson);
                idTracker.ReturnToAvailable(entityType, hash);
            }

            logger.LogError(ex, "Publishing failed for {RoutingKey}, rolling back", routingKey);
            await channel.TxRollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// Waits for entity creation confirmations
    /// </summary>
    /// <param name="entityType">Type of entity to wait for</param>
    /// <param name="expectedCount">Expected number of entities</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    /// <exception cref="TimeoutException">Thrown when waiting times out</exception>
    private async Task WaitForEntityCreation(string entityType, int expectedCount, List<object> batch, CancellationToken cancellationToken)
    {
        for (var attempt = 1; attempt <= options.Value.PublishMaxRetries; attempt++)
        {
            try
            {
                var timeout = TimeSpan.FromMilliseconds(options.Value.PublisherConfirmTimeoutMs * attempt);
                var startTime = DateTime.UtcNow;

                while (DateTime.UtcNow - startTime < timeout)
                {
                    var createdCount = idTracker.GetCreatedCount(entityType);
                    if (createdCount >= expectedCount)
                    {
                        logger.LogInformation("Successfully received {Count} {EntityType} IDs", createdCount, entityType);
                        return;
                    }
                    await Task.Delay(100, cancellationToken);
                }

                logger.LogWarning("Attempt {Attempt} timed out waiting for {EntityType} entities. Received {Received}/{Expected}",
                    attempt, entityType, idTracker.GetCreatedCount(entityType), expectedCount);
            }
            catch (OperationCanceledException)
            {
                ReturnBatchToAvailable(entityType, batch);
                logger.LogWarning("Waiting for {EntityType} entities was cancelled", entityType);
                throw;
            }
        }
        ReturnBatchToAvailable(entityType, batch);
        throw new TimeoutException($"Failed to receive {expectedCount} entities after {options.Value.PublishMaxRetries} attempts");
    }

    private void ReturnBatchToAvailable(string entityType, List<object> batch)
    {
        logger.LogWarning("Returning {Count} {EntityType} entities to available state", batch.Count, entityType);

        foreach (var item in batch)
        {
            var payloadJson = JsonSerializer.Serialize(item, _jsonOptions);
            var hash = ComputeDataHashFromString(payloadJson);
            idTracker.ReturnToAvailable(entityType, hash);
        }
    }

    /// <summary>
    /// Computes SHA256 hash from string data
    /// </summary>
    /// <param name="json">JSON string to hash</param>
    /// <returns>Base64 encoded hash string</returns>
    private static string ComputeDataHashFromString(string json)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Cleans up publish channels
    /// </summary>
    /// <param name="publishChannels">Dictionary of channels to clean up</param>
    /// <returns>Task representing the asynchronous operation</returns>
    private async Task CleanupChannels(Dictionary<string, IChannel> publishChannels)
    {
        foreach (var channel in publishChannels.Values)
        {
            try
            {
                await channel.DisposeAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error disposing channel");
            }
        }
    }
}