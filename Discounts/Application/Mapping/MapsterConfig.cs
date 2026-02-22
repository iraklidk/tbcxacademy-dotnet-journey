using Mapster;
using Domain.Entities;
using Persistence.Identity;
using Application.DTOs.User;
using Application.DTOs.Offer;
using Application.DTOs.Merchant;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class MapsterConfig
{
    public static IServiceCollection RegisterMaps(this IServiceCollection services)
    {
        TypeAdapterConfig<Offer, OfferDto>.NewConfig()
            .Map(dest => dest.ReservationsCount, src => src.Reservations.Count)
            .Map(dest => dest.MerchantName, src => src.Merchant!.Name)
            .Map(dest => dest.Category, src => src.Category!.Name);

        TypeAdapterConfig<CreateOfferDto, Offer>.NewConfig()
            .Ignore(dest => dest.Category)
            .Ignore(dest => dest.Updated)
            .Ignore(dest => dest.Created)
            .Ignore(dest => dest.Id)
            .Map(dest => dest.RemainingCoupons, src => src.TotalCoupons);

        TypeAdapterConfig<IdentityUser<int>, UserDto>.NewConfig()
            .Map(dest => dest.UserName, src => src.UserName ?? "Unknown");

        TypeAdapterConfig<UpdateOfferDto, Offer>.NewConfig()
            .Ignore(dest => dest.Created);

        TypeAdapterConfig<Merchant, MerchantResponseDto>.NewConfig()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Id, src => src.Id);

        TypeAdapterConfig<Offer, OfferDto>
            .NewConfig()
            .Map(dest => dest.ReservationsCount, src => src.Reservations.Count)
            .Map(dest => dest.MerchantName, src => src.Merchant!.Name)
            .Map(dest => dest.Category, src => src.Category!.Name);

        TypeAdapterConfig<UpdateOfferDto, Offer>.NewConfig()
            .Ignore(dest => dest.Created);

        TypeAdapterConfig<User, UserDto>.NewConfig()
            .Map(dest => dest.IsActive, src => src.IsActive);

        return services;
    }
}
