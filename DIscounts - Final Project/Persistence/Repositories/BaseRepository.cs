using System.Linq.Expressions;
using Application.Interfaces.Repos;
using Discounts.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Discounts.Persistence.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly DiscountsDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(DiscountsDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct)
        => await _dbSet.FindAsync(new object?[] { id }, ct).ConfigureAwait(false);

    public Task<List<T>> GetAllAsync(CancellationToken ct) => _dbSet.ToListAsync(ct);

    public async Task AddAsync(T entity, CancellationToken ct)
    {
        await _dbSet.AddAsync(entity, ct).ConfigureAwait(false);
        await _context.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    public async Task UpdateAsync(T entity, CancellationToken ct)
    {
        var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey();
        var tracked = _context.ChangeTracker.Entries<T>()
                              .FirstOrDefault(e =>
                                  key.Properties.All(p =>
                                      e.Property(p.Name).CurrentValue.Equals(
                                      _context.Entry(entity).Property(p.Name).CurrentValue)));
        if (tracked != null)
            tracked.State = EntityState.Detached;

        _dbSet.Update(entity);
        await _context.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    public async Task DeleteAsync(T entity, CancellationToken ct)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            _dbSet.Attach(entity);

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct) => _context.Database.BeginTransactionAsync(ct);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);

    public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) => _dbSet.AnyAsync(predicate, ct);
}
