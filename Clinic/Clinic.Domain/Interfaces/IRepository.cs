namespace Clinic.Domain.Interfaces;
public interface IRepository<T> where T : IModel
{
    public Task<uint> CreateAsync(T entity);
    public Task<IEnumerable<T>> GetAsync();
    public Task<T?> GetAsync(uint id);
    public Task<T?> UpdateAsync(T entity);
    public Task<bool> DeleteAsync(uint id);
}
