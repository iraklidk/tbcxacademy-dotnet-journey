using System.ComponentModel.DataAnnotations;

namespace MVC.Models.Merchant;

public class MerchantRegisterViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string UserName { get; set; } = null!;

    [Required, MinLength(6), DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required, Compare("Password"), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;

    [Required, Phone]
    public string PhoneNumber { get; set; } = null!;

    public string Role { get; set; } = "Merchant";
}
