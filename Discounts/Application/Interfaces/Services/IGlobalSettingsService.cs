using Application.DTOs.GlobalSettings;

namespace Application.Interfaces.Services;

public interface IGlobalSettingsService
{
    Task<GlobalSettingsDto> GetSettingsAsync(CancellationToken ct = default);

    Task UpdateSettingsAsync(UpdateGlobalSettingsDto settings, CancellationToken ct = default);
}
