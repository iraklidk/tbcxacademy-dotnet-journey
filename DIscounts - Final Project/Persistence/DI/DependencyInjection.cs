using Persistence.Identity;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Infrastructure.Persistence;
using Application.Interfaces.Repos;
using Microsoft.AspNetCore.Identity;
using Discounts.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Discounts.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
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

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.LogoutPath = "/Account/Logout";
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
            options.SlidingExpiration = true;
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            options.Events.OnRedirectToLogin = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    return context.Response.WriteAsync(
                        "{\"error\":\"You are not authenticated.\"}");
                }

                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    return context.Response.WriteAsync(
                        "{\"error\":\"You do not have permission to access this resource.\"}");
                }

                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
        });

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
