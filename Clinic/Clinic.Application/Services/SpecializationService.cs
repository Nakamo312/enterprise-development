using AutoMapper;
using Clinic.Application.Dtos.Specializations;
using Clinic.Domain.Models;
using Clinic.Infrastructure.Repositories;

namespace Clinic.Application.Services;

/// <summary>
/// Service for managing Specialization entities.
/// Provides CRUD operations for Specializations using the underlying repository.
/// </summary>
/// <param name="repository">The repository for Specialization data access.</param>
/// <param name="mapper">The AutoMapper instance for object mapping.</param>
public class SpecializationService(IRepository<Specialization> repository, IMapper mapper)
    : BaseCrudService<Specialization, SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto>(repository, mapper);