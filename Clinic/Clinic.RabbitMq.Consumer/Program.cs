using Clinic.Infrastructure.Repositories;
using Clinic.RabbitMq.Consumer.Configuration;
using Clinic.RabbitMq.Consumer.Consumers;
using Clinic.Infrastructure.Repositories.Interfaces;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Clinic.RabbitMq.Consumer.Services;
using Clinic.Application.Profiles;
using Clinic.ServiceDefaults; 

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.Configure<RabbitMqConsumerOptions>(
    builder.Configuration.GetSection("RabbitMqConsumerOptions"));

builder.AddNpgsqlDbContext<AppDbContext>(connectionName: "Database");

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new MappingProfile());
});

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
        Uri = new Uri(connectionString)
    };
});

builder.Services.AddSingleton<EntityResponseService>();
builder.Services.AddHostedService<RabbitMqConsumer>();

var host = builder.Build();

host.Run();