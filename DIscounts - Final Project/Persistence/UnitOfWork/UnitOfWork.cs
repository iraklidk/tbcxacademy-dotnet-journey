using Application.Interfaces;
using Discounts.Persistence.Context;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DiscountsDbContext _dbContext;

    public UnitOfWork(DiscountsDbContext dbContext) => _dbContext = dbContext;

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => await _dbContext.Database.BeginTransactionAsync(ct).ConfigureAwait(false);

    public async Task CommitAsync(CancellationToken ct = default)
        => await _dbContext.Database.CommitTransactionAsync(ct).ConfigureAwait(false);

    public async Task RollbackAsync(CancellationToken ct = default)
        => await _dbContext.Database.RollbackTransactionAsync(ct).ConfigureAwait(false);
}
