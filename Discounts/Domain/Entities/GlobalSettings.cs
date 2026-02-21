namespace Domain.Entities;

public class GlobalSettings
{
    public int Id { get; set; }

    public ushort BookingDurationMinutes { get; set; } = 30;

    public ushort MerchantEditHours { get; set; } = 24;

    public int ReservationPrice { get; set; } = 10;
}
