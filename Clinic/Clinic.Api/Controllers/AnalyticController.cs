using Clinic.Application.Dtos.Appointments;
using Clinic.Application.Dtos.Doctors;
using Clinic.Application.Dtos.Patients;
using Clinic.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

/// <summary>
/// Controller for analytic queries and reports.
/// Provides endpoints for generating various business intelligence reports and data analytics.
/// </summary>
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class AnalyticController(IAnalyticQueryService analyticQueryService) : ControllerBase
{
    /// <summary>
    /// Retrieves doctors with work experience greater than or equal to the specified number of years.
    /// </summary>
    /// <param name="experience">Minimum years of experience required</param>
    /// <returns>Collection of doctors matching the experience criteria</returns>
    /// <response code="200">Returns list of doctors</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("doctors/experience")]
    [ProducesResponseType(typeof(IEnumerable<DoctorResponseDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<DoctorResponseDto>>> GetDoctorsWithExperience([FromQuery] uint experience)
    {
        try
        {
            var result = await analyticQueryService.GetDoctorsWithExperienceAsync(experience);
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Retrieves all patients who have appointments with a specific doctor.
    /// </summary>
    /// <param name="doctorId">Unique identifier of the doctor</param>
    /// <returns>Collection of patients treated by the specified doctor</returns>
    /// <response code="200">Returns list of patients</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("doctors/{doctorId}/patients")]
    [ProducesResponseType(typeof(IEnumerable<PatientResponseDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<PatientResponseDto>>> GetPatientsByDoctor(uint doctorId)
    {
        try
        {
            var result = await analyticQueryService.GetPatientsByDoctorAsync(doctorId);
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Retrieves repeated appointments within the specified date range.
    /// </summary>
    /// <param name="startDate">Start date of the search range</param>
    /// <param name="endDate">End date of the search range</param>
    /// <returns>Collection of appointment IDs for repeated appointments in the date range</returns>
    /// <response code="200">Returns list of appointment IDs</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("appointments/repeated")]
    [ProducesResponseType(typeof(IEnumerable<AppointmentResponseDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<AppointmentResponseDto>>> GetRepeatedAppointments([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                var today = DateTime.Today;
                startDate = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
            }
            var result = await analyticQueryService.GetRepeatedAppointmentsAsync(startDate.Value, endDate.Value);
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Retrieves patients over 30 years old who have been treated by multiple different doctors.
    /// </summary>
    /// <returns>Collection of patients meeting the age and multiple doctors criteria</returns>
    /// <response code="200">Returns list of patients</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("patients/over30-multiple-doctors")]
    [ProducesResponseType(typeof(IEnumerable<PatientResponseDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<PatientResponseDto>>> GetPatientsOver30WithMultipleDoctors()
    {
        try
        {
            var result = await analyticQueryService.GetPatientsOver30WithMultipleDoctorsAsync();
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Retrieves appointments for a specific room during the current month.
    /// </summary>
    /// <param name="roomNumber">Query parameters containing room number</param>
    /// <returns>Collection of appointment IDs for the specified room in current month</returns>
    /// <response code="200">Returns list of appointment IDs</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("appointments/room")]
    [ProducesResponseType(typeof(IEnumerable<AppointmentResponseDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<AppointmentResponseDto>>> GetAppointmentsForRoom([FromQuery] string roomNumber)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await analyticQueryService.GetAppointmentsForRoomThisMonthAsync(roomNumber);
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }
}