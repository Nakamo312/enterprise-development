using AutoMapper;
using Clinic.Application.Dtos.Doctors;
using Clinic.Domain.Models;
using Clinic.Infrastructure.Repositories;

namespace Clinic.Application.Services;

/// <summary>
/// Service for managing Doctor entities.
/// Provides CRUD operations for Doctors using the underlying repository.
/// </summary>
/// <param name="repository">The repository for Doctor data access.</param>
/// <param name="mapper">The AutoMapper instance for object mapping.</param>
public class DoctorService(IRepository<Doctor> repository, IMapper mapper)
    : BaseCrudService<Doctor, DoctorResponseDto, DoctorCreateDto, DoctorUpdateDto>(repository, mapper);