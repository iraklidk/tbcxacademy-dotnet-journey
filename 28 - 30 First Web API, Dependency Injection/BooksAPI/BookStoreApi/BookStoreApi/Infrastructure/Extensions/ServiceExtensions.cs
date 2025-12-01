using BookStoreApi.Application.Services;

namespace BookStoreApi.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //services.AddTransient<IBookService, BookService>();
        //services.AddSingleton<IBookService, BookService>();
        services.AddScoped<IBookService, BookService>();
        return services;
    }
}