using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly LibraryDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(LibraryDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct) => await _dbSet.FindAsync(new object[] { id }, ct);

    public Task<List<T>> GetAllAsync(CancellationToken ct) => _dbSet.ToListAsync(ct);
    public async Task AddAsync(T entity, CancellationToken ct)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(T entity, CancellationToken ct)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(T entity, CancellationToken ct)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct)
    {
       return _context.Database.BeginTransactionAsync(ct);
    }
}
