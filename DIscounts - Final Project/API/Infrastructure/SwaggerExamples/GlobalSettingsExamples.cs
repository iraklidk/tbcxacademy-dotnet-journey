using Swashbuckle.AspNetCore.Filters;
using Application.DTOs.GlobalSettings;

namespace API.Infrastructure.SwaggerExamples;

public class UpdateGlobalSettingsDtoExample : IExamplesProvider<UpdateGlobalSettingsDto>
{
    public UpdateGlobalSettingsDto GetExamples()
    {
        return new UpdateGlobalSettingsDto
        {
            ReservationPrice = 10,
            MerchantEditHours = 24,
            BookingDurationMinutes = 15
        };
    }
}


