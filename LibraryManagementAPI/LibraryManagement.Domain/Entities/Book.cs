using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Book
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string ISBN { get; set; } = null!;
    public int PublicationYear { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public int Quantity { get; set; }
    // Foreign Key
    public int AuthorId { get; set; }
    // Navigation
    public Author Author { get; set; } = null!;
}
