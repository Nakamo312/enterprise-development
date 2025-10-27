using Clinic.Application.Attributes;
using Clinic.Application.DTOs;
using Clinic.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : CrudControllerBase<PatientResponseDto, PatientCreateDto, PatientUpdateDto>
{
    public PatientsController(ICrudService<PatientResponseDto, PatientCreateDto, PatientUpdateDto> service)
        : base(service)
    {
    }
}