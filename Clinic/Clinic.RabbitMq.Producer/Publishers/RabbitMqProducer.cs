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
            var bodySpan = ea.Body.ToArray();
            var response = JsonSerializer.Deserialize<EntityResponse>(bodySpan, _jsonOptions);

            if (response == null)
            {
                logger.LogWarning("Received unknown message format in response queue");
                return;
            }

            if (response.Success && response.GeneratedId.HasValue)
            {
                idTracker.RegisterCreatedId(response.EntityType, response.GeneratedId.Value, response.PayloadHash);
                logger.LogInformation("Received response for {EntityType} with ID {Id}",
                    response.EntityType, response.GeneratedId);
            }
            else
            {
                if (response.EntityType == "appointment" && response.AdditionalData != null)
                {
                    // Удаляем невалидные DoctorId
                    if (response.AdditionalData.TryGetValue("InvalidDoctorIds", out var doctorIdsObj))
                    {
                        if (doctorIdsObj is System.Text.Json.JsonElement doctorIdsElement)
                        {
                            try
                            {
                                var invalidDoctorIds = doctorIdsElement.Deserialize<List<Guid>>(_jsonOptions);
                                if (invalidDoctorIds != null)
                                {
                                    foreach (var doctorId in invalidDoctorIds)
                                    {
                                        idTracker.RemoveCreatedIdByGuid("doctor", doctorId);
                                        logger.LogWarning("Removed invalid DoctorId {Id} from tracker", doctorId);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "Failed to deserialize InvalidDoctorIds");
                            }
                        }
                    }

                    if (response.AdditionalData.TryGetValue("InvalidPatientIds", out var patientIdsObj))
                    {
                        if (patientIdsObj is System.Text.Json.JsonElement patientIdsElement)
                        {
                            try
                            {
                                var invalidPatientIds = patientIdsElement.Deserialize<List<Guid>>(_jsonOptions);
                                if (invalidPatientIds != null)
                                {
                                    foreach (var patientId in invalidPatientIds)
                                    {
                                        idTracker.RemoveCreatedIdByGuid("patient", patientId);
                                        logger.LogWarning("Removed invalid PatientId {Id} from tracker", patientId);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "Failed to deserialize InvalidPatientIds");
                            }
                        }
                    }
                }

                idTracker.ReturnToAvailable(response.EntityType, response.PayloadHash);
                logger.LogWarning("Received invalid response for {EntityType}, reason: {Reason}",
                    response.EntityType, response.Reason);
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

    private async Task GenerateAndPublishSequence(Dictionary<string, IChannel> channels, CancellationToken cancellationToken)
    {
        await ProcessSpecializations(channels["specialization.create"], cancellationToken);
        await ProcessPatients(channels["patient.create"], cancellationToken);
        await ProcessDoctors(channels["doctor.create"], cancellationToken);
        await ProcessAppointments(channels["appointment.create"], cancellationToken);
    }

    private async Task ProcessSpecializations(IChannel channel, CancellationToken cancellationToken)
    {
        var availableCount = dataGeneratorService.SpecializationsRemainingCount;
        if (availableCount == 0)
        {
            logger.LogInformation("All unique specializations have been generated, skipping");
            return;
        }

        var toGenerate = Math.Min(options.Value.BatchSize, availableCount);
        logger.LogInformation("Generating {Count} of {Available} available specializations", toGenerate, availableCount);

        var specializations = dataGeneratorService.GenerateSpecializations(toGenerate);

        if (specializations.Count() != 0)
        {
            await PublishBatchWithTracking(channel, "specialization", "specialization.create", specializations, cancellationToken);
            await WaitForEntityCreation("specialization", specializations, cancellationToken);
        }
    }

    private async Task ProcessPatients(IChannel channel, CancellationToken cancellationToken)
    {
        var patients = dataGeneratorService.GeneratePatients(options.Value.BatchSize);
        await PublishBatchWithTracking(channel, "patient", "patient.create", patients, cancellationToken);
        await WaitForEntityCreation("patient", patients, cancellationToken);
    }

    private async Task ProcessDoctors(IChannel channel, CancellationToken cancellationToken)
    {
        var specIds = idTracker.GetIds("specialization");
        logger.LogInformation("Available specialization IDs for doctors: {Count}", specIds.Count);

        if (specIds.Count() == 0)
        {
            logger.LogWarning("No specialization IDs available for generating doctors");
            return;
        }

        var doctors = dataGeneratorService.GenerateDoctors(options.Value.BatchSize);
        await PublishBatchWithTracking(channel, "doctor", "doctor.create", doctors, cancellationToken);
        await WaitForEntityCreation("doctor", doctors, cancellationToken);
    }

    private async Task ProcessAppointments(IChannel channel, CancellationToken cancellationToken)
    {
        var doctorIds = idTracker.GetIds("doctor");
        var patientIds = idTracker.GetIds("patient");

        if (doctorIds.Count() == 0 || patientIds.Count() == 0)
        {
            logger.LogWarning("Cannot create appointments - missing dependencies. Doctors: {DoctorCount}, Patients: {PatientCount}",
                doctorIds.Count, patientIds.Count);
            return;
        }

        logger.LogInformation("Creating appointments with {DoctorCount} doctors and {PatientCount} patients",
            doctorIds.Count, patientIds.Count);

        var appointments = dataGeneratorService.GenerateAppointments(options.Value.BatchSize);
        await PublishBatchWithTracking(channel, "appointment", "appointment.create", appointments, cancellationToken);
        await WaitForEntityCreation("appointment", appointments, cancellationToken);
    }

    private async Task PublishBatchWithTracking(IChannel channel, string entityType, string routingKey, List<object> batch, CancellationToken cancellationToken)
    {
        if (batch.Count() == 0) return;

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

    private async Task WaitForEntityCreation(string entityType, List<object> batch, CancellationToken cancellationToken)
    {
        var expectedCount = batch.Count;

        for (var attempt = 1; attempt <= options.Value.PublishMaxRetries; attempt++)
        {
            try
            {
                var timeout = TimeSpan.FromMilliseconds(options.Value.PublisherConfirmTimeoutMs * attempt);
                var startTime = DateTime.UtcNow;

                while (DateTime.UtcNow - startTime < timeout)
                {
                    var createdCount = batch.Count(item =>
                    {
                        var json = JsonSerializer.Serialize(item, _jsonOptions);
                        var hash = ComputeDataHashFromString(json);
                        return idTracker.IsCreated(entityType, hash);
                    });

                    if (createdCount == batch.Count)
                    {
                        logger.LogInformation("Successfully received {Count} {EntityType} IDs", createdCount, entityType);
                        return;
                    }
                    await Task.Delay(100, cancellationToken);
                }

                logger.LogWarning("Attempt {Attempt} timed out waiting for {EntityType} entities. Received {Received}/{Expected}",
                    attempt, entityType, idTracker.GetIds(entityType).Count, expectedCount);
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

    private static string ComputeDataHashFromString(string json)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(hash);
    }

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