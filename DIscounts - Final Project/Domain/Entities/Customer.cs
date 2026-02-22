namespace Domain.Entities;

public class Customer
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Balance { get; set; } = 5000;

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
