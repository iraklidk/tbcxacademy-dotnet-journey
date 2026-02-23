using Application.Interfaces;
using Discounts.Persistence.Context;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DiscountsDbContext _dbContext;

    public UnitOfWork(DiscountsDbContext dbContext) => _dbContext = dbContext;

    public Task BeginTransactionAsync(CancellationToken ct = default)
        => _dbContext.Database.BeginTransactionAsync(ct);

    public Task CommitAsync(CancellationToken ct = default)
        => _dbContext.Database.CommitTransactionAsync(ct);

    public Task RollbackAsync(CancellationToken ct = default)
        => _dbContext.Database.RollbackTransactionAsync(ct);
}
