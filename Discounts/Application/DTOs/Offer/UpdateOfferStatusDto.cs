using Domain.Constants;

namespace Application.DTOs.Offer;

public class UpdateOfferStatusDto
{
    public int Id { get; set; }

    public OfferStatus Status { get; set; }
}
