namespace Clinic.Infrastructure.Repositories;

/// Generic repository interface for performing CRUD operations on domain
/// <summary> entities.
/// Provides basic create, read, update, and delete functionality for all entity types.
/// </summary>
/// <typeparam name="T">The type of entity this repository works with, must inherit from Model.</typeparam>
public interface IRepository<T> 
{
    /// <summary>
    /// Creates a new entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>The ID of the newly created entity.</returns>
    public Task<uint> CreateAsync(T entity);

    /// <summary>
    /// Retrieves all entities from the repository.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    public Task<IEnumerable<T>> GetAsync();

    /// <summary>
    /// Retrieves a specific entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public Task<T?> GetAsync(uint id);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity with updated data.</param>
    /// <returns>The updated entity if successful; otherwise, null.</returns>
    public Task<T?> UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>True if the entity was successfully deleted; otherwise, false.</returns>
    public Task<bool> DeleteAsync(uint id);
}