using Application.DTOs.Offer;
using Domain.Constants;
using MVC.Models.Offer;
using Domain.Entities;
using Mapster;

public static class MapsterConfigMvc {
    public static IServiceCollection RegisterMappingsMvc(this IServiceCollection services)
    {
        TypeAdapterConfig<Offer, OfferViewModel>.NewConfig()
            .Map(dest => dest.DiscountedPrice, src => src.DiscountedPrice)
            .Map(dest => dest.OriginalPrice, src => src.OriginalPrice)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.EndDate, src => src.EndDate)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Id, src => src.Id);

        TypeAdapterConfig<CreateOfferDto, Offer>
            .NewConfig()
            .Map(dest => dest.RemainingCoupons, src => src.TotalCoupons)
            .Map(dest => dest.Status, _ => OfferStatus.Pending)
            .Map(dest => dest.Updated, _ => DateTime.UtcNow)
            .Map(dest => dest.Created, _ => DateTime.UtcNow)
            .Ignore(dest => dest.Reservations)
            .Ignore(dest => dest.Merchant)
            .Ignore(dest => dest.Category)
            .Ignore(dest => dest.Id);

        return services;
    }
}
