using BookStoreApi.Application.Models;

namespace Infrastructure.Extensions
{
    internal class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            return services;
        }
    }
}