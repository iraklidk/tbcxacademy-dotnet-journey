namespace LibraryManagement.Application.DTOs
{
    public class CreateAuthorDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }

    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ICollection<BookDto>? Books { get; set; }
    }

    public class UpdateAuthorDto : CreateAuthorDto { }
}
