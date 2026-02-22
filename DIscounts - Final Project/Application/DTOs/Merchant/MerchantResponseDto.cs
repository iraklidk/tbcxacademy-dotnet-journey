namespace Application.DTOs.Merchant;

public class MerchantResponseDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Name { get; set; }

    public decimal Balance { get; set; }
}
