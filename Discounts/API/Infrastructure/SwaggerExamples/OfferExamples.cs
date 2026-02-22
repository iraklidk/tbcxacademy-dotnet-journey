using Domain.Constants;
using Application.DTOs.Offer;
using Swashbuckle.AspNetCore.Filters;

namespace API.Infrastructure.SwaggerExamples;

public class CreateOfferDtoExample : IExamplesProvider<CreateOfferDto>
{
    public CreateOfferDto GetExamples()
    {
        return new CreateOfferDto
        {
            Title = "50% Off on Headphones",
            Description = "Limited time offer for premium headphones",
            OriginalPrice = 200.00m,
            DiscountedPrice = 100.00m,
            TotalCoupons = 50,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            UserId = 1,
            CategoryId = 3
        };
    }
}

public class OfferDtoExample : IExamplesProvider<OfferDto>
{
    public OfferDto GetExamples()
    {
        return new OfferDto
        {
            Id = 101,
            Title = "50% Off on Headphones",
            Description = "Limited time offer for premium headphones",
            OriginalPrice = 200.00m,
            DiscountedPrice = 100.00m,
            TotalCoupons = 50,
            RemainingCoupons = 20,
            StartDate = DateTime.UtcNow.AddDays(-5),
            EndDate = DateTime.UtcNow.AddDays(25),
            Created = DateTime.UtcNow.AddDays(-10),
            Updated = DateTime.UtcNow.AddDays(-2),
            Status = OfferStatus.Approved,
            MerchantId = 1,
            MerchantName = "SuperShop",
            CategoryId = 2,
            Category = "Electronics",
            ReservationsCount = 15,
            CouponsCount = 30
        };
    }
}
public class UpdateOfferDtoExample : IExamplesProvider<UpdateOfferDto>
{
    public UpdateOfferDto GetExamples()
    {
        return new UpdateOfferDto
        {
            Id = 101,
            Title = "50% Off on Headphones - Updated",
            Description = "Extended offer for premium headphones",
            DiscountedPrice = 95.00m,
            RemainingCoupons = 15,
            EndDate = DateTime.UtcNow.AddDays(35),
            Status = OfferStatus.Expired,
        };
    }
}
