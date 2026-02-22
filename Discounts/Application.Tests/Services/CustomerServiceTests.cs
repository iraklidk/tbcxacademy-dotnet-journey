using Moq;
using Xunit;
using Domain.Entities;
using FluentAssertions;
using Application.Services;
using Persistence.Identity;
using System.Linq.Expressions;
using Application.DTOs.Customer;
using Application.Interfaces.Repos;
using Microsoft.AspNetCore.Identity;
using Discounts.Application.Exceptions;

namespace Application.Tests.Services;

public class CustomerServiceTests
{
    private readonly CustomerService _sut;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ICustomerRepository> _customerRepoMock;

    public CustomerServiceTests()
    {
        var store = new Mock<IUserStore<User>>();
        _customerRepoMock = new Mock<ICustomerRepository>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _sut = new CustomerService(_userManagerMock.Object, _customerRepoMock.Object);
    }

    #region Get Tests
    [Fact]
    public async Task GetCustomerByIdAsync_ShouldReturnMappedDto_WhenCustomerExists()
    {
        // Arrange
        var customerId = 1;
        var customer = new Customer { Id = customerId, Firstname = "Alice" };

        _customerRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _sut.GetCustomerByIdAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result!.Firstname.Should().Be("Alice");
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ShouldThrowNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        _customerRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await _sut.Invoking(s => s.GetCustomerByIdAsync(99))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetCustomerByUserIdAsync_ShouldThrowNotFound_WhenIdentityUserMissing()
    {
        // Arrange
        _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync((User?)null);

        // Act & Assert
        await _sut.Invoking(s => s.GetCustomerByUserIdAsync(1))
            .Should().ThrowAsync<NotFoundException>().WithMessage("*User with id 1 not found*");
    }

    [Fact]
    public async Task GetCustomerByUserIdAsync_ShouldThrowNotFound_WhenUserExistsInIdentityButNotCustomersTable()
    {
        // Arrange
        var userId = 10;
        _userManagerMock.Setup(u => u.FindByIdAsync("10")).ReturnsAsync(new User { Id = 10 });
        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        // Act & Assert
        await _sut.Invoking(s => s.GetCustomerByUserIdAsync(10))
            .Should().ThrowAsync<NotFoundException>().WithMessage("*Customer with user id 10 not found*");
    }

    [Fact]
    public async Task GetCustomerByUserIdAsync_ShouldReturnCorrectData_WhenBothExist()
    {
        // Arrange
        var customer = new Customer { Id = 1, UserId = 10, Firstname = "John" };
        _userManagerMock.Setup(u => u.FindByIdAsync("10")).ReturnsAsync(new User { Id = 10 });
        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _sut.GetCustomerByUserIdAsync(10);

        // Assert
        result.Should().NotBeNull();
        result!.UserId.Should().Be(10);
    }

    [Fact]
    public async Task GetAllCustomersAsync_ShouldReturnEmpty_WhenNoCustomersExist()
    {
        // Arrange
        _customerRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Customer>());

        // Act
        var result = await _sut.GetAllCustomersAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllCustomersAsync_ShouldReturnMultipleCustomers_WhenDatabaseIsPopulated()
    {
        // Arrange
        var list = new List<Customer> { new(), new(), new() };
        _customerRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list);

        // Act
        var result = await _sut.GetAllCustomersAsync();

        // Assert
        result.Should().HaveCount(3);
    }
    #endregion

    #region Create Tests
    [Fact]
    public async Task AddCustomerAsync_ShouldCallAddAndReturnNewCustomer()
    {
        // Arrange
        var dto = new CreateCustomerDto { UserId = 10, Firstname = "Bob" };
        var createdEntity = new Customer { Id = 1, UserId = 10, Firstname = "Bob" };

        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdEntity);

        // Act
        var result = await _sut.AddCustomerAsync(dto);

        // Assert
        _customerRepoMock.Verify(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result!.Firstname.Should().Be("Bob");
    }
    #endregion

    #region Update Tests
    [Fact]
    public async Task UpdateCustomerAsync_ShouldUpdateExistingEntity_WhenFound()
    {
        // Arrange
        var existing = new Customer { Id = 1, Firstname = "Old" };
        var dto = new UpdateCustomerDto { Id = 1, Firstname = "New" };

        _customerRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        // Act
        await _sut.UpdateCustomerAsync(dto);

        // Assert
        existing.Firstname.Should().Be("New");
        _customerRepoMock.Verify(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldNotCallUpdateRepo_WhenCustomerIsNotFound()
    {
        // Arrange
        _customerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        // Act
        try { await _sut.UpdateCustomerAsync(new UpdateCustomerDto { Id = 99 }); } catch { }

        // Assert
        _customerRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    #endregion

    #region Mapping Validation
    [Fact]
    public async Task AddCustomerAsync_ShouldMapAllFieldsFromDtoToEntity()
    {
        // Arrange
        Customer? capturedEntity = null;
        _customerRepoMock.Setup(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                         .Callback<Customer, CancellationToken>((c, ct) => capturedEntity = c);

        var dto = new CreateCustomerDto { Firstname = "Test", Lastname = "User", Balance = 500 };

        // Act
        await _sut.AddCustomerAsync(dto);

        // Assert
        capturedEntity.Should().NotBeNull();
        capturedEntity!.Firstname.Should().Be("Test");
        capturedEntity.Lastname.Should().Be("User");
        capturedEntity.Balance.Should().Be(500);
    }
    #endregion

    #region Delete
    [Fact]
    public async Task DeleteCustomerAsync_ShouldCallRepoDelete_WithCorrectEntity()
    {
        // Arrange
        var customer = new Customer { Id = 50 };
        _customerRepoMock.Setup(r => r.GetByIdAsync(50, It.IsAny<CancellationToken>())).ReturnsAsync(customer);

        // Act
        await _sut.DeleteCustomerAsync(50);

        // Assert
        _customerRepoMock.Verify(r => r.DeleteAsync(customer, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomerAsync_ShouldThrowNotFound_WhenEntityMissing()
    {
        // Arrange
        _customerRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        // Act & Assert
        await _sut.Invoking(s => s.DeleteCustomerAsync(1))
            .Should().ThrowAsync<NotFoundException>();
    }
    #endregion

    #region Repository Verification
    [Fact]
    public async Task GetCustomerByIdAsync_ShouldVerifyExistsAsyncParameters()
    {
        // Arrange
        _customerRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Customer, bool>>>(), default))
            .ReturnsAsync(true);
        _customerRepoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(new Customer());

        // Act
        await _sut.GetCustomerByIdAsync(1);

        // Assert
        _customerRepoMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Customer, bool>>>(), default), Times.Once);
    }

    [Fact]
    public async Task GetCustomerByUserIdAsync_ShouldCallFindByIdAsyncWithCorrectString()
    {
        // Arrange
        _userManagerMock.Setup(u => u.FindByIdAsync("123")).ReturnsAsync(new User());
        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(123, default)).ReturnsAsync(new Customer());

        // Act
        await _sut.GetCustomerByUserIdAsync(123);

        // Assert
        _userManagerMock.Verify(u => u.FindByIdAsync("123"), Times.Once);
    }
    #endregion

}
