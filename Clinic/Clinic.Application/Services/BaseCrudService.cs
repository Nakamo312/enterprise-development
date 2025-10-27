using AutoMapper;
using Clinic.Infrastructure.Repositories;
using Clinic.Domain.Models;

namespace Clinic.Application.Services;

/// <summary>
/// Base implementation of CRUD service providing common operations for all entities.
/// Handles mapping between domain models and DTOs, and delegates data access to the repository.
/// </summary>
/// <typeparam name="TModel">The type of the domain model, must inherit from Model.</typeparam>
/// <typeparam name="TDto">The type of the response DTO used for data retrieval.</typeparam>
/// <typeparam name="TCreateDto">The type of the DTO used for creating new entities.</typeparam>
/// <typeparam name="TUpdateDto">The type of the DTO used for updating existing entities.</typeparam>
public class BaseCrudService<TModel, TDto, TCreateDto, TUpdateDto>
    (
        IRepository<TModel> repository,
        IMapper mapper
    )
    : ICrudService<TDto, TCreateDto, TUpdateDto>
    where TModel : Model
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
{
    protected readonly IRepository<TModel> _repository = repository;
    protected readonly IMapper _mapper = mapper;

    /// <summary>
    /// Retrieves all entities as DTOs.
    /// </summary>
    /// <returns>A collection of all entity DTOs.</returns>
    /// <exception cref="InvalidOperationException">Thrown when retrieval fails.</exception>
    public virtual async Task<IEnumerable<TDto>> GetAsync()
    {
        try
        {
            var entities = await _repository.GetAsync();
            return _mapper.Map<List<TDto>>(entities);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve {typeof(TModel).Name} entities", ex);
        }
    }

    /// <summary>
    /// Retrieves a specific entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>The entity DTO if found; otherwise, null.</returns>
    /// <exception cref="InvalidOperationException">Thrown when retrieval fails.</exception>
    public virtual async Task<TDto?> GetAsync(uint id)
    {
        try
        {
            var entity = await _repository.GetAsync(id);
            return entity == null ? null : _mapper.Map<TDto>(entity);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve {typeof(TModel).Name} with ID {id}", ex);
        }
    }

    /// <summary>
    /// Creates a new entity from the provided DTO.
    /// </summary>
    /// <param name="createDto">The DTO containing data for the new entity.</param>
    /// <returns>The created entity as a DTO.</returns>
    /// <exception cref="InvalidOperationException">Thrown when creation fails.</exception>
    public virtual async Task<TDto> CreateAsync(TCreateDto createDto)
    {
        try
        {
            var entity = _mapper.Map<TModel>(createDto);
            await _repository.CreateAsync(entity);
            return _mapper.Map<TDto>(entity);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create {typeof(TModel).Name}", ex);
        }
    }

    /// <summary>
    /// Updates an existing entity with the provided DTO data.
    /// </summary>
    /// <param name="id">The ID of the entity to update.</param>
    /// <param name="updateDto">The DTO containing updated data.</param>
    /// <returns>The updated entity as a DTO if successful; otherwise, null.</returns>
    /// <exception cref="InvalidOperationException">Thrown when update fails.</exception>
    public virtual async Task<TDto?> UpdateAsync(uint id, TUpdateDto updateDto)
    {
        try
        {
            var entity = await _repository.GetAsync(id);
            if (entity == null) return null;

            _mapper.Map(updateDto, entity);
            await _repository.UpdateAsync(entity);
            return _mapper.Map<TDto>(entity);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to update {typeof(TModel).Name} with ID {id}", ex);
        }
    }

    /// <summary>
    /// Deletes an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>True if the entity was successfully deleted; otherwise, false.</returns>
    /// <exception cref="InvalidOperationException">Thrown when deletion fails.</exception>
    public virtual async Task<bool> DeleteAsync(uint id)
    {
        try
        {
            var exists = await _repository.GetAsync(id);
            if (exists == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to delete {typeof(TModel).Name} with ID {id}", ex);
        }
    }
}