using Identity.SeedROles;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

public static class MiddlewareExtensions
{
    public static async Task<WebApplication> RegisterMiddlewares(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var result = new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description
                    })
                };
                await context.Response.WriteAsJsonAsync(result).ConfigureAwait(false);
            }
        });
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        app.UseVersionedSwaggerUI();
        await app.AddRolesAsync().ConfigureAwait(false);
        app.UseCors("AllowWebApp");
        app.UseHttpsRedirection();
        // app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}
