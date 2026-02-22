using Discounts.Persistence.Context;
using Discounts.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DiscountsDbContext>();

        // await dbContext.ResetDbAsync().ConfigureAwait(false);
        dbContext.Database.Migrate();
        if (app.Environment.IsDevelopment())
            await app.SeedDatabaseAsync().ConfigureAwait(false);
    }
}
