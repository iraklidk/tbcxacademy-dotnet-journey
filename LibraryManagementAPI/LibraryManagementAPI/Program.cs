using LibraryManagement.Infrastructure.Extensions;
using LibraryManagement.Persistence.Context;
using LibraryManagement.Persistence.Seeding;
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

app.UseVersionedSwaggerUI();

// Use rate limiter middleware
app.UseRateLimiter();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

//await dbContext.ResetDbAsync();

dbContext.Database.Migrate();
if (app.Environment.IsDevelopment())
{
    await dbContext.SeedAsync();
}

app.Run();
