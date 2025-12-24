using Clinic.DataGenerator.Services;
using Clinic.RabbitMq.Producer.Configuration;
using Clinic.RabbitMq.Producer.Publishers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMqOptions"));

builder.Services.AddSingleton<IConnectionFactory>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("RabbitMQ");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("RabbitMQ connection string is not configured");
    }

    return new ConnectionFactory
    {
        Uri = new Uri(connectionString),
        AutomaticRecoveryEnabled = true
    };
});

builder.Services.AddSingleton<EntityIdTracker>();
builder.Services.AddSingleton<DataGeneratorService>();
builder.Services.AddSingleton<SpecializationGenService>();
builder.Services.AddSingleton<PatientGenService>();
builder.Services.AddTransient<DoctorGenService>();
builder.Services.AddTransient<AppointmentGenService>();
builder.Services.AddHostedService<RabbitMqProducer>();

var host = builder.Build();
host.Run();