using Application.DTOs.Category;

namespace Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetByIdsAsync(List<int> ids, CancellationToken ct = default);

    Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default);

    Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default);

    Task UpdateAsync(UpdateCategoryDto dto, CancellationToken ct = default);

    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
