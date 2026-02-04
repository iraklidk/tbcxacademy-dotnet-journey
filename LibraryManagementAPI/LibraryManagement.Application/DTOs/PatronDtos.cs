namespace LibraryManagement.Application.DTOs
{
    public class CreatePatronDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? MembershipDate { get; set; } = DateTime.Now;
    }

    public class PatronDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? MembershipDate { get; set; } = DateTime.Now;
}

    public class UpdatePatronDto : CreatePatronDto { }
}
