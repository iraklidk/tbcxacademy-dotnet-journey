namespace Application.DTOs.GlobalSettings;

public class UpdateGlobalSettingsDto
{
    public int Id { get; set; } = 1;

    public ushort BookingDurationMinutes { get; set; }

    public ushort MerchantEditHours { get; set; }

    public decimal ReservationPrice { get; set; }
}
