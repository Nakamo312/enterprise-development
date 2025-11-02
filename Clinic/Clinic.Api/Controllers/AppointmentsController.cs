using Clinic.Application.Dtos.Appointments;
using Clinic.Application.Dtos.Doctors;
using Clinic.Application.Dtos.Patients;
using Clinic.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

/// <summary>
/// Controller for managing appointments in the clinic system.
/// </summary>
public class AppointmentsController
    (
        ICrudService<AppointmentResponseDto, AppointmentCreateDto, AppointmentUpdateDto> service,
        ICrudService<PatientResponseDto, PatientCreateDto, PatientUpdateDto> patientService,
        ICrudService<DoctorResponseDto, DoctorCreateDto, DoctorUpdateDto> doctorService
    )
         : CrudControllerBase<AppointmentResponseDto, AppointmentCreateDto, AppointmentUpdateDto>(service)
{

    /// <summary>
    /// Creates a new appointment record.
    /// </summary>
    /// <param name="createDto">The appointment data to create</param>
    /// <returns>The newly created appointment record</returns>
    /// <response code="201">Returns the newly created appointment</response>
    /// <response code="400">If the request data is invalid or patient/doctor does not exist</response>
    /// <response code="500">If there was an internal server error</response>
    public override async Task<ActionResult<AppointmentResponseDto>> Create([FromBody] AppointmentCreateDto createDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var patient = await patientService.GetAsync(createDto.PatientId);
            if (patient == null)
            {
                return BadRequest(new { error = $"Patient with ID {createDto.PatientId} does not exist" });
            }

            var doctor = await doctorService.GetAsync(createDto.DoctorId);
            if (doctor == null)
            {
                return BadRequest(new { error = $"Doctor with ID {createDto.DoctorId} does not exist" });
            }

            var entity = await service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Updates an existing appointment record.
    /// </summary>
    /// <param name="id">The ID of the appointment to update</param>
    /// <param name="updateDto">The updated appointment data</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the update was successful</response>
    /// <response code="400">If the request data is invalid or patient/doctor does not exist</response>
    /// <response code="404">If the appointment with the specified ID was not found</response>
    /// <response code="500">If there was an internal server error</response>
    public override async Task<ActionResult> Update(uint id, [FromBody] AppointmentUpdateDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var patient = await patientService.GetAsync(updateDto.PatientId);
            if (patient == null)
            {
                return BadRequest(new { error = $"Patient with ID {updateDto.PatientId} does not exist" });
            }
            var doctor = await doctorService.GetAsync(updateDto.DoctorId);

            if (doctor == null)
            {
                return BadRequest(new { error = $"Doctor with ID {updateDto.DoctorId} does not exist" });
            }

            var updatedEntity = await service.UpdateAsync(id, updateDto);
            if (updatedEntity == null) return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }
}