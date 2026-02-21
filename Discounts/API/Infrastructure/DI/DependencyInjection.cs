using Application.DI;
using Persistence.DI;
using Worker.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddContainer(this IServiceCollection services, IConfiguration config)
    {;
        services.AddInfrastructure(config);
        services.AddPresentationServices();
        services.RegisterWorker(config);
        services.AddApplication();
        return services;
    }
}
