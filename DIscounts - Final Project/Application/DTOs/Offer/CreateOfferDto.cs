namespace Application.DTOs.Offer;

public class CreateOfferDto
{
    public int UserId { get; set; }

    public string? Title { get; set; }

    public int CategoryId { get; set; }

    public int TotalCoupons { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime StartDate { get; set; }

    public string? Description { get; set; }

    public decimal OriginalPrice { get; set; }

    public decimal DiscountedPrice { get; set; }
}
