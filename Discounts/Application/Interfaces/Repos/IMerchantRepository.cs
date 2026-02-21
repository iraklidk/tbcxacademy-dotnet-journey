using Application.DTOs.Merchant;
using Domain.Entities;

namespace Application.Interfaces.Repos;

public interface IMerchantRepository : IBaseRepository<Merchant>
{
    Task<MerchantResponseDto> GetMerchantByUserIdAsync(int userId, CancellationToken ct = default);
}
