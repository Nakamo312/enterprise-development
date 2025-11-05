using Microsoft.AspNetCore.Mvc;
using Clinic.Application.Services;


namespace Clinic.Api.Controllers;

/// <summary>
/// Base controller providing CRUD operations for entities.
/// </summary>
/// <typeparam name="TDto">The response DTO type</typeparam>
/// <typeparam name="TCreateDto">The create DTO type</typeparam>
/// <typeparam name="TUpdateDto">The update DTO type</typeparam>
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public abstract class CrudControllerBase<TDto, TCreateDto, TUpdateDto>
    (ICrudService<TDto, TCreateDto, TUpdateDto> service) : ControllerBase
{
    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>A list of all entities</returns>
    /// <response code="200">Returns the list of entities</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
    {
        try
        {
            var entities = await service.GetAsync();
            return Ok(entities);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Retrieves a specific entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve</param>
    /// <returns>The entity record if found</returns>
    /// <response code="200">Returns the requested entity</response>
    /// <response code="404">If the entity with the specified ID was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult<TDto>> GetById(Guid id)
    {
        try
        {
            var entity = await service.GetAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Creates a new entity record.
    /// </summary>
    /// <param name="createDto">The entity data to create</param>
    /// <returns>The newly created entity record</returns>
    /// <response code="201">Returns the newly created entity</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateDto createDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var entity = await service.CreateAsync(createDto);
            var id = GetIdFromDto(entity);
            return CreatedAtAction(nameof(GetById), new { id }, entity);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }


    /// <summary>
    /// Updates an existing entity record.
    /// </summary>
    /// <param name="id">The ID of the entity to update</param>
    /// <param name="updateDto">The updated entity data</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the update was successful</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="404">If the entity with the specified ID was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult> Update(Guid id, [FromBody] TUpdateDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedEntity = await service.UpdateAsync(id, updateDto);
            if (updatedEntity == null) return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Deletes an entity record.
    /// </summary>
    /// <param name="id">The ID of the entity to delete</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the deletion was successful</response>
    /// <response code="204">If the entity with the specified ID was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await service.DeleteAsync(id);
            if (deleted == true)
            {
                return Ok();
            }

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }
    /// <summary>
    /// Gets the ID from DTO using reflection
    /// </summary>
    private static Guid GetIdFromDto(TDto dto)
    {
        var property = typeof(TDto).GetProperty("Id");
        if (property != null && property.PropertyType == typeof(Guid))
        {
            return (Guid)property.GetValue(dto)!;
        }

        throw new InvalidOperationException($"DTO type {typeof(TDto).Name} does not have an Id property of type uint");
    }
}



