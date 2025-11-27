using Clinic.DataGenerator.Services.Interfaces;
using System.Collections.Concurrent;

namespace Clinic.DataGenerator.Services;

/// <summary>
/// Tracks entity IDs and expected entities for data generation and validation
/// </summary>
public class EntityIdTracker
{
    private readonly ConcurrentDictionary<string, List<Guid>> _createdIds = new();
    private readonly ConcurrentDictionary<string, Dictionary<string, object>> _expectedEntities = new();
    private readonly ConcurrentDictionary<string, ICreationConfirmable> _confirmableServices = new();

    /// <summary>
    /// Registers a service that can confirm entity creation
    /// </summary>
    /// <param name="entityType">Type of the entity</param>
    /// <param name="service">Service implementing ICreationConfirmable</param>
    public void RegisterConfirmableService(string entityType, ICreationConfirmable service)
    {
        _confirmableServices[entityType] = service;
    }

    /// <summary>
    /// Registers an expected entity that will be created
    /// </summary>
    /// <param name="entityType">Type of the entity (e.g., "doctor", "patient")</param>
    /// <param name="dataHash">Unique hash representing the entity data</param>
    /// <param name="entity">The entity object</param>
    public void RegisterExpectedEntity(string entityType, string dataHash, object entity)
    {
        var entities = _expectedEntities.GetOrAdd(entityType, _ => new Dictionary<string, object>());
        lock (entities)
        {
            entities[dataHash] = entity;
        }
    }

    /// <summary>
    /// Registers a created entity ID and removes the corresponding expected entity
    /// </summary>
    /// <param name="entityType">Type of the entity (e.g., "doctor", "patient")</param>
    /// <param name="id">The generated GUID for the entity</param>
    /// <param name="dataHash">Unique hash representing the entity data</param>
    public void RegisterCreatedId(string entityType, Guid id, string dataHash)
    {
        var ids = _createdIds.GetOrAdd(entityType, _ => new List<Guid>());
        lock (ids)
        {
            ids.Add(id);
        }

        if (_confirmableServices.TryGetValue(entityType, out var confirmableService))
        {
            confirmableService.ConfirmCreation(dataHash);
        }

        if (_expectedEntities.TryGetValue(entityType, out var dict))
        {
            lock (dict)
            {
                if (dict.ContainsKey(dataHash))
                    dict.Remove(dataHash);
            }
        }
    }

    /// <summary>
    /// Returns an entity to available state after failed publication
    /// </summary>
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
                if (dict.ContainsKey(dataHash))
                    dict.Remove(dataHash);
            }
        }
    }

    /// <summary>
    /// Gets all created IDs for a specific entity type
    /// </summary>
    /// <param name="entityType">Type of the entity to get IDs for</param>
    /// <returns>List of created GUIDs for the specified entity type</returns>
    public List<Guid> GetIds(string entityType)
    {
        return _createdIds.TryGetValue(entityType, out var idList) ? new List<Guid>(idList) : new List<Guid>();
    }

    /// <summary>
    /// Gets the count of created entities for a specific type
    /// </summary>
    /// <param name="entityType">Type of the entity to count</param>
    /// <returns>Number of created entities for the specified type</returns>
    public int GetCreatedCount(string entityType)
    {
        return _createdIds.TryGetValue(entityType, out var ids) ? ids.Count : 0;
    }

    /// <summary>
    /// Clears all tracking data for a specific entity type
    /// </summary>
    /// <param name="entityType">Type of the entity to clear</param>
    public void Clear(string entityType)
    {
        _createdIds.TryRemove(entityType, out _);
        _expectedEntities.TryRemove(entityType, out _);
    }
}