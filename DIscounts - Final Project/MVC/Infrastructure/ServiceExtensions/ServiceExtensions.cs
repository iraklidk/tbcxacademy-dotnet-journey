using Worker.DI;
using Persistence.DI;
using Application.DI;
using Identity.SeedROles;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

public static class ServiceExtensions
{
    public static IServiceCollection AddContainer(this IServiceCollection services, IConfiguration config)
    {
        services.RegisterMaps();
        services.AddApplication();
        services.RegisterMappingsMvc();
        services.RegisterWorker(config);
        services.AddControllersWithViews();
        services.AddInfrastructure(config);
        return services;
    }

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
        await app.AddRolesAsync().ConfigureAwait(false);
        if (!app.Environment.IsDevelopment())
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
        else
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );
        return app;
    }
}
