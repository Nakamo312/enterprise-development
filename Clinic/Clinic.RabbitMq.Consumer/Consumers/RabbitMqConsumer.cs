using Clinic.RabbitMq.Consumer.Configuration;
using Clinic.Application.Dtos.Appointments;
using Clinic.Application.Dtos.Doctors;
using Clinic.Application.Dtos.Patients;
using Clinic.Application.Dtos.Specializations;
using Clinic.Application.Dtos.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using AutoMapper;
using Clinic.Domain.Models;
using Clinic.Infrastructure.Repositories.Interfaces;

namespace Clinic.RabbitMq.Consumer.Consumers;

/// <summary>
/// RabbitMQ consumer service for processing entity creation messages
/// </summary>
public class RabbitMqConsumer(
        IConnectionFactory connectionFactory,
        EntityResponseService responseService,
        IServiceProvider serviceProvider,
        IOptions<RabbitMqConsumerOptions> options,
        ILogger<RabbitMqConsumer> logger) : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    /// <summary>
    /// Executes the main consumer loop
    /// </summary>
    /// <param name="stoppingToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting RabbitMQ Consumer...");

        _connection = await connectionFactory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(null, stoppingToken);

        await _channel.BasicQosAsync(0, 1, false, stoppingToken);

        await SetupInfrastructureAsync();
        await StartConsumersAsync();

        logger.LogInformation("RabbitMQ Consumer started successfully");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    /// <summary>
    /// Sets up RabbitMQ infrastructure (exchange, queues, bindings)
    /// </summary>
    /// <returns>Task representing the asynchronous operation</returns>
    private async Task SetupInfrastructureAsync()
    {
        if (_channel is null)
            throw new InvalidOperationException("RabbitMQ channel is not initialized");

        await _channel.ExchangeDeclareAsync(
            exchange: options.Value.Exchange,
            type: options.Value.ExchangeType,
            durable: true,
            autoDelete: false);

        foreach (var queueConfig in options.Value.Queues)
        {
            await _channel.QueueDeclareAsync(
                queueConfig.Key,
                durable: true,
                exclusive: false,
                autoDelete: false);

            await _channel.QueueBindAsync(
                queueConfig.Key,
                options.Value.Exchange,
                queueConfig.Key);

            logger.LogDebug("Bound {Queue} to {Exchange}", queueConfig.Key, options.Value.Exchange);
        }
    }

    /// <summary>
    /// Starts consumers for all configured queues
    /// </summary>
    /// <returns>Task representing the asynchronous operation</returns>
    private async Task StartConsumersAsync()
    {
        if (_channel is null)
        {
            throw new InvalidOperationException("RabbitMQ channel is not initialized");
        }

        foreach (var queueConfig in options.Value.Queues)
        {
            var queueName = queueConfig.Key;
            var entityType = queueConfig.Value;

            var consumer = new AsyncEventingBasicConsumer(_channel!);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    await ProcessMessageAsync(queueName, entityType, ea.Body.ToArray());
                    await _channel!.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex,
                        "Failed to process message from {Queue}, requeueing...", queueName);

                    await _channel!.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer);

            logger.LogInformation("Started consumer for {Queue}", queueName);
        }
    }

    /// <summary>
    /// Processes a single message from RabbitMQ
    /// </summary>
    /// <param name="queueName">Name of the source queue</param>
    /// <param name="entityType">Type of entity being processed</param>
    /// <param name="body">Message body bytes</param>
    /// <returns>Task representing the asynchronous operation</returns>
    private async Task ProcessMessageAsync(string queueName, string entityType, byte[] body)
    {
        var bodyString = Encoding.UTF8.GetString(body);

        var envelope = JsonSerializer.Deserialize<MessageEnvelope>(bodyString, _jsonOptions);
        if (envelope?.Payload is null)
        {
            logger.LogWarning("Invalid message in {Queue}", queueName);
            return;
        }

        logger.LogInformation("Processing {EntityType}", entityType);

        var newEntityId = await ProcessEntity(queueName, envelope.Payload);

        var payloadHash = envelope.PayloadHash;
        if (string.IsNullOrWhiteSpace(payloadHash))
        {
            payloadHash = EntityResponseService.ComputeDataHash(envelope.Payload);
        }

        await responseService.SendEntityCreatedResponse(entityType, newEntityId, payloadHash);

        logger.LogInformation("{EntityType} created: {Id}", entityType, newEntityId);
    }

    /// <summary>
    /// Processes an entity based on queue type
    /// </summary>
    /// <param name="queue">Queue name indicating entity type</param>
    /// <param name="payload">Entity payload data</param>
    /// <returns>Generated entity ID</returns>
    /// <exception cref="InvalidOperationException">Thrown for unknown queue types</exception>
    private async Task<Guid> ProcessEntity(string queue, object payload)
    {
        using var scope = serviceProvider.CreateScope();
        return queue switch
        {
            "specialization.create" => await ProcessSpecialization(scope, payload),
            "patient.create" => await ProcessPatient(scope, payload),
            "doctor.create" => await ProcessDoctor(scope, payload),
            "appointment.create" => await ProcessAppointment(scope, payload),
            _ => throw new InvalidOperationException($"Unknown queue: {queue}")
        };
    }

    /// <summary>
    /// Processes a specialization entity
    /// </summary>
    /// <param name="scope">Service scope</param>
    /// <param name="payload">Specialization payload data</param>
    /// <returns>Generated specialization ID</returns>
    private async Task<Guid> ProcessSpecialization(IServiceScope scope, object payload)
    {
        var dto = JsonSerializer.Deserialize<SpecializationCreateDto>(
            JsonSerializer.Serialize(payload), _jsonOptions)!;

        var repo = scope.ServiceProvider.GetRequiredService<IRepository<Specialization>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        var entity = mapper.Map<Specialization>(dto);
        return await repo.CreateAsync(entity);
    }

    /// <summary>
    /// Processes a patient entity
    /// </summary>
    /// <param name="scope">Service scope</param>
    /// <param name="payload">Patient payload data</param>
    /// <returns>Generated patient ID</returns>
    private async Task<Guid> ProcessPatient(IServiceScope scope, object payload)
    {
        var dto = JsonSerializer.Deserialize<PatientCreateDto>(
            JsonSerializer.Serialize(payload), _jsonOptions)!;

        var repo = scope.ServiceProvider.GetRequiredService<IRepository<Patient>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        var entity = mapper.Map<Patient>(dto);
        return await repo.CreateAsync(entity);
    }

    /// <summary>
    /// Processes a doctor entity
    /// </summary>
    /// <param name="scope">Service scope</param>
    /// <param name="payload">Doctor payload data</param>
    /// <returns>Generated doctor ID</returns>
    private async Task<Guid> ProcessDoctor(IServiceScope scope, object payload)
    {
        var dto = JsonSerializer.Deserialize<DoctorCreateDto>(
            JsonSerializer.Serialize(payload), _jsonOptions)!;

        var repo = scope.ServiceProvider.GetRequiredService<IRepository<Doctor>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        var entity = mapper.Map<Doctor>(dto);
        return await repo.CreateAsync(entity);
    }

    /// <summary>
    /// Processes an appointment entity
    /// </summary>
    /// <param name="scope">Service scope</param>
    /// <param name="payload">Appointment payload data</param>
    /// <returns>Generated appointment ID</returns>
    private async Task<Guid> ProcessAppointment(IServiceScope scope, object payload)
    {
        var dto = JsonSerializer.Deserialize<AppointmentCreateDto>(
            JsonSerializer.Serialize(payload), _jsonOptions)!;

        logger.LogInformation("Appointment -> DoctorId:{Doc}, PatientId:{Pat}",
            dto.DoctorId, dto.PatientId);

        var repo = scope.ServiceProvider.GetRequiredService<IRepository<Appointment>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        var entity = mapper.Map<Appointment>(dto);

        if (entity.DateTime.Kind == DateTimeKind.Local)
            entity.DateTime = entity.DateTime.ToUniversalTime();

        return await repo.CreateAsync(entity);
    }

    /// <summary>
    /// Stops the consumer service
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping RabbitMQ Consumer...");

        if (_channel != null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        _connection?.Dispose();

        await base.StopAsync(cancellationToken);
    }
}