using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Worker.CleanupService;

namespace Worker.DI;

public static class DependencyInjection
{
    public static IServiceCollection RegisterWorker(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<WorkerSettings>(config.GetSection("WorkerSettings"));
        services.AddHostedService<BackgroundWorker>();
        return services;
    }
}
