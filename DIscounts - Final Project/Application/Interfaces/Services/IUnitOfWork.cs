namespace Application.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken ct = default);

    Task RollbackAsync(CancellationToken ct = default);

    Task BeginTransactionAsync(CancellationToken ct = default);
}
