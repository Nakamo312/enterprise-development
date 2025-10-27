using Clinic.Application.Profiles;
using Clinic.Application.Services;
using Clinic.Domain.Models;
using Clinic.Infrastructure.Repositories;
using Clinic.Application.DTOs;
using AutoMapper;
using Clinic.Domain.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Clinic.Application.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
});

var mapperConfig = new MapperConfiguration(
    config => config.AddProfile(new MappingProfile()),
    LoggerFactory.Create(builder => builder.AddConsole()));

IMapper? mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSingleton<IRepository<Patient>, InMemoryRepository<Patient>>();
builder.Services.AddSingleton<IRepository<Doctor>, InMemoryRepository<Doctor>>();
builder.Services.AddSingleton<IRepository<Appointment>, InMemoryRepository<Appointment>>();
builder.Services.AddSingleton<IRepository<Specialization>, InMemoryRepository<Specialization>>();

builder.Services.AddScoped<ICrudService<PatientResponseDto, PatientCreateDto, PatientUpdateDto>>(
    provider => new BaseCrudService<Patient, PatientResponseDto, PatientCreateDto, PatientUpdateDto>(
        provider.GetRequiredService<IRepository<Patient>>(),
        provider.GetRequiredService<IMapper>()
    ));

builder.Services.AddScoped<ICrudService<DoctorResponseDto, DoctorCreateDto, DoctorUpdateDto>>(
    provider => new BaseCrudService<Doctor, DoctorResponseDto, DoctorCreateDto, DoctorUpdateDto>(
        provider.GetRequiredService<IRepository<Doctor>>(),
        provider.GetRequiredService<IMapper>()
    ));

builder.Services.AddScoped<ICrudService<AppointmentResponseDto, AppointmentCreateDto, AppointmentUpdateDto>>(
    provider => new BaseCrudService<Appointment, AppointmentResponseDto, AppointmentCreateDto, AppointmentUpdateDto>(
        provider.GetRequiredService<IRepository<Appointment>>(),
        provider.GetRequiredService<IMapper>()
    ));

builder.Services.AddScoped<ICrudService<SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto>>(
    provider => new BaseCrudService<Specialization, SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto>(
        provider.GetRequiredService<IRepository<Specialization>>(),
        provider.GetRequiredService<IMapper>()
    ));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clinic API",
        Version = "v1",
        Description = "Clinic Management System API"
    });

    var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
    if (File.Exists(apiXmlPath))
    {
        c.IncludeXmlComments(apiXmlPath);
    }
    var dtoXmlFile = "Clinic.Application.xml";
    var dtoXmlPath = Path.Combine(AppContext.BaseDirectory, dtoXmlFile);
    if (File.Exists(dtoXmlPath))
    {
        c.IncludeXmlComments(dtoXmlPath);
    }
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.UseAllOfToExtendReferenceSchemas();
    c.UseAllOfForInheritance();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var patientRepo = scope.ServiceProvider.GetRequiredService<IRepository<Patient>>();
    var doctorRepo = scope.ServiceProvider.GetRequiredService<IRepository<Doctor>>();
    var specializationRepo = scope.ServiceProvider.GetRequiredService<IRepository<Specialization>>();
    var appointmentRepo = scope.ServiceProvider.GetRequiredService<IRepository<Appointment>>();

    foreach (var patient in DataSeed.Patients)
    {
        await patientRepo.CreateAsync(patient);
    }

    foreach (var specialization in DataSeed.Specializations)
    {
        await specializationRepo.CreateAsync(specialization);
    }

    foreach (var doctor in DataSeed.Doctors)
    {
        await doctorRepo.CreateAsync(doctor);
    }

    foreach (var appointment in DataSeed.Appointments)
    {
        await appointmentRepo.CreateAsync(appointment);
    }
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