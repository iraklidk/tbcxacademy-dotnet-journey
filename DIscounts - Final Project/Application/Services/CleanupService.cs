using Domain.Constants;
using Application.Interfaces.Repos;
using Application.Interfaces.Services;

namespace Application.Services;

public class CleanupService : ICleanupService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IReservationRepository _reservationRepository;

    public CleanupService(IOfferRepository offerRepository,
                          IReservationRepository reservationRepository)
    {
        _offerRepository = offerRepository;
        _reservationRepository = reservationRepository;
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

        if (dict.Count > 0)
        {
            var offerToIncreaseCoupon = await _offerRepository.GetByIdsAsync(dict.Keys.ToList(), ct).ConfigureAwait(false);
            foreach (var offer in offerToIncreaseCoupon) offer.RemainingCoupons += dict[offer.Id];
        }

        await _offerRepository.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
