using AutoMapper;
using Microsoft.Extensions.Logging;
using Clinic.Infrastructure.Repositories;
using Clinic.Domain.Models;

namespace Clinic.Application.Services;

public class BaseCrudService<TModel, TDto, TCreateDto, TUpdateDto>
    (
        IRepository<TModel> repository,
        IMapper mapper,
        ILogger<BaseCrudService<TModel, TDto, TCreateDto, TUpdateDto>> logger
    )
    : ICrudService<TDto, TCreateDto, TUpdateDto>
    where TModel : class, IModel
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
{
    protected readonly IRepository<TModel> _repository = repository;
    protected readonly IMapper _mapper = mapper;
    protected readonly ILogger<BaseCrudService<TModel, TDto, TCreateDto, TUpdateDto>> _logger = logger;

    public virtual async Task<IEnumerable<TDto>> GetAsync()
    {
        try
        {
            _logger.LogInformation("Getting all {EntityName}", typeof(TModel).Name);
            var entities = await _repository.GetAsync();
            return _mapper.Map<List<TDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all {EntityName}", typeof(TModel).Name);
            throw new InvalidOperationException($"Failed to retrieve {typeof(TModel).Name} entities", ex);
        }
    }

    public virtual async Task<TDto?> GetAsync(uint id)
    {
        try
        {
            _logger.LogInformation("Getting {EntityName} by ID: {Id}", typeof(TModel).Name, id);
            var entity = await _repository.GetAsync(id);
            return entity == null ? null : _mapper.Map<TDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting {EntityName} with ID: {Id}", typeof(TModel).Name, id);
            throw new InvalidOperationException($"Failed to retrieve {typeof(TModel).Name} with ID {id}", ex);
        }
    }

    public virtual async Task<TDto> CreateAsync(TCreateDto createDto)
    {
        try
        {
            _logger.LogInformation("Creating {EntityName}", typeof(TModel).Name);
            var entity = _mapper.Map<TModel>(createDto);
            await _repository.CreateAsync(entity);
            return _mapper.Map<TDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating {EntityName}", typeof(TModel).Name);
            throw new InvalidOperationException($"Failed to create {typeof(TModel).Name}", ex);
        }
    }

    public virtual async Task<TDto?> UpdateAsync(uint id, TUpdateDto updateDto)
    {
        try
        {
            _logger.LogInformation("Updating {EntityName} with ID: {Id}", typeof(TModel).Name, id);
            var entity = await _repository.GetAsync(id);
            if (entity == null) return null;

            _mapper.Map(updateDto, entity);
            await _repository.UpdateAsync(entity);
            return _mapper.Map<TDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating {EntityName} with ID: {Id}", typeof(TModel).Name, id);
            throw new InvalidOperationException($"Failed to update {typeof(TModel).Name} with ID {id}", ex);
        }
    }

    public virtual async Task<bool> DeleteAsync(uint id)
    {
        try
        {
            _logger.LogInformation("Deleting {EntityName} with ID: {Id}", typeof(TModel).Name, id);
            var exists = await _repository.GetAsync(id);
            if (exists == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting {EntityName} with ID: {Id}", typeof(TModel).Name, id);
            throw new InvalidOperationException($"Failed to delete {typeof(TModel).Name} with ID {id}", ex);
        }
    }
}