using Mapster;
using Domain.Entities;
using MVC.Models.Offer;
using Domain.Constants;
using Application.DTOs.Offer;

public static class MapsterConfigMvc {
    public static IServiceCollection RegisterMappingsMvc(this IServiceCollection services)
    {
        TypeAdapterConfig<Offer, OfferViewModel>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.EndDate, src => src.EndDate)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.OriginalPrice, src => src.OriginalPrice)
            .Map(dest => dest.DiscountedPrice, src => src.DiscountedPrice);

        TypeAdapterConfig<CreateOfferDto, Offer>.NewConfig()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Category)
            .Ignore(dest => dest.Merchant)
            .Ignore(dest => dest.Reservations)
            .Map(dest => dest.Created, _ => DateTime.UtcNow)
            .Map(dest => dest.Updated, _ => DateTime.UtcNow)
            .Map(dest => dest.Status, _ => OfferStatus.Pending)
            .Map(dest => dest.RemainingCoupons, src => src.TotalCoupons);

        return services;
    }
}
