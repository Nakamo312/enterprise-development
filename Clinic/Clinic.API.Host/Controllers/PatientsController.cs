using Clinic.Application.DTOs.Patients;
using Clinic.Application.Services;

namespace Clinic.API.Host.Controllers;

/// <summary>
/// Controller for managing patients in the clinic system.
/// </summary>
public class PatientsController
    (
        ICrudService<PatientResponseDto, PatientCreateDto, PatientUpdateDto> service
    )
    : CrudControllerBase<PatientResponseDto, PatientCreateDto, PatientUpdateDto>(service);