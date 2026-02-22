using Domain.Constants;
using Application.DTOs.Coupon;
using Swashbuckle.AspNetCore.Filters;

namespace API.Infrastructure.SwaggerExamples;

public class CreateCouponDtoExample : IExamplesProvider<CreateCouponDto>
{
    public CreateCouponDto GetExamples()
    {
        return new CreateCouponDto
        {
            OfferId = 10,
            UserId = 5,
            Code = "DISCOUNT50",
            PurchasedAt = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddMonths(1)
        };
    }
}

public class UpdateCouponDtoExample : IExamplesProvider<UpdateCouponDto>
{
    public UpdateCouponDto GetExamples()
    {
        return new UpdateCouponDto
        {
            Id = 1,
            Status = CouponStatus.Used,
            ExpirationDate = DateTime.UtcNow.AddMonths(1),
            UsedAt = DateTime.UtcNow,
        };
    }
}

public class CouponDtoExample : IExamplesProvider<CouponDto>
{
    public CouponDto GetExamples()
    {
        return new CouponDto
        {
            Code = "DISCOUNT50",
            Status = CouponStatus.Active,
            PurchasedAt = DateTime.UtcNow.AddDays(-2),
            ExpirationDate = DateTime.UtcNow.AddMonths(1),
            UsedAt = DateTime.UtcNow.AddDays(-1),
        };
    }
}
