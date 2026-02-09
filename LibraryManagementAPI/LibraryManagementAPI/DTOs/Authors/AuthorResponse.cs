using LibraryManagementAPI.DTOs.Book;

public class AuthorResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Biography { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public ICollection<BookResponse>? Books { get; set; }
}