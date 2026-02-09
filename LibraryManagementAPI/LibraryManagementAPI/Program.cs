using LibraryManagement.Infrastructure.Extensions;
using LibraryManagement.Persistence.Context;
using LibraryManagement.Api.Extensions;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
LibraryApiExtensions.ConfigureLogging();
builder.Host.UseSerilog();

//Register DbContext in DI container
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// Add custom rate limiting
ServiceExtensions.AddCustomRateLimiting(builder);

// API Versioning
builder.Services.AddLibraryApiVersioning();

var app = builder.Build();

app.RegisterMiddlewares();

await app.InitializeDatabaseAsync();

app.Run();
