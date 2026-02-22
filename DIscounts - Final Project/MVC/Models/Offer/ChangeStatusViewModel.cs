using Domain.Constants;

namespace MVC.Models.Offer;

public class ChangeStatusViewModel
{
    public OfferStatus Status { get; set; } = OfferStatus.Pending;
}
