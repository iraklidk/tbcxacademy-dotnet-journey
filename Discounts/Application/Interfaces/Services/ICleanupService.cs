namespace Application.Interfaces.Services;

public interface ICleanupService
{
    Task CleanupAsync(CancellationToken ct = default);
}
