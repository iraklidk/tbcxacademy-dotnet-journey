using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.Services;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        services.AddScoped<IGlobalSettingsService, GlobalSettingsService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IMerchantService, MerchantService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IMerchantService, MerchantService>();
        services.AddScoped<ICleanupService, CleanupService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IOfferService, OfferService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
