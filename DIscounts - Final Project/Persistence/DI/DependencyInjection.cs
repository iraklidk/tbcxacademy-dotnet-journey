using Persistence.Identity;
using Application.Interfaces;
using Infrastructure.Persistence;
using Application.Interfaces.Repos;
using Discounts.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Discounts.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Persistence.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
        })
                .AddEntityFrameworkStores<DiscountsDbContext>()
                .AddDefaultTokenProviders();

        services.AddDbContext<DiscountsDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddHealthChecks()
                .AddDbContextCheck<DiscountsDbContext>(
                    name: "SQL Database",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "db", "sql" });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IMerchantRepository, MerchantRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IGlobalSettingsRepository, GlobalSettingsRepository>();
        services.AddCors();
        return services;
    }
}
