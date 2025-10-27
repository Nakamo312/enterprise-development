using Clinic.Domain.Models;

namespace Clinic.Infrastructure.Repositories;

public class InMemoryRepository<T> : IRepository<T> where T : Model
{
    private readonly List<T> _entities = new();
    private uint _nextId = 1;

    public Task<uint> CreateAsync(T entity)
    {
        entity.Id = _nextId++;
        _entities.Add(entity);
        return Task.FromResult(entity.Id);
    }

    public Task<IEnumerable<T>> GetAsync()
    {
        return Task.FromResult(_entities.AsEnumerable());
    }

    public Task<T?> GetAsync(uint id)
    {
        return Task.FromResult(_entities.FirstOrDefault(e => e.Id == id));
    }

    public Task<T?> UpdateAsync(T entity)
    {
        var existing = _entities.FirstOrDefault(e => e.Id == entity.Id);
        if (existing != null)
        {
            _entities.Remove(existing);
            _entities.Add(entity);
            return Task.FromResult<T?>(entity);
        }
        return Task.FromResult<T?>(null);
    }

    public Task<bool> DeleteAsync(uint id)
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