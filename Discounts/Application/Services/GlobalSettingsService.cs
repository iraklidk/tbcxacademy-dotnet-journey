using Application.Interfaces.Services;
using Application.DTOs.GlobalSettings;
using Application.Interfaces.Repos;
using Mapster;

namespace Application.Services;

public class GlobalSettingsService : IGlobalSettingsService
{
    private readonly IGlobalSettingsRepository _globalSettingsRepository;

    public GlobalSettingsService(IGlobalSettingsRepository globalSettingsRepository)
    {
        _globalSettingsRepository = globalSettingsRepository;
    }

    public async Task<GlobalSettingsDto> GetSettingsAsync(CancellationToken ct)
    {
        var entity = await _globalSettingsRepository.GetByIdAsync(1, ct).ConfigureAwait(false);
        return entity.Adapt<GlobalSettingsDto>();
    }

    public async Task UpdateSettingsAsync(UpdateGlobalSettingsDto dto, CancellationToken ct)
    {
        var entity = await _globalSettingsRepository.GetByIdAsync(1, ct).ConfigureAwait(false);
        await _globalSettingsRepository.UpdateAsync(dto.Adapt(entity), ct).ConfigureAwait(false);
    }
}
