public static class MiddlewareExtensions
{
    public static async Task<WebApplication> RegisterMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        app.AddHealthCheck();
        app.UseVersionedSwaggerUI();
        app.UseRateLimiter();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowWebApp");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}
