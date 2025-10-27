using Clinic.Application.DTOs;
using Clinic.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.API.Controllers;

/// <summary>
/// Controller for managing patients in the clinic system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PatientController
    (
        ICrudService<PatientResponseDto, PatientCreateDto, PatientUpdateDto> service
    )
    : CrudControllerBase<PatientResponseDto, PatientCreateDto, PatientUpdateDto>(service);
