using LibraryManagement.API.Infrastructure.Mappings;
using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Application.Services.Implementations;
using LibraryManagement.Application.Services.Interfaces;
using LibraryManagement.Persistence.Repositories;
using Mapster;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace LibraryManagement.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.RegisterMaps();
            services.AddMapster();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IPatronService, PatronService>();
            services.AddScoped<IBorrowRecordService, BorrowRecordService>();
            
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IPatronRepository, PatronRepository>();
            services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();
            return services;
        }

        public static void AddCustomRateLimiting(this WebApplicationBuilder builder)
        {
            builder.Services.AddRateLimiter(options =>
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

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync(
                        "{\"error\": \"Too many requests. Please try again later.\"}",
                        cancellationToken);
                };
            });
        }
    }
}
