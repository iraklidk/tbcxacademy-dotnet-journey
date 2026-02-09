using System.ComponentModel.DataAnnotations;

public class BorrowRecord
{
    [Key]
    public int Id { get; set; }
    // Foreign Keys
    public int BookId { get; set; }
    public int PatronId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public BorrowStatus Status { get; set; }
    // Navigation
    public Book Book { get; set; } = null!;
    public Patron Patron { get; set; } = null!;
}
