using Application.Interfaces.Services;
using Application.Interfaces.Repos;
using Domain.Constants;

namespace Application.Services;

public class CleanupService : ICleanupService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IOfferRepository _offerRepository;

    public CleanupService(IReservationRepository reservationRepository,
                          IOfferRepository offerRepository)
    {
        _reservationRepository = reservationRepository;
        _offerRepository = offerRepository;
    }

    public async Task CleanupAsync(CancellationToken ct)
    {
        var expiredOffers = await _offerRepository.GetExpiredOffersAsync(ct).ConfigureAwait(false);
        foreach (var offer in expiredOffers) offer.Status = OfferStatus.Expired;

        var expiredReservations = await _reservationRepository.GetExpiredReservationsAsync(ct).ConfigureAwait(false);

        var dict = new Dictionary<int, int>();

        foreach (var reservation in expiredReservations)
        {
            reservation.IsActive = false;
            if (dict.ContainsKey(reservation.OfferId)) dict[reservation.OfferId]++;
            else dict[reservation.OfferId] = 1;
        }

        var offerToIncreaseCoupon = await _offerRepository.GetByIdsAsync(dict.Keys.ToList(), ct).ConfigureAwait(false);
        foreach(var offer in offerToIncreaseCoupon) offer.RemainingCoupons += dict[offer.Id];

        await _offerRepository.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
