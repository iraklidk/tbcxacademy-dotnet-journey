using Worker.DI;
using Application.DI;
using Persistence.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddContainer(this IServiceCollection services, WebApplicationBuilder builder)
    {;
        builder.AddLogging();
        services.AddApplication();
        services.AddPresentationServices();
        services.RegisterWorker(builder.Configuration);
        services.AddInfrastructure(builder.Configuration);
        return services;
    }
}
