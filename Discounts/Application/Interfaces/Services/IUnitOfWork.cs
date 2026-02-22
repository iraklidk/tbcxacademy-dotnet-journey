namespace Application.Interfaces;

public interface IUnitOfWork
{
    Task RollbackAsync(CancellationToken ct = default);

    Task CommitAsync(CancellationToken ct = default);

    Task BeginTransactionAsync(CancellationToken ct = default);
}
