using Application.DTOs.Offer;
using Application.DTOs.Merchant;

namespace Application.Interfaces.Services;

public interface IMerchantService
{
    Task DeleteMerchantAsync(int id, CancellationToken ct = default);

    Task<MerchantResponseDto?> GetMerchantByIdAsync(int id, CancellationToken ct);

    Task AddMerchantAsync(CreateMerchantDto merchant, CancellationToken ct = default);

    Task UpdateMerchantAsync(UpdateMerchantDto merchant, CancellationToken ct = default);

    Task<IEnumerable<OfferDto>> GetOffersAsync(int merchantId, CancellationToken ct = default);

    Task<IEnumerable<MerchantResponseDto>> GetAllMerchantsAsync(CancellationToken ct = default);

    Task<MerchantResponseDto> GetMerchantByUserIdAsync(int userId, CancellationToken ct = default);
}
