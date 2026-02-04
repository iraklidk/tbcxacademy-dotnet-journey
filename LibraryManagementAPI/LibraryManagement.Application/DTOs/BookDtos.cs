namespace LibraryManagement.Application.DTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public int PublicationYear { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public int Quantity { get; set; }
        public string Author { get; set; } = null!;

    }

    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public int PublicationYear { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public int Quantity { get; set; }
        public int AuthorId { get; set; }
    }

    public class UpdateBookDto : CreateBookDto { }
}
