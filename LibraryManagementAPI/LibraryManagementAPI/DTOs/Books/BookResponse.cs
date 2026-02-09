namespace LibraryManagementAPI.DTOs.Book
{
    public class BookResponse
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
}
