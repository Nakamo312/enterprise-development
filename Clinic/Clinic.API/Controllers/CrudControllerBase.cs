using Microsoft.AspNetCore.Mvc;
using Clinic.Application.Services;
using Clinic.Domain.Models;

namespace Clinic.API.Controllers;

/// <summary>
/// Base controller providing CRUD operations for entities.
/// </summary>
/// <typeparam name="TDto">The response DTO type</typeparam>
/// <typeparam name="TCreateDto">The create DTO type</typeparam>
/// <typeparam name="TUpdateDto">The update DTO type</typeparam>
[ApiController]
[Route("api/[controller]")]
public abstract class CrudControllerBase<TDto, TCreateDto, TUpdateDto>
    (ICrudService<TDto, TCreateDto, TUpdateDto> service) : ControllerBase
    where TDto : Model
    where TCreateDto : class
    where TUpdateDto : class
{
    protected readonly ICrudService<TDto, TCreateDto, TUpdateDto> _service = service;

    protected virtual string EntityName => GetType().Name.Replace("Controller", "");

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
            var entities = await _service.GetAsync();
            return Ok(entities);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
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
    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult<TDto>> GetById(uint id)
    {
        try
        {
            var entity = await _service.GetAsync(id);
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
            var entity = await _service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity); ;
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
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult> Update(uint id, [FromBody] TUpdateDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedEntity = await _service.UpdateAsync(id, updateDto);
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
    /// <response code="404">If the entity with the specified ID was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult> Delete(uint id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    private static int GetEntityId(object entity)
    {
        var idProperty = entity.GetType().GetProperty("Id");
        return (int)(idProperty?.GetValue(entity) ?? 0);
    }
}