using System.Text;
using System.Text.Json;

using AutoMapper;

using Clinic.Application.Dtos.Appointments;
using Clinic.Application.Dtos.Doctors;
using Clinic.Application.Dtos.Patients;
using Clinic.Application.Dtos.RabbitMq;
using Clinic.Application.Dtos.Specializations;
using Clinic.Domain.Models;
using Clinic.Infrastructure.Repositories.Interfaces;
using Clinic.RabbitMq.Consumer.Configuration;
using Clinic.RabbitMq.Consumer.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Clinic.RabbitMq.Consumer.Consumers;

/// <summary>
/// RabbitMQ consumer service for processing entity creation messages.
/// Uses a unified <see cref="EntityResponse"/> for both success and failure responses.
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
    /// Main consumer loop.
    /// </summary>
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

    private async Task StartConsumersAsync()
    {
        if (_channel is null)
            throw new InvalidOperationException("RabbitMQ channel is not initialized");

        foreach (var queueConfig in options.Value.Queues)
        {
            var queueName = queueConfig.Key;
            var entityType = queueConfig.Value;

            var consumer = new AsyncEventingBasicConsumer(_channel!);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                await ProcessMessageAsync(queueName, entityType, ea.Body.ToArray(), ea.DeliveryTag);
            };

            await _channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer);

            logger.LogInformation("Started consumer for {Queue}", queueName);
        }
    }

    /// <summary>
    /// Processes a single message and sends a unified <see cref="EntityResponse"/>.
    /// </summary>
    private async Task ProcessMessageAsync(string queueName, string entityType, byte[] body, ulong deliveryTag)
    {
        var bodyString = Encoding.UTF8.GetString(body);
        var envelope = JsonSerializer.Deserialize<MessageEnvelope>(bodyString, _jsonOptions);

        var payloadHash = string.Empty;
        try
        {
            if (envelope?.Payload is null)
            {
                payloadHash = "";
                logger.LogWarning("Invalid message in {Queue}: payload is null", queueName);
                await responseService.SendEntityResponse(entityType, payloadHash, success: false, reason: "Payload is null");
                await _channel!.BasicAckAsync(deliveryTag, false);
                return;
            }

            payloadHash = string.IsNullOrWhiteSpace(envelope.PayloadHash)
                ? EntityResponseService.ComputeDataHash(envelope.Payload)
                : envelope.PayloadHash;

            Guid newEntityId = await ProcessEntity(queueName, envelope.Payload);

            await responseService.SendEntityResponse(
                entityType,
                payloadHash,
                success: true,
                generatedId: newEntityId
            );

            logger.LogInformation("{EntityType} created: {Id}", entityType, newEntityId);
            await _channel!.BasicAckAsync(deliveryTag, false);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Invalid Appointment"))
        {
            logger.LogError(ex, "Invalid appointment in {Queue}, dropping message", queueName);

            Dictionary<string, object>? additionalData = null;

            if (ex.Data.Contains("AdditionalData"))
            {
                additionalData = ex.Data["AdditionalData"] as Dictionary<string, object>;
            }

            await responseService.SendEntityResponse(
                entityType,
                payloadHash,
                success: false,
                reason: ex.Message,
                additionalData: additionalData);

            await _channel!.BasicNackAsync(deliveryTag, false, false);
        }
    }

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

    private async Task<Guid> ProcessSpecialization(IServiceScope scope, object payload)
    {
        var dto = JsonSerializer.Deserialize<SpecializationCreateDto>(JsonSerializer.Serialize(payload), _jsonOptions)!;
        var repo = scope.ServiceProvider.GetRequiredService<IRepository<Specialization>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        return await repo.CreateAsync(mapper.Map<Specialization>(dto));
    }

    private async Task<Guid> ProcessPatient(IServiceScope scope, object payload)
    {
        var dto = JsonSerializer.Deserialize<PatientCreateDto>(JsonSerializer.Serialize(payload), _jsonOptions)!;
        var repo = scope.ServiceProvider.GetRequiredService<IRepository<Patient>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        return await repo.CreateAsync(mapper.Map<Patient>(dto));
    }

    private async Task<Guid> ProcessDoctor(IServiceScope scope, object payload)
    {
        var dto = JsonSerializer.Deserialize<DoctorCreateDto>(JsonSerializer.Serialize(payload), _jsonOptions)!;
        var repo = scope.ServiceProvider.GetRequiredService<IRepository<Doctor>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        return await repo.CreateAsync(mapper.Map<Doctor>(dto));
    }

    private async Task<Guid> ProcessAppointment(IServiceScope scope, object payload)
    {
        var dto = JsonSerializer.Deserialize<AppointmentCreateDto>(JsonSerializer.Serialize(payload), _jsonOptions)!;
        logger.LogInformation("Appointment -> DoctorId:{Doc}, PatientId:{Pat}", dto.DoctorId, dto.PatientId);
        var repo = scope.ServiceProvider.GetRequiredService<IRepository<Appointment>>();
        var doctorRepo = scope.ServiceProvider.GetRequiredService<IRepository<Doctor>>();
        var patientRepo = scope.ServiceProvider.GetRequiredService<IRepository<Patient>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        var doctor = await doctorRepo.GetAsync(dto.DoctorId);
        var patient = await patientRepo.GetAsync(dto.PatientId);

        if (doctor == null || patient == null)
        {
            var missingEntities = new List<string>();
            var additionalData = new Dictionary<string, object>();

            if (doctor == null)
            {
                missingEntities.Add($"DoctorId={dto.DoctorId}");
                additionalData["InvalidDoctorIds"] = new List<Guid> { dto.DoctorId };
            }

            if (patient == null)
            {
                missingEntities.Add($"PatientId={dto.PatientId}");
                additionalData["InvalidPatientIds"] = new List<Guid> { dto.PatientId };
            }

            var ex = new InvalidOperationException(
                $"Invalid Appointment: referenced entities do not exist. {string.Join(", ", missingEntities)}");
            ex.Data["AdditionalData"] = additionalData;
            throw ex;
        }

        var entity = mapper.Map<Appointment>(dto);
        if (entity.DateTime.Kind == DateTimeKind.Local)
            entity.DateTime = entity.DateTime.ToUniversalTime();

        return await repo.CreateAsync(entity);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping RabbitMQ Consumer...");
        if (_channel != null)
        {
            await _channel.CloseAsync(cancellationToken);
            await _channel.DisposeAsync();
        }
        _connection?.Dispose();
        await base.StopAsync(cancellationToken);
    }
}
