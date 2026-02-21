using Discounts.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repos;
using Domain.Entities;

namespace Discounts.Persistence.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(DiscountsDbContext context) : base(context) { }

    public Task<Category?> GetByNameAsync(string title, CancellationToken ct = default)
        => _context.Categories.FirstOrDefaultAsync(c => c.Name == title, ct);

    public Task<List<Category>> GetByIdsAsync(List<int> ids, CancellationToken ct = default)
        => _context.Categories.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
}
