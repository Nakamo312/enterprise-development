using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Clinic.Domain.Models;
using Clinic.Application.Services;

namespace Clinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CrudControllerBase<TResponseDto, TCreateDto, TUpdateDto>
    (ICrudService<TResponseDto, TCreateDto, TUpdateDto> service) : ControllerBase
    where TResponseDto : class
    where TCreateDto : class
    where TUpdateDto : class
{
    protected readonly ICrudService<TResponseDto, TCreateDto, TUpdateDto> _service = service;

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TResponseDto>>> GetAll()
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

    [HttpGet("{id:uint}")]
    public virtual async Task<ActionResult<TResponseDto>> GetById(uint id)
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
    public virtual async Task<ActionResult<TResponseDto>> Create([FromBody] TCreateDto createDto)
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

    [HttpPut("{id:uint}")]
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

    [HttpDelete("{id:uint}")]
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