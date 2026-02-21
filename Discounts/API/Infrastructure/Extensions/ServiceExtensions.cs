using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.RateLimiting;
using Mapster;

public static class ServiceExtensions
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddVersioning();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSwaggerExamplesFromAssemblyOf<Program>();
        AddCustomRateLimiting(services);
        services.RegisterMaps();
        services.AddMapster();
    }

    public static void AddCustomRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("FixedWindow", limiterOptions =>
            {
                limiterOptions.PermitLimit = 10;
                limiterOptions.Window = TimeSpan.FromMinutes(1);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 2;
            });

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
            });

            options.RejectionStatusCode = 429;

            options.OnRejected = async (context, ct) =>
            {
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(
                    "{\"error\": \"Too many requests. Please try again later.\"}",
                    ct).ConfigureAwait(false);
            };
        });
    }
}
