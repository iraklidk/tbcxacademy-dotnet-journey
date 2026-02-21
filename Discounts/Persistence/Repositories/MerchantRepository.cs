using Microsoft.EntityFrameworkCore;
using Discounts.Persistence.Context;
using Application.Interfaces.Repos;
using Application.DTOs.Merchant;
using Domain.Entities;

namespace Discounts.Persistence.Repositories;

public class MerchantRepository : BaseRepository<Merchant>, IMerchantRepository
{
    public MerchantRepository(DiscountsDbContext context) : base(context) { }

    public Task<MerchantResponseDto?> GetMerchantByUserIdAsync(int userId, CancellationToken ct = default)
        => _context.Merchants.AsNoTracking().Where(m => m.UserId == userId).Select(m => new MerchantResponseDto
            {
                Id = m.Id,
                Name = m.Name,
                UserId = m.UserId,
                Balance = m.Balance
            })
            .FirstOrDefaultAsync(ct);
}
