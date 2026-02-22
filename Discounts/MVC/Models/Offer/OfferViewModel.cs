using Domain.Constants;

namespace MVC.Models.Offer;

public class OfferViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int MerchantId { get; set; }

    public string Category { get; set; } = null!;

    public int CategoryId { get; set; }

    public decimal OriginalPrice { get; set; }

    public string Description { get; set; } = null!;

    public DateTime EndDate { get; set; }

    public decimal DiscountedPrice { get; set; }

    public int TotalCoupons { get; set; }

    public int RemainingCoupons { get; set; }

    public OfferStatus Status { get; set; }

    public bool IsReserved { get; set; }
}
