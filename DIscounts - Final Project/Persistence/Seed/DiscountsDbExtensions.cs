using Domain.Entities;
using Domain.Constants;
using Persistence.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Discounts.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Discounts.Persistence.Seeding;

public static class DiscountsDbExtensions
{
    public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app, CancellationToken ct = default)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DiscountsDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        if (await context.Users.AnyAsync(ct).ConfigureAwait(false))
            return app;

        // Seed roles
        var roles = new[] { "Admin", "Merchant", "Customer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role).ConfigureAwait(false))
                await roleManager.CreateAsync(new IdentityRole<int>(role)).ConfigureAwait(false);
        }

        // Seed Admin
        var adminUser = new User
        {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser, "last").ConfigureAwait(false);
        await userManager.AddToRoleAsync(adminUser, "Admin").ConfigureAwait(false);

        // Seed categories
        var categories = new[]
        {
                new Category { Name = "Food", Description = "Food & Drinks" },
                new Category { Name = "Electronics", Description = "Gadgets & Devices" },
                new Category { Name = "Health", Description = "Health & Wellness" }
            };
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);
        var allCategories = await context.Categories.ToListAsync(ct).ConfigureAwait(false);

        // Seed merchants & Offers
        var merchants = new List<Merchant>();
        for (var i = 1; i <= 5; i++)
        {
            var merchantUser = new User
            {
                UserName = $"merchant{i}",
                Email = $"merchant{i}@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(merchantUser, $"last").ConfigureAwait(false);
            await userManager.AddToRoleAsync(merchantUser, "Merchant").ConfigureAwait(false);

            var merchant = new Merchant
            {
                Name = $"Merchant {i}",
                UserId = merchantUser.Id
            };
            context.Merchants.Add(merchant);
            await context.SaveChangesAsync(ct).ConfigureAwait(false);
            merchants.Add(merchant);

            for (var j = 1; j <= 3; j++)
            {
                var category = allCategories[(i + j) % allCategories.Count];
                var offer = new Offer
                {
                    Title = $"Offer {i}-{j}",
                    Description = $"Special discount {i}-{j}",
                    OriginalPrice = 100 + j * 50,
                    DiscountedPrice = 50 + j * 20,
                    TotalCoupons = 10 + j,
                    RemainingCoupons = 10 + j,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                    Status = OfferStatus.Approved,
                    MerchantId = merchant.Id,
                    CategoryId = category.Id
                };
                context.Offers.Add(offer);
            }
            await context.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        // Seed customers
        var customers = new List<Customer>();
        for (var i = 1; i <= 5; i++)
        {
            var customerUser = new User
            {
                UserName = $"customer{i}",
                Email = $"customer{i}@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(customerUser, $"last").ConfigureAwait(false);
            await userManager.AddToRoleAsync(customerUser, "Customer").ConfigureAwait(false);

            var customer = new Customer
            {
                UserId = customerUser.Id,
                Firstname = $"Customer{i}",
                Lastname = "Test",
                Balance = (decimal)5000
            };
            context.Customers.Add(customer);
            await context.SaveChangesAsync(ct).ConfigureAwait(false);
            customers.Add(customer);
        }
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        // Seed GlobalSettings
        if (!await context.GlobalSettings.AnyAsync(ct).ConfigureAwait(false))
        {
            context.GlobalSettings.Add(new GlobalSettings
            {
                BookingDurationMinutes = 30,
                MerchantEditHours = 24
            });
        }

        await context.SaveChangesAsync(ct).ConfigureAwait(false);
        return app;
    }

    // Resets database
    public static async Task ResetDbAsync(this DiscountsDbContext context, CancellationToken ct = default)
    {
        var userRoles = await context.Set<IdentityUserRole<int>>().ToListAsync(ct).ConfigureAwait(false);
        context.Set<IdentityUserRole<int>>().RemoveRange(userRoles);
        var roles = await context.Roles.ToListAsync(ct).ConfigureAwait(false);
        context.Roles.RemoveRange(roles);
        var users = await context.Users.ToListAsync(ct).ConfigureAwait(false);
        context.Users.RemoveRange(users); context.Reservations.RemoveRange(await context.Reservations.ToListAsync(ct).ConfigureAwait(false));
        context.Coupons.RemoveRange(await context.Coupons.ToListAsync(ct).ConfigureAwait(false));
        context.Offers.RemoveRange(await context.Offers.ToListAsync(ct).ConfigureAwait(false));
        context.Merchants.RemoveRange(await context.Merchants.ToListAsync(ct).ConfigureAwait(false));
        context.Categories.RemoveRange(await context.Categories.ToListAsync(ct).ConfigureAwait(false));
        context.Customers.RemoveRange(await context.Customers.ToListAsync(ct).ConfigureAwait(false));
        await context.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
