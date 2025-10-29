using AutoMapper;
using Microsoft.OpenApi.Models;
using Clinic.Infrastructure.Repositories;
using Clinic.Infrastructure.Data.Interfaces;
using Clinic.Infrastructure.Data;
using Clinic.Application.Middleware;
using Clinic.Application.DTOs.Appointments;
using Clinic.Application.DTOs.Doctors;
using Clinic.Application.DTOs.Patients;
using Clinic.Application.DTOs.Specializations;
using Clinic.Application.Profiles;
using Clinic.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
});

var mapperConfig = new MapperConfiguration(
    config =>
    {
        config.AddProfile(new MappingProfile());
        config.AddProfile(new AnalyticQueryProfile());
    },

    LoggerFactory.Create(builder => builder.AddConsole()));

IMapper? mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSingleton(typeof(IRepository<>), typeof(InMemoryRepository<>));

builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<SpecializationService>();

builder.Services.AddScoped<ICrudService<PatientResponseDto, PatientCreateDto, PatientUpdateDto>>(
    provider => provider.GetRequiredService<PatientService>());

builder.Services.AddScoped<ICrudService<DoctorResponseDto, DoctorCreateDto, DoctorUpdateDto>>(
    provider => provider.GetRequiredService<DoctorService>());

builder.Services.AddScoped<ICrudService<AppointmentResponseDto, AppointmentCreateDto, AppointmentUpdateDto>>(
    provider => provider.GetRequiredService<AppointmentService>());

builder.Services.AddScoped<ICrudService<SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto>>(
    provider => provider.GetRequiredService<SpecializationService>());
builder.Services.AddScoped<IAnalyticQueryService, AnalyticQueryService>();

builder.Services.AddScoped<IDataSeeder, InMemoryDataSeeder>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clinic API",
        Version = "v1",
        Description = "Clinic Management System API"
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await seeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();