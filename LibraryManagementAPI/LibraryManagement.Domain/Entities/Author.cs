using System.ComponentModel.DataAnnotations;

public class Author
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    public string? Biography { get; set; }
    public DateTime? DateOfBirth { get; set; }
    // Navigation
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
