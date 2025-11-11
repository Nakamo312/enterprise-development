using Clinic.Domain.Models.Abstract;
using Clinic.Infrastructure.Repositories.Interfaces;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation using Entity Framework Core for data access.
/// Provides CRUD operations for domain entities with database persistence.
/// </summary>
/// <typeparam name="T">The type of entity this repository works with, must inherit from Model.</typeparam>
public class Repository<T>(AppDbContext context) : IRepository<T> where T : Model
{
    /// <summary>
    /// Creates a new entity in the database.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>The ID of the newly created entity.</returns>
    public async Task<Guid> CreateAsync(T entity)
    {
        context.Set<T>().Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    /// <summary>
    /// Retrieves all entities from the database.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    public async Task<IEnumerable<T>> GetAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public async Task<T?> GetAsync(Guid id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <param name="entity">The entity with updated data.</param>
    /// <returns>The updated entity if successful; otherwise, null.</returns>
    public async Task<T?> UpdateAsync(T entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes an entity from the database by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>True if the entity was successfully deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await GetAsync(id);
        if (entity == null) return false;
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}