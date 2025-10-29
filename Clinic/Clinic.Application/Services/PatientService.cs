using AutoMapper;
using Clinic.Application.DTOs.Patients;
using Clinic.Domain.Models;
using Clinic.Infrastructure.Repositories;

namespace Clinic.Application.Services;

/// <summary>
/// Service for managing patient entities.
/// Provides CRUD operations for patients using the underlying repository.
/// </summary>
/// <param name="repository">The repository for patient data access.</param>
/// <param name="mapper">The AutoMapper instance for object mapping.</param>
public class PatientService(IRepository<Patient> repository, IMapper mapper)
    : BaseCrudService<Patient, PatientResponseDto, PatientCreateDto, PatientUpdateDto>(repository, mapper)
{

}