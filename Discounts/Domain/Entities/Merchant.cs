namespace Domain.Entities;

public class Merchant
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Balance { get; set; } = 5000;

    public string Name { get; set; } = null!;

    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
