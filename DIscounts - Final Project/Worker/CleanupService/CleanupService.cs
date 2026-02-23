using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Worker.CleanupService;

internal class BackgroundWorker : BackgroundService
{
    private readonly WorkerSettings _settings;
    private readonly ILogger<BackgroundWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public BackgroundWorker(IOptions<WorkerSettings> options,
                            ILogger<BackgroundWorker> logger,
                            IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _settings = options.Value;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cleanup Worker started.");

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
