using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class HealthController : Controller
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [HttpGet("/health")]
    public async Task<IActionResult> Index()
    {
        var report = await _healthCheckService.CheckHealthAsync().ConfigureAwait(false);

        var model = report.Entries.Select(entry => new HealthCheckViewModel
        {
            Name = entry.Key,
            Status = entry.Value.Status.ToString()
        }).ToList();

        ViewBag.OverallStatus = report.Status.ToString();
        return View(model);
    }
}
