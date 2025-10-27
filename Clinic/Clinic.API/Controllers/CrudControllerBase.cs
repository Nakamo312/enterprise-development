using Microsoft.AspNetCore.Mvc;
using Clinic.Application.Services;
using Clinic.Application.Attributes;

namespace Clinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class CrudControllerBase<TDto, TCreateDto, TUpdateDto>
    (ICrudService<TDto, TCreateDto, TUpdateDto> service) : ControllerBase
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
{
    protected readonly ICrudService<TDto, TCreateDto, TUpdateDto> _service = service;

    protected virtual string EntityName => GetType().Name.Replace("Controller", "");

    [HttpGet]
    [Logging("GetAll{Entity}")]
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

    [HttpGet("{id:int}")]
    [Logging("Get{Entity}ById")]
    public virtual async Task<ActionResult<TDto>> GetById(uint id)
    {
        try
        {
            var entity = await _service.GetAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost]
    [Logging("Create{Entity}")]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateDto createDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var entity = await _service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(entity) }, entity);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Logging("Update{Entity}")]
    public virtual async Task<ActionResult> Update(uint id, [FromBody] TUpdateDto updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updatedEntity = await _service.UpdateAsync(id, updateDto);
            if (updatedEntity == null) return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Logging("Delete{Entity}")]
    public virtual async Task<ActionResult> Delete(uint id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    private static uint GetEntityId(object entity)
    {
        var idProperty = entity.GetType().GetProperty("Id");
        return (uint)(idProperty?.GetValue(entity) ?? 0);
    }
}