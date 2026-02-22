namespace Domain.Entities;

public class GlobalSettings
{
    public int Id { get; set; }

    public int ReservationPrice { get; set; } = 10;

    public ushort MerchantEditHours { get; set; } = 24;

    public ushort BookingDurationMinutes { get; set; } = 30;
}
