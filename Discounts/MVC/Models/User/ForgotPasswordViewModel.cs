using System.ComponentModel.DataAnnotations;

namespace MVC.Models.User;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}
