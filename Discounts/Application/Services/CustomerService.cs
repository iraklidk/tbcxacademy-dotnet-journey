using Discounts.Application.Exceptions;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces.Repos;
using Application.DTOs.Customer;
using Persistence.Identity;
using Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

internal class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly UserManager<User> _userManager;

    public CustomerService(ICustomerRepository customerRepository,
                           UserManager<User> userManager)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId, CancellationToken ct = default)
    {
        if (!await _customerRepository.ExistsAsync(c => c.Id == customerId, ct).ConfigureAwait(false))
            throw new NotFoundException($"Customer with id {customerId} not found!");
        return (await _customerRepository.GetByIdAsync(customerId, ct).ConfigureAwait(false)).Adapt<CustomerDto>();
    }

    public async Task<IEnumerable<CustomerDto?>> GetAllCustomersAsync(CancellationToken ct = default)
        => (await _customerRepository.GetAllAsync(ct).ConfigureAwait(false)).Adapt<IEnumerable<CustomerDto>>();

    public async Task<CustomerDto?> AddCustomerAsync(CreateCustomerDto customer, CancellationToken ct = default)
    {
        await _customerRepository.AddAsync(customer.Adapt<Customer>(), ct).ConfigureAwait(false);
        return (await _customerRepository.GetCustomerByUserIdAsync(customer.UserId, ct).ConfigureAwait(false)).Adapt<CustomerDto>();
    }

    public async Task UpdateCustomerAsync(UpdateCustomerDto customer, CancellationToken ct = default)
    {
        var entity = await _customerRepository.GetByIdAsync(customer.Id, ct).ConfigureAwait(false);
        if(entity == null) throw new NotFoundException($"Customer with id {customer.Id} not found!");
        await _customerRepository.UpdateAsync(customer.Adapt(entity), ct).ConfigureAwait(false);
    }

    public async Task<CustomerDto?> GetCustomerByUserIdAsync(int userId, CancellationToken ct = default)
    {
        if(!(await _userManager.Users.AnyAsync(u => u.Id == userId, ct).ConfigureAwait(false)))
            throw new NotFoundException($"User with id {userId} not found!");
        var customer = await _customerRepository.GetCustomerByUserIdAsync(userId, ct).ConfigureAwait(false);
        if(customer == null) throw new NotFoundException($"Customer with user id {userId} not found!");
        return (await _customerRepository.GetCustomerByUserIdAsync(userId, ct).ConfigureAwait(false)).Adapt<CustomerDto>();
    }

    public async Task DeleteCustomerAsync(int customerId, CancellationToken ct = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, ct).ConfigureAwait(false);
        if(customer == null) throw new NotFoundException($"Customer with id {customerId} not found!");
        await _customerRepository.DeleteAsync(customer, ct).ConfigureAwait(false);
    }
}
