using Discounts.Application.Exceptions;
using Application.Interfaces.Services;
using Application.Interfaces.Repos;
using Application.DTOs.Category;
using Domain.Entities;
using Mapster;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default)
    {
        var categories = await _repository.GetAllAsync(ct).ConfigureAwait(false);
        return categories.Adapt<IEnumerable<CategoryDto>>();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var category = await _repository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (category == null) throw new NotFoundException($"Category with Id {id} was not found!");
        return category.Adapt<CategoryDto>();
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default)
    {
        var category = await _repository.GetByNameAsync(dto.Name, ct).ConfigureAwait(false);
        if (category is not null) throw new DomainException($"Category with name '{dto.Name}' already exists!");
        var entity = dto.Adapt<Category>();
        await _repository.AddAsync(entity, ct).ConfigureAwait(false);
        return entity.Adapt<CategoryDto>();
    }

    public async Task UpdateAsync(UpdateCategoryDto dto, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(dto.Id, ct).ConfigureAwait(false);
        if (existing == null) throw new NotFoundException($"Category with Id {dto.Id} was not found!");
        existing.Name = dto.Name;
        existing.Description = dto.Description;
        await _repository.UpdateAsync(existing, ct).ConfigureAwait(false);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (existing == null) throw new NotFoundException($"Category with Id {id} was not found!");
        await _repository.DeleteAsync(existing, ct).ConfigureAwait(false);
    }

    public async Task<IEnumerable<CategoryDto>> GetByIdsAsync(List<int> ids, CancellationToken ct = default)
        => (await _repository.GetByIdsAsync(ids, ct).ConfigureAwait(false)).Adapt<IEnumerable<CategoryDto>>();
}
