using Application.DTOs.GlobalSettings;

namespace Application.Interfaces.Services;

public interface IGlobalSettingsService
{
    Task UpdateSettingsAsync(UpdateGlobalSettingsDto settings, CancellationToken ct = default);

    Task<GlobalSettingsDto> GetSettingsAsync(CancellationToken ct = default);
}
