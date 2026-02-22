using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOs.Book
{
    public class CreateBookRequest
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string? Author { get; set; }
        [Required]
        public string ISBN { get; set; } = null!;
        public int PublicationYear { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
