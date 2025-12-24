using System.Collections.Concurrent;

using Clinic.DataGenerator.Services.Interfaces;

namespace Clinic.DataGenerator.Services;

/// <summary>
/// Tracks entity IDs and expected entities for data generation and validation.
/// Supports confirming creation, rolling back failed entities, and retrieving created IDs.
/// </summary>
public class EntityIdTracker
{
    /// <summary>
    /// Stores created entity IDs per entity type and payload hash.
    /// Key: entityType, Value: Dictionary{payloadHash -> createdId}.
    /// </summary>
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Guid>> _created
        = [];

    /// <summary>
    /// Stores expected entities per entity type and payload hash.
    /// Key: entityType, Value: Dictionary{payloadHash -> entity object}.
    /// </summary>
    private readonly ConcurrentDictionary<string, Dictionary<string, object>> _expectedEntities
        = [];

    /// <summary>
    /// Stores services that can confirm entity creation for each entity type.
    /// </summary>
    private readonly ConcurrentDictionary<string, ICreationConfirmable> _confirmableServices
        = [];

    /// <summary>
    /// Registers a service that can confirm entity creation for a specific entity type.
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="service">Service implementing <see cref="ICreationConfirmable"/>.</param>
    public void RegisterConfirmableService(string entityType, ICreationConfirmable service)
    {
        _confirmableServices[entityType] = service;
    }

    /// <summary>
    /// Registers an expected entity that is planned to be created.
    /// </summary>
    /// <param name="entityType">Type of the entity (e.g., "doctor", "patient").</param>
    /// <param name="dataHash">Unique hash representing the entity data.</param>
    /// <param name="entity">The entity object.</param>
    public void RegisterExpectedEntity(string entityType, string dataHash, object entity)
    {
        var entities = _expectedEntities.GetOrAdd(entityType, _ => new Dictionary<string, object>());
        lock (entities)
        {
            entities[dataHash] = entity;
        }
    }

    /// <summary>
    /// Marks an entity as created, storing its generated ID and removing it from expected entities.
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="id">The generated GUID for the entity.</param>
    /// <param name="dataHash">Unique hash representing the entity data.</param>
    public void RegisterCreatedId(string entityType, Guid id, string dataHash)
    {
        var map = _created.GetOrAdd(entityType, _ => new ConcurrentDictionary<string, Guid>());
        map[dataHash] = id;

        if (_confirmableServices.TryGetValue(entityType, out var confirmableService))
        {
            confirmableService.ConfirmCreation(dataHash);
        }

        if (_expectedEntities.TryGetValue(entityType, out var dict))
        {
            lock (dict)
            {
                dict.Remove(dataHash);
            }
        }
    }

    /// <summary>
    /// Returns an entity to the available state after failed publication.
    /// Removes it from expected entities and notifies the confirmable service.
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="dataHash">Unique hash representing the entity data.</param>
    public void ReturnToAvailable(string entityType, string dataHash)
    {
        if (_confirmableServices.TryGetValue(entityType, out var confirmableService))
        {
            confirmableService.ReturnToAvailable(dataHash);
        }

        if (_expectedEntities.TryGetValue(entityType, out var dict))
        {
            lock (dict)
            {
                dict.Remove(dataHash);
            }
        }
    }

    /// <summary>
    /// Checks if a specific entity (by payload hash) has already been created.
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="dataHash">Payload hash of the entity.</param>
    /// <returns>True if the entity has been created; otherwise, false.</returns>
    public bool IsCreated(string entityType, string dataHash)
    {
        return _created.TryGetValue(entityType, out var map)
               && map.ContainsKey(dataHash);
    }

    /// <summary>
    /// Retrieves all created IDs for a specific entity type.
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <returns>List of created GUIDs.</returns>
    public List<Guid> GetIds(string entityType)
    {
        return _created.TryGetValue(entityType, out var map)
            ? map.Values.Distinct().ToList()
            : new List<Guid>();
    }

    /// <summary>
    /// Clears all tracking data (created IDs and expected entities) for a specific entity type.
    /// </summary>
    /// <param name="entityType">Type of the entity to clear.</param>
    public void Clear(string entityType)
    {
        _created.TryRemove(entityType, out _);
        _expectedEntities.TryRemove(entityType, out _);
    }

    /// <summary>
    /// Removes a created entity ID by its data hash.
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="dataHash">Unique hash representing the entity data.</param>
    /// <returns>True if the ID was removed; otherwise, false.</returns>
    public bool RemoveCreatedId(string entityType, string dataHash)
    {
        if (_created.TryGetValue(entityType, out var map))
        {
            return map.TryRemove(dataHash, out _);
        }
        return false;
    }

    /// <summary>
    /// Removes a created entity ID by its GUID.
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="id">The GUID to remove.</param>
    /// <returns>True if the ID was removed; otherwise, false.</returns>
    public bool RemoveCreatedIdByGuid(string entityType, Guid id)
    {
        if (_created.TryGetValue(entityType, out var map))
        {
            var hashToRemove = map.FirstOrDefault(x => x.Value == id).Key;
            if (hashToRemove != null)
            {
                return map.TryRemove(hashToRemove, out _);
            }
        }
        return false;
    }

    /// <summary>
    /// Gets the number of expected entities remaining for a specific entity type.
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <returns>Number of expected entities.</returns>
    public int GetExpectedCount(string entityType)
    {
        return _expectedEntities.TryGetValue(entityType, out var dict)
            ? dict.Count
            : 0;
    }
}