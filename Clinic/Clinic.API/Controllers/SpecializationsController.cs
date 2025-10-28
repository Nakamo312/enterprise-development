using Clinic.Application.DTOs;
using Clinic.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.API.Controllers;

/// <summary>
/// Controller for managing Specializations in the clinic system.
/// </summary>
public class SpecializationsController
    (
        ICrudService<SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto> service
    )
    : CrudControllerBase<SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto>(service);
