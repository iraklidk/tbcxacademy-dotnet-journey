using Application.DTOs.Customer;

namespace Application.Interfaces.Services;

public interface ICustomerService
{
    Task<CustomerDto> AddCustomerAsync(CreateCustomerDto customer, CancellationToken ct = default);

    Task<CustomerDto?> GetCustomerByUserIdAsync(int userId, CancellationToken ct = default);

    Task UpdateCustomerAsync(UpdateCustomerDto customer, CancellationToken ct = default);

    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync(CancellationToken ct = default);

    Task<CustomerDto?> GetCustomerByIdAsync(int id, CancellationToken ct = default);

    Task DeleteCustomerAsync(int customerId, CancellationToken ct = default);
}
