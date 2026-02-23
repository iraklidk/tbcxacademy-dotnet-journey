namespace Application.DTOs.Customer;

public class CreateCustomerDto
{
    public int UserId { get; set; }

    public decimal Balance { get; set; } = 100;

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;
}
