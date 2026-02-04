using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOs.Author
{
    public class UpdateAuthorRequest
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        public string? Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
