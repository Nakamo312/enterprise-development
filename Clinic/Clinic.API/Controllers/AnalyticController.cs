using Clinic.Application.DTOs;
using Clinic.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace analytic.API.Controllers;

/// <summary>
/// Controller for analytic queries and reports.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticController(IAnalyticQueryService analyticQueryService) : ControllerBase
{
    private readonly IAnalyticQueryService _analyticQueryService = analyticQueryService;

    /// <summary>
    /// Gets doctors with minimum experience.
    /// </summary>
    /// <param name="queryDto">Query parameters</param>
    /// <returns>List of doctor full names</returns>
    /// <response code="200">Returns list of doctors</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("doctors/experience")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<NamesResponseDto>> GetDoctorsWithExperience([FromQuery] DoctorsExperienceQueryDto queryDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _analyticQueryService.GetDoctorsWithExperienceAsync(queryDto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Gets patients by specific doctor.
    /// </summary>
    /// <param name="queryDto">Query parameters</param>
    /// <returns>List of patient full names</returns>
    /// <response code="200">Returns list of patients</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("doctors/patients")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<NamesResponseDto>> GetPatientsByDoctor([FromQuery] DoctorPatientsQueryDto queryDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _analyticQueryService.GetPatientsByDoctorAsync(queryDto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Gets repeated appointments within date range.
    /// </summary>
    /// <param name="queryDto">Query parameters</param>
    /// <returns>List of appointment IDs</returns>
    /// <response code="200">Returns list of appointment IDs</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("appointments/repeated")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IdsResponseDto>> GetRepeatedAppointments([FromQuery] RepeatedAppointmentsQueryDto queryDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _analyticQueryService.GetRepeatedAppointmentsAsync(queryDto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Gets patients older than 30 who visited multiple doctors.
    /// </summary>
    /// <returns>List of patient full names</returns>
    /// <response code="200">Returns list of patients</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("patients/over30-multiple-doctors")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<NamesResponseDto>> GetPatientsOver30WithMultipleDoctors()
    {
        try
        {
            var result = await _analyticQueryService.GetPatientsOver30WithMultipleDoctorsAsync();
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Gets appointments for current month in specified room.
    /// </summary>
    /// <param name="queryDto">Query parameters</param>
    /// <returns>List of appointment IDs</returns>
    /// <response code="200">Returns list of appointment IDs</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("appointments/room")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IdsResponseDto>> GetAppointmentsForRoom([FromQuery] RoomAppointmentsQueryDto queryDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _analyticQueryService.GetAppointmentsForRoomThisMonthAsync(queryDto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}