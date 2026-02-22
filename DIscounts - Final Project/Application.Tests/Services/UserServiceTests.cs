using Moq;
using Xunit;
using FluentAssertions;
using Application.Services;
using Persistence.Identity;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Exceptions.User;
using Application.Interfaces.Repos;
using Microsoft.AspNetCore.Identity;
using Discounts.Application.Exceptions;

namespace Application.Tests.Services;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ICustomerRepository> _custRepoMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole<int>>> _roleManagerMock;

    public UserServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _custRepoMock = new Mock<ICustomerRepository>();
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        var roleStore = new Mock<IRoleStore<IdentityRole<int>>>();
        _roleManagerMock = new Mock<RoleManager<IdentityRole<int>>>(roleStore.Object, null!, null!, null!, null!);
        _sut = new UserService(_uowMock.Object, _userManagerMock.Object, _custRepoMock.Object, _roleManagerMock.Object);
    }

    #region Get Tests
    [Fact]
    public async Task GetUserByIdAsync_ValidId_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 1 };
        _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);

        // Act
        var result = await _sut.GetUserByIdAsync(1, default);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetUserByIdAsync_InvalidId_ThrowsUserNotFound()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByIdAsync("99")).ReturnsAsync((User?)null);

        var act = () => _sut.GetUserByIdAsync(99, default);

        // Act & Assert
        await act.Should().ThrowAsync<UserNotFound>();
    }

    [Fact]
    public async Task GetBatchUsersAsync_EmptyIds_ReturnsEmptyCollection()
    {
        var result = await _sut.GetBatchUsersAsync(new List<int>(), default);
        result.Should().BeEmpty();
    }
    #endregion

    #region Update Tests
    [Fact]
    public async Task UpdateUserAsync_UserNotFound_ThrowsException()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        // Act
        var act = () => _sut.UpdateUserAsync(new UpdateUserDto { Id = 1 });

        // Assert
        await act.Should().ThrowAsync<UserNotFound>();
    }

    [Fact]
    public async Task UpdateUserAsync_WithPassword_CallsResetPassword()
    {
        // Arrange
        var user = new User { Id = 1 };
        _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("token");
        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, "token", "Pass123")).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string>());

        // Act
        await _sut.UpdateUserAsync(new UpdateUserDto { Id = 1, Password = "Pass123", RoleId = 1 });

        // Assert
        _userManagerMock.Verify(x => x.ResetPasswordAsync(user, "token", "Pass123"), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_InvalidRoleId_ThrowsDomainException()
    {
        // Arrange
        var user = new User { Id = 1 };
        _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        // Act
        var act = () => _sut.UpdateUserAsync(new UpdateUserDto { Id = 1, RoleId = 5 });

        // Assert
        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task UpdateUserAsync_Success_CommitsTransaction()
    {
        // Arrange
        var user = new User { Id = 1 };
        _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string>());

        // Act
        await _sut.UpdateUserAsync(new UpdateUserDto { Id = 1, RoleId = 3 });

        // Assert
        _uowMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(1, "Admin")]
    [InlineData(2, "Merchant")]
    [InlineData(3, "Customer")]
    public async Task UpdateUserAsync_ShouldAssignCorrectRoleString(int roleId, string expectedRole)
    {
        // Arrange
        var user = new User { Id = 1, PasswordHash = "OldHash" };

        _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "OldRole" });
        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(user, expectedRole)).ReturnsAsync(IdentityResult.Success);

        var dto = new UpdateUserDto { Id = 1, RoleId = roleId, Password = null };

        // Act
        await _sut.UpdateUserAsync(dto);

        // Assert
        _userManagerMock.Verify(x => x.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()), Times.Once);
        _userManagerMock.Verify(x => x.AddToRoleAsync(user, expectedRole), Times.Once);
        _uowMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(null, 1)]
    [InlineData("Pass123", 1)]
    [InlineData("Pass123", 2)]
    [InlineData("Pass123", 3)]
    public async Task UpdateUserAsync_ShouldResetPasswordOnlyWhenProvided(string? password, int roleId)
    {
        var user = new User { Id = 1 };
        _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("token");
        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, "token", It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string>());

        await _sut.UpdateUserAsync(new UpdateUserDto { Id = 1, Password = password, RoleId = roleId});

        if (password != null)
            _userManagerMock.Verify(x => x.ResetPasswordAsync(user, "token", password), Times.Once);
        else
            _userManagerMock.Verify(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
    #endregion

    #region Delete Tests
    [Fact]
    public async Task DeleteUserAsync_UserExists_CallsDelete()
    {
        // Arrange
        var user = new User { Id = 1 };
        _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Act
        await _sut.DeleteUserAsync(1);

        // Assert
        _userManagerMock.Verify(x => x.DeleteAsync(user), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_UserMissing_ThrowsUserNotFound()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync((User?)null);

        // Act
        var act = () => _sut.DeleteUserAsync(1);

        // Assert
        await act.Should().ThrowAsync<UserNotFound>();
    }
    #endregion

    #region ChangeUserStatusAsync Test
    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task ChangeUserStatusAsync_ShouldToggleStatus(bool initial, bool expected)
    {
        var user = new User { Id = 1, IsActive = initial };
        _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        await _sut.ChangeUserStatusAsync(1);

        user.IsActive.Should().Be(expected);
    }
    #endregion

}
