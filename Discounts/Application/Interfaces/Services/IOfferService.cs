using Application.DTOs.Offer;

namespace Application.Interfaces.Services;

public interface IOfferService
{
    Task DeleteOfferAsync(int id, CancellationToken ct = default);

    Task<OfferDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task UpdateOfferAsync(UpdateOfferDto dto, CancellationToken ct = default);

    Task<IEnumerable<OfferDto>> GetPendingsAsync(CancellationToken ct = default);

    Task UpdateStatusAsync(UpdateOfferStatusDto dto, CancellationToken ct = default);

    Task<OfferDto> CreateOfferAsync(CreateOfferDto dto, CancellationToken ct = default);

    Task<IEnumerable<OfferDto>> GetAllWithCategoryNamesAsync(CancellationToken ct = default);

    Task ChangeRemainingCouponsAsync(int offerId, int count = 1, CancellationToken ct = default);

    Task<bool> IsOfferReservedByUserAsync(int offerId, int userId, CancellationToken ct = default);

    Task<IEnumerable<OfferDto>> GetByMerchantIdAsync(int merchantId, CancellationToken ct = default);
}
