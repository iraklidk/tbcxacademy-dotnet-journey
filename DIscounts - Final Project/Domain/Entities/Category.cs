namespace Domain.Entities;

public class Category
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
