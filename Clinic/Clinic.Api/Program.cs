using Microsoft.OpenApi.Models;
using Clinic.Infrastructure.Repositories;
using Clinic.Infrastructure.Data.Interfaces;
using Clinic.Infrastructure.Data;
using Clinic.Application.Dtos.Appointments;
using Clinic.Application.Dtos.Doctors;
using Clinic.Application.Dtos.Patients;
using Clinic.Application.Dtos.Specializations;
using Clinic.Application.Profiles;
using Clinic.Application.Services;
using Clinic.Api.Middleware;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var response = new
            {
                error = "Invalid request payload."
            };
            return new BadRequestObjectResult(response);
        };
    });

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new MappingProfile());
});

builder.Services.AddSingleton(typeof(IRepository<>), typeof(InMemoryRepository<>));

builder.Services.AddScoped<ICrudService<PatientResponseDto, PatientCreateDto, PatientUpdateDto>, PatientService>();
builder.Services.AddScoped<ICrudService<DoctorResponseDto, DoctorCreateDto, DoctorUpdateDto>, DoctorService>();
builder.Services.AddScoped<ICrudService<AppointmentResponseDto, AppointmentCreateDto, AppointmentUpdateDto>, AppointmentService>();
builder.Services.AddScoped<ICrudService<SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto>, SpecializationService>();
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

    var basePath = AppContext.BaseDirectory;

    c.IncludeXmlComments(Path.Combine(basePath, "Clinic.API.xml"));
    c.IncludeXmlComments(Path.Combine(basePath, "Clinic.Application.xml"));
    c.IncludeXmlComments(Path.Combine(basePath, "Clinic.Domain.xml"));
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