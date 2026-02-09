using LibraryManagement.Api.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication RegisterMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        app.UseVersionedSwaggerUI();

        app.UseRateLimiter();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.MapControllers();

        return app;
    }
}
