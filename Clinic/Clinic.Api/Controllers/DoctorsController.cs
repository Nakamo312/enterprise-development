using Clinic.Application.Dtos.Doctors;
using Clinic.Application.Dtos.Specializations;
using Clinic.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

/// <summary>
/// Controller for managing doctors in the clinic system.
/// </summary>
public class DoctorsController
    (
        ICrudService<DoctorResponseDto, DoctorCreateDto, DoctorUpdateDto> service,
        ICrudService<SpecializationResponseDto, SpecializationCreateDto, SpecializationUpdateDto> specializationService
    )
    : CrudControllerBase<DoctorResponseDto, DoctorCreateDto, DoctorUpdateDto>(service)
{
    /// <summary>
    /// Creates a new doctor record.
    /// </summary>
    /// <param name="createDto">The doctor data to create</param>
    /// <returns>The newly created doctor record</returns>
    /// <response code="201">Returns the newly created doctor</response>
    /// <response code="400">If the request data is invalid or specialization does not exist</response>
    /// <response code="500">If there was an internal server error</response>
    public override async Task<ActionResult<DoctorResponseDto>> Create([FromBody] DoctorCreateDto createDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var specialization = await specializationService.GetAsync(createDto.SpecializationId);
            if (specialization == null)
            {
                return BadRequest(new { error = $"Specialization with ID {createDto.SpecializationId} does not exist" });
            }

            var entity = await service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity); ;
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Updates an existing doctor record.
    /// </summary>
    /// <param name="id">The ID of the doctor to update</param>
    /// <param name="updateDto">The updated doctor data</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the update was successful</response>
    /// <response code="400">If the request data is invalid or specialization does not exist</response>
    /// <response code="404">If the doctor with the specified ID was not found</response>
    /// <response code="500">If there was an internal server error</response>
    public override async Task<ActionResult> Update(Guid id, [FromBody] DoctorUpdateDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var specialization = await specializationService.GetAsync(updateDto.SpecializationId);
            if (specialization == null)
            {
                return BadRequest(new { error = $"Specialization with ID {updateDto.SpecializationId} does not exist" });
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