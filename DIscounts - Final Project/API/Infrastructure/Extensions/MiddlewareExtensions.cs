using Identity.SeedROles;

public static class MiddlewareExtensions
{
    public static async Task<WebApplication> RegisterMiddlewares(this WebApplication app)
    {
        app.AddHealthCheck();
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        app.UseVersionedSwaggerUI();
        await app.AddRolesAsync().ConfigureAwait(false);
        app.UseCors("AllowWebApp");
        // app.UseRateLimiter();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}
