using AutoMapper;
using Clinic.Application.Dtos.Appointments;
using Clinic.Domain.Models;
using Clinic.Infrastructure.Repositories;

namespace Clinic.Application.Services;

/// <summary>
/// Service for managing Appointment entities.
/// Provides CRUD operations for Appointments using the underlying repository.
/// </summary>
/// <param name="repository">The repository for Appointment data access.</param>
/// <param name="mapper">The AutoMapper instance for object mapping.</param>
public class AppointmentService(IRepository<Appointment> repository, IMapper mapper)
    : BaseCrudService<Appointment, AppointmentResponseDto, AppointmentCreateDto, AppointmentUpdateDto>(repository, mapper);