using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOs.Book
{
    public class UpdateBookRequest
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string ISBN { get; set; } = null!;
        [Required]
        public int PublicationYear { get; set; }
        [Required]
        public string? Author { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
