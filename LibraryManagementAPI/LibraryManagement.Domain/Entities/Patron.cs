using System.ComponentModel.DataAnnotations;

public class Patron
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    public DateTime MembershipDate { get; set; }
    // Navigation
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}
