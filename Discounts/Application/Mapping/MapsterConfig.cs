using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Application.DTOs.Merchant;
using Application.DTOs.Offer;
using Application.DTOs.User;
using Persistence.Identity;
using Domain.Entities;
using Mapster;

public static class MapsterConfig
{
    public static IServiceCollection RegisterMaps(this IServiceCollection services)
    {
        TypeAdapterConfig<Offer, OfferDto>
            .NewConfig()
            .Map(dest => dest.ReservationsCount, src => src.Reservations.Count)
            .Map(dest => dest.MerchantName, src => src.Merchant!.Name)
            .Map(dest => dest.Category, src => src.Category!.Name);

        TypeAdapterConfig<CreateOfferDto, Offer>
            .NewConfig()
            .Ignore(dest => dest.Category)
            .Ignore(dest => dest.Updated)
            .Ignore(dest => dest.Created)
            .Ignore(dest => dest.Id);

        TypeAdapterConfig<IdentityUser<int>, UserDto>.NewConfig()
            .Map(dest => dest.UserName, src => src.UserName ?? "Unknown");

        TypeAdapterConfig<UpdateOfferDto, Offer>
            .NewConfig()
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

        TypeAdapterConfig<UpdateOfferDto, Offer>
            .NewConfig()
            .Ignore(dest => dest.Created);

        TypeAdapterConfig<User, UserDto>
            .NewConfig()
            .Map(dest => dest.IsActive, src => src.IsActive);

        return services;
    }
}
