using Clinic.Application.Dtos.Patients;
using Clinic.Application.Services;

namespace Clinic.Api.Controllers;

/// <summary>
/// Controller for managing patients in the clinic system.
/// </summary>
public class PatientsController
    (
        ICrudService<PatientResponseDto, PatientCreateDto, PatientUpdateDto> service
    )
    : CrudControllerBase<PatientResponseDto, PatientCreateDto, PatientUpdateDto>(service);