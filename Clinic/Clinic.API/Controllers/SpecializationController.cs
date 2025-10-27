using Clinic.Application.DTOs;
using Clinic.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.API.Controllers;

/// <summary>
/// Controller for managing Specializations in the clinic system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SpecializationController
    (
        ICrudService<SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto> service
    )
    : CrudControllerBase<SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto>(service);
