namespace Clinic.Application.Services;

public interface ICrudService<TResponseDto, TCreateDto, TUpdateDto>
{
    Task<IEnumerable<TResponseDto>> GetAsync();
    Task<TResponseDto?> GetAsync(uint id);
    Task<TResponseDto> CreateAsync(TCreateDto createDto);
    Task<TResponseDto?> UpdateAsync(uint id, TUpdateDto updateDto);
    Task<bool> DeleteAsync(uint id);
}
