using Application.DTOs.Merchant;
using Application.DTOs.Offer;

namespace Application.Interfaces.Services;

public interface IMerchantService
{
    Task<MerchantResponseDto> GetMerchantByUserIdAsync(int userId, CancellationToken ct = default);

    Task<IEnumerable<OfferDto>> GetOffersAsync(int merchantId, CancellationToken ct = default);

    Task UpdateMerchantAsync(UpdateMerchantDto merchant, CancellationToken ct = default);

    Task AddMerchantAsync(CreateMerchantDto merchant, CancellationToken ct = default);

    Task<MerchantResponseDto?> GetMerchantByIdAsync(int id, CancellationToken ct);

    Task<IEnumerable<MerchantResponseDto>> GetAllMerchantsAsync(CancellationToken ct = default);

    Task DeleteMerchantAsync(int id, CancellationToken ct = default);
}
