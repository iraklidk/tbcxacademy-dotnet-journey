using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.SeedROles;

public static class SeedRole
{
    public static async Task<WebApplication> AddRolesAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            var roles = new string[] { "Admin", "Merchant", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role).ConfigureAwait(false))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role)).ConfigureAwait(false);
                }
            }
        }
        return app;
    }
}
