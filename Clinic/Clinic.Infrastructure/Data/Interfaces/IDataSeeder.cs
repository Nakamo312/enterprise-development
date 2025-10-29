namespace Clinic.Infrastructure.Data.Interfaces;

/// <summary>
/// Defines the contract for data seeding operations.
/// Provides methods for initializing and clearing data in the underlying data store.
/// </summary>
public interface IDataSeeder
{
    /// <summary>
    /// Seeds the data store with initial test or development data.
    /// Populates the database with predefined entities for application initialization.
    /// </summary>
    /// <returns>A task representing the asynchronous seeding operation.</returns>
    public Task SeedAsync();

    /// <summary>
    /// Clears all data from the data store.
    /// Removes all entities to prepare for fresh data initialization or testing scenarios.
    /// </summary>
    /// <returns>A task representing the asynchronous clearing operation.</returns>
    public Task ClearAsync();
}