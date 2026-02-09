using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagement.Application.RepoInterfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken ct); // c
        Task<T?> GetByIdAsync(int id, CancellationToken ct); // r
        Task UpdateAsync(T entity, CancellationToken ct); // u
        Task DeleteAsync(T entity, CancellationToken ct); // d
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
    }
}
