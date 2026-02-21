using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Worker.CleanupService;

internal class BackgroundWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BackgroundWorker> _logger;
    private readonly WorkerSettings _settings;

    public BackgroundWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<BackgroundWorker> logger,
        IOptions<WorkerSettings> options)
    {
        _scopeFactory = scopeFactory;
        _settings = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Expired Cleanup Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var cleanupService = scope.ServiceProvider
                    .GetRequiredService<ICleanupService>();

                await cleanupService.CleanupAsync(stoppingToken).ConfigureAwait(false);

                _logger.LogInformation("Expired reservations & offers cleaned.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cleanup error.");
            }

            await Task.Delay(TimeSpan.FromMinutes(_settings.CleanupIntervalMinutes), stoppingToken).ConfigureAwait(false);
        }
    }
}
