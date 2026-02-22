using LibraryManagement.Persistence.Seeding;
using LibraryManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

        await dbContext.ResetDbAsync();
        dbContext.Database.Migrate();
        if (app.Environment.IsDevelopment())
            await dbContext.SeedAsync();
    }
}
