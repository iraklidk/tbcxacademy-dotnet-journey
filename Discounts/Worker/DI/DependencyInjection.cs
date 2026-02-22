using Worker.CleanupService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
