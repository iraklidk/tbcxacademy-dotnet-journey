using FluentValidation;
using Application.Services;
using FluentValidation.AspNetCore;
using Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IOfferService, OfferService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<ICleanupService, CleanupService>();
        services.AddScoped<IMerchantService, MerchantService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IMerchantService, MerchantService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IGlobalSettingsService, GlobalSettingsService>();
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        return services;
    }
}
