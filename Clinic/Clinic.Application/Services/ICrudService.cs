namespace Clinic.Application.Services;

public interface ICrudService<TDto, TCreateDto, TUpdateDto>
{
    Task<IEnumerable<TDto>> GetAsync();
    Task<TDto?> GetAsync(uint id);
    Task<TDto> CreateAsync(TCreateDto createDto);
    Task<TDto?> UpdateAsync(uint id, TUpdateDto updateDto);
    Task<bool> DeleteAsync(uint id);
}
