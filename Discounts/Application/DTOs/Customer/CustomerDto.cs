namespace Application.DTOs.Customer;

public class CustomerDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Balance { get; set; } = 100;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;
}
