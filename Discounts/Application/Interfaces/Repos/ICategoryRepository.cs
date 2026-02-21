using Domain.Entities;

namespace Application.Interfaces.Repos;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<List<Category>> GetByIdsAsync(List<int> ids, CancellationToken ct = default);

    Task<Category?> GetByNameAsync(string title, CancellationToken ct = default);
}
