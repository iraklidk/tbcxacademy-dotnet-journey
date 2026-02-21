using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Application.Interfaces.Repos;

public interface IBaseRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken ct = default); // c

    Task<T?> GetByIdAsync(int id, CancellationToken ct = default); // r

    Task UpdateAsync(T entity, CancellationToken ct = default); // u

    Task DeleteAsync(T entity, CancellationToken ct = default); // d

    Task<List<T>> GetAllAsync(CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken ct = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);

    public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
}
