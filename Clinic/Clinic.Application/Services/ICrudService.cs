namespace Clinic.Application.Services;

/// <summary>
/// Generic service interface for performing CRUD operations on entities.
/// Defines standard create, read, update, and delete operations using DTOs.
/// </summary>
/// <typeparam name="TDto">The type of the response DTO used for data retrieval.</typeparam>
/// <typeparam name="TCreateDto">The type of the DTO used for creating new entities.</typeparam>
/// <typeparam name="TUpdateDto">The type of the DTO used for updating existing entities.</typeparam>
public interface ICrudService<TDto, TCreateDto, TUpdateDto>
{
    /// <summary>
    /// Retrieves all entities as DTOs.
    /// </summary>
    /// <returns>A collection of all entity DTOs.</returns>
    public Task<IEnumerable<TDto>> GetAsync();

    /// <summary>
    /// Retrieves a specific entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>The entity DTO if found; otherwise, null.</returns>
    public Task<TDto?> GetAsync(uint id);

    /// <summary>
    /// Creates a new entity from the provided DTO.
    /// </summary>
    /// <param name="createDto">The DTO containing data for the new entity.</param>
    /// <returns>The created entity as a DTO.</returns>
    public Task<TDto> CreateAsync(TCreateDto createDto);

    /// <summary>
    /// Updates an existing entity with the provided DTO data.
    /// </summary>
    /// <param name="id">The ID of the entity to update.</param>
    /// <param name="updateDto">The DTO containing updated data.</param>
    /// <returns>The updated entity as a DTO if successful; otherwise, null.</returns>
    public Task<TDto?> UpdateAsync(uint id, TUpdateDto updateDto);

    /// <summary>
    /// Deletes an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>True if the entity was successfully deleted; otherwise, false.</returns>
    public Task<bool> DeleteAsync(uint id);
}