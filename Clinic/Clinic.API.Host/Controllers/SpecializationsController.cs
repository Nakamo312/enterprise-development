using Clinic.Application.DTOs.Specializations;
using Clinic.Application.Services;

namespace Clinic.API.Host.Controllers;

/// <summary>
/// Controller for managing Specializations in the clinic system.
/// </summary>
public class SpecializationsController
    (
        ICrudService<SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto> service
    )
    : CrudControllerBase<SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto>(service);
