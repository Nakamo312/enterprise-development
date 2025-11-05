using Clinic.Domain.Models.Abstract;
using Clinic.Infrastructure.Repositories.Interfaces;

namespace Clinic.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of the repository pattern for testing and development purposes.
/// Stores entities in a local list without persistent storage.
/// </summary>
/// <typeparam name="T">The type of entity this repository works with, must inherit from Model.</typeparam>
public class InMemoryRepository<T> : IRepository<T> where T : Model
{
    private readonly List<T> _entities = [];

    /// <summary>
    /// Creates a new entity in the in-memory repository.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>The ID of the newly created entity.</returns>
    public Task<Guid> CreateAsync(T entity)
    {
        entity.Id = Guid.NewGuid();
        _entities.Add(entity);
        return Task.FromResult(entity.Id);
    }

    /// <summary>
    /// Retrieves all entities from the in-memory repository.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    public Task<IEnumerable<T>> GetAsync()
    {
        return Task.FromResult(_entities.AsEnumerable());
    }

    /// <summary>
    /// Retrieves a specific entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public Task<T?> GetAsync(Guid id)
    {
        return Task.FromResult(_entities.FirstOrDefault(e => e.Id == id));
    }

    /// <summary>
    /// Updates an existing entity in the in-memory repository.
    /// </summary>
    /// <param name="entity">The entity with updated data.</param>
    /// <returns>The updated entity if successful; otherwise, null.</returns>
    public Task<T?> UpdateAsync(T entity)
    {
        var existing = _entities.FirstOrDefault(e => e.Id == entity.Id);

        if (existing != null)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == "Id" || !property.CanWrite)
                    continue;

                var newValue = property.GetValue(entity);
                property.SetValue(existing, newValue);
            }
            return Task.FromResult<T?>(existing);
        }
        return Task.FromResult<T?>(null);
    }

    /// <summary>
    /// Deletes an entity from the in-memory repository by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>True if the entity was successfully deleted; otherwise, false.</returns>
    public Task<bool> DeleteAsync(Guid id)
    {
        var entity = _entities.FirstOrDefault(e => e.Id == id);
        if (entity != null)
        {
            _entities.Remove(entity);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}