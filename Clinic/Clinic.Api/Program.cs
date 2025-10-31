using Clinic.Application.Dtos.Appointments;
using Clinic.Application.Dtos.Doctors;
using Clinic.Application.Dtos.Patients;
using Clinic.Application.Dtos.Specializations;
using Clinic.Application.Profiles;
using Clinic.Application.Services;
using Clinic.Infrastructure.Repositories;
using Clinic.Infrastructure.Persistence;
using Clinic.Infrastructure.Data.Interfaces;
using Clinic.Infrastructure.Data;
using Clinic.ServiceDefaults;
using Clinic.Api.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<AppDbContext>("Database", configureDbContextOptions: builder => builder.UseLazyLoadingProxies());
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new MappingProfile());
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<ICrudService<PatientResponseDto, PatientCreateDto, PatientUpdateDto>, PatientService>();
builder.Services.AddScoped<ICrudService<DoctorResponseDto, DoctorCreateDto, DoctorUpdateDto>, DoctorService>();
builder.Services.AddScoped<ICrudService<AppointmentResponseDto, AppointmentCreateDto, AppointmentUpdateDto>, AppointmentService>();
builder.Services.AddScoped<ICrudService<SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto>, SpecializationService>();
builder.Services.AddScoped<IAnalyticQueryService, AnalyticQueryService>();

builder.Services.AddScoped<IDataSeeder, EfDataSeeder>();

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

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();