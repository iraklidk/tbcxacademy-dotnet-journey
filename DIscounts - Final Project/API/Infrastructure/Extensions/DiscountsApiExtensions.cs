using Serilog;
using Asp.Versioning;
using System.Globalization;
using Microsoft.OpenApi.Models;
using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.Filters;

public static class DiscountsApiExtensions
{
    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen(options =>
        {

            using var serviceProvider = services.BuildServiceProvider();
            var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = "Discounts API",
                    Version = description.ApiVersion.ToString()
                });
            }

            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            options.ExampleFilters();

        });

        return services;
    }

    public static IApplicationBuilder UseVersionedSwaggerUI(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                        description.GroupName.ToUpperInvariant());
            }
        });

        return app;
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                formatProvider: CultureInfo.InvariantCulture
            )
            .WriteTo.File(
                "Logs/requests.log",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                formatProvider: CultureInfo.InvariantCulture
            )
            .CreateLogger();
        builder.Host.UseSerilog();
        return builder;
    }
}
