using Moq;
using Xunit;
using FluentAssertions;
using Persistence.Identity;
using Application.Services;
using Application.DTOs.Auth;
using Application.Interfaces;
using Application.DTOs.Customer;
using Application.DTOs.Merchant;
using Microsoft.AspNetCore.Http;
using Application.Exceptions.User;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces.Services;
using Discounts.Application.Exceptions;

namespace Application.Tests.Services;

public class AuthServiceTests
{
    private readonly AuthService _sut;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IEmailService> _emailMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly Mock<IMerchantService> _merchantServiceMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;

    public AuthServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _emailMock = new Mock<IEmailService>();
        _customerServiceMock = new Mock<ICustomerService>();
        _merchantServiceMock = new Mock<IMerchantService>();

        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
        _signInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object, contextAccessorMock.Object, claimsFactoryMock.Object,
                                                           null!, null!, null!, null!);

        _sut = new AuthService(
            _uowMock.Object,
            _emailMock.Object,
            _userManagerMock.Object,
            _customerServiceMock.Object,
            _merchantServiceMock.Object,
            _signInManagerMock.Object);
    }

    #region Login Tests
    [Theory]
    [InlineData("wrongpass", true, "Invalid password!")]
    [InlineData("pass", false, "User account is blocked!")]
    public async Task LoginAsync_ShouldThrow_WhenInvalid(string password, bool isActive, string expectedMessage)
    {
        // Arrange
        var user = new User { UserName = "testuser", IsActive = isActive };
        _userManagerMock.Setup(u => u.FindByNameAsync("testuser")).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.CheckPasswordAsync(user, password)).ReturnsAsync(password == "pass");

        // Act
        var request = new LoginRequest { UserName = "testuser", Password = password };
        var act = () => _sut.LoginAsync(request);

        // Assert
        if (isActive) await act.Should().ThrowAsync<ValidationException>().WithMessage(expectedMessage);
        else await act.Should().ThrowAsync<DomainException>().WithMessage(expectedMessage);
    }
    #endregion

    #region Registration Tests
    [Theory]
    [InlineData("email", "taken@test.com")]
    [InlineData("username", "existingUser")]
    public async Task RegisterAsync_ShouldThrow_WhenDuplicate(string type, string value)
    {
        // Arrange
        var request = type == "email"
            ? new RegisterRequest { UserName = "newuser", Email = value }
            : new RegisterRequest { UserName = value, Email = "new@test.com" };

        if (type == "email") _userManagerMock.Setup(u => u.FindByEmailAsync(value)).ReturnsAsync(new User());
        else _userManagerMock.Setup(u => u.FindByNameAsync(value)).ReturnsAsync(new User());

        // Act & Assert
        await _sut.Invoking(s => s.RegisterAsync(request)).Should().ThrowAsync<UserAlreadyExists>();
        _uowMock.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("Customer")]
    [InlineData("Merchant")]
    public async Task RegisterAsync_ShouldCreateUserAndCallProperService(string role)
    {
        // Arrange
        var request = new RegisterRequest { UserName = "user", Role = role, Email = "test@test.com", Password = "Pass123!" };

        _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<User>(), role)).ReturnsAsync(IdentityResult.Success);

        // Act
        await _sut.RegisterAsync(request);

        // Assert
        if (role == "Customer")
        {
            _customerServiceMock.Verify(s => s.AddCustomerAsync(It.IsAny<CreateCustomerDto>(), It.IsAny<CancellationToken>()), Times.Once);
            _merchantServiceMock.Verify(s => s.AddMerchantAsync(It.IsAny<CreateMerchantDto>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        else
        {
            _merchantServiceMock.Verify(s => s.AddMerchantAsync(It.IsAny<CreateMerchantDto>(), It.IsAny<CancellationToken>()), Times.Once);
            _customerServiceMock.Verify(s => s.AddCustomerAsync(It.IsAny<CreateCustomerDto>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
    #endregion

    #region Password Reset Test
    [Theory]
    [InlineData("test1@test.com", true)]
    [InlineData("missing2@test.com", false)]
    public async Task ForgotPasswordAsync_ShouldThrowUserNotFound_WhenEmailDoesNotExist(string email, bool exists)
    {
        // Arrange
        var user = exists ? new User { Email = email } : null;

        _userManagerMock.Setup(u => u.FindByEmailAsync(email)).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.GeneratePasswordResetTokenAsync(user!)).ReturnsAsync("reset-token");
        _userManagerMock.Setup(u => u.ResetPasswordAsync(user!, "reset-token", It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        // Act
        Func<Task> act = async () => await _sut.ForgotPasswordAsync(email).ConfigureAwait(false);

        // Assert
        if (!exists)
        {
            await act.Should().ThrowAsync<UserNotFound>();
            _userManagerMock.Verify(u => u.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Never);
        }
        else
        {
            await act();
            _userManagerMock.Verify(u => u.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Once);
            _userManagerMock.Verify(u => u.ResetPasswordAsync(user!, "reset-token", It.IsAny<string>()), Times.Once);
        }
    }
    #endregion

}
