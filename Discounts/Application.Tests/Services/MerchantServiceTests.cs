using Moq;
using Xunit;
using Domain.Entities;
using FluentAssertions;
using Application.Services;
using Application.DTOs.Merchant;
using Application.Interfaces.Repos;
using Discounts.Application.Exceptions;
using Application.Interfaces.Services;

namespace Application.Tests.Services;

public class MerchantServiceTests
{
    private readonly MerchantService _sut;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IOfferRepository> _offerRepoMock;
    private readonly Mock<ICategoryRepository> _categoryRepoMock;
    private readonly Mock<IMerchantRepository> _merchantRepoMock;

    public MerchantServiceTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _offerRepoMock = new Mock<IOfferRepository>();
        _merchantRepoMock = new Mock<IMerchantRepository>();
        _categoryRepoMock = new Mock<ICategoryRepository>();

        _sut = new MerchantService(
            _userServiceMock.Object,
            _offerRepoMock.Object,
            _merchantRepoMock.Object,
            _categoryRepoMock.Object);
    }

    #region Get (Complex Logic) Tests
    [Fact]
    public async Task GetOffersAsync_ShouldMapCategoryNamesToOffers_WhenOffersExist()
    {
        // Arrange
        var userId = 1;
        var merchant = new Merchant { Id = 10 };
        var offers = new List<Offer>
        {
            new() { Id = 1, CategoryId = 5, Title = "Discount 1" },
            new() { Id = 2, CategoryId = 6, Title = "Discount 2" }
        };
        var categories = new List<Category>
        {
            new() { Id = 5, Name = "Electronics" },
            new() { Id = 6, Name = "Food" }
        };

        _merchantRepoMock.Setup(r => r.GetMerchantByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(merchant);
        _offerRepoMock.Setup(r => r.GetByMerchantIdAsync(merchant.Id, It.IsAny<CancellationToken>())).ReturnsAsync(offers);
        _categoryRepoMock.Setup(r => r.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>())).ReturnsAsync(categories);

        // Act
        var result = await _sut.GetOffersAsync(userId);

        // Assert
        result.Should().HaveCount(2);
        result.First(x => x.CategoryId == 5).Category.Should().Be("Electronics");
        result.First(x => x.CategoryId == 6).Category.Should().Be("Food");
    }

    [Fact]
    public async Task GetOffersAsync_ShouldHandleMissingCategoryNamesGracefully()
    {
        // Arrange
        var merchant = new Merchant { Id = 10 };
        var offers = new List<Offer> { new() { Id = 1, CategoryId = 99 } };

        _merchantRepoMock.Setup(r => r.GetMerchantByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(merchant);
        _offerRepoMock.Setup(r => r.GetByMerchantIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(offers);
        _categoryRepoMock.Setup(r => r.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Category>());

        // Act
        var result = await _sut.GetOffersAsync(1);

        // Assert
        result.First().Category.Should().BeNull();
    }

    [Fact]
    public async Task GetOffersAsync_ShouldQueryCategoryRepoWithDistinctIdsOnly()
    {
        // Arrange
        var userId = 1;
        var offers = new List<Offer>
        {
            new() { CategoryId = 5 },
            new() { CategoryId = 5 }
        };
        _merchantRepoMock.Setup(r => r.GetMerchantByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(new Merchant { Id = 10 });
        _offerRepoMock.Setup(r => r.GetByMerchantIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(offers);
        _categoryRepoMock.Setup(r => r.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Category>());

        // Act
        await _sut.GetOffersAsync(userId);

        // Assert
        _categoryRepoMock.Verify(r => r.GetByIdsAsync(It.Is<List<int>>(l => l.Count == 1), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllMerchantsAsync_ShouldReturnAllMappedEntities()
    {
        // Arrange
        var list = new List<Merchant> { new() { Id = 1 }, new() { Id = 2 } };
        _merchantRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list);

        // Act
        var result = await _sut.GetAllMerchantsAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetMerchantByIdAsync_ShouldReturnDto_WhenFound()
    {
        // Arrange
        _merchantRepoMock.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Merchant { Id = 5, Name = "Test" });

        // Act
        var result = await _sut.GetMerchantByIdAsync(5);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(5);
    }

    [Fact]
    public async Task GetMerchantByUserIdAsync_ShouldReturnMerchant_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var merchant = new Merchant { Id = 10, UserId = userId, Name = "Store A" };
        _merchantRepoMock.Setup(r => r.GetMerchantByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchant);

        // Act
        var result = await _sut.GetMerchantByUserIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Store A");
    }

    [Fact]
    public async Task GetMerchantByUserIdAsync_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _merchantRepoMock.Setup(r => r.GetMerchantByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Merchant?)null);

        // Act & Assert
        await _sut.Invoking(s => s.GetMerchantByUserIdAsync(99))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData(new int[] {5, 6, 7, 8}, 3)] // 3 category missing
    [InlineData(new int[] { 5, 99 }, 1)] // 99 category missing
    [InlineData(new int[] { 5, 6 }, 2)] // none of the categories missing
    [InlineData(new int[] { }, 0)]       // no offers
    public async Task GetOffersAsync_ShouldHandleVariousCategoryMappings(int[] categoryIds, int expectedMappedCount)
    {
        var merchant = new Merchant { Id = 1 };
        var offers = categoryIds.Select((id, i) => new Offer { Id = i + 1, CategoryId = id }).ToList();
        var categories = new List<Category> { new Category { Id = 5, Name = "test1" },
                                              new Category { Id = 6, Name = "test2" },
                                              new Category { Id = 7, Name = "test3" } };

        _merchantRepoMock.Setup(r => r.GetMerchantByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(merchant);
        _offerRepoMock.Setup(r => r.GetByMerchantIdAsync(merchant.Id, It.IsAny<CancellationToken>())).ReturnsAsync(offers);
        _categoryRepoMock.Setup(r => r.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>())).ReturnsAsync(categories);

        var result = await _sut.GetOffersAsync(1);

        result.Count(x => x.Category != null).Should().Be(expectedMappedCount);
    }
    #endregion

    #region Create Test
    [Fact]
    public async Task AddMerchantAsync_ShouldCallRepositoryAdd_WithMappedEntity()
    {
        // Arrange
        var dto = new CreateMerchantDto { Name = "New Store" };

        // Act
        await _sut.AddMerchantAsync(dto);

        // Assert
        _merchantRepoMock.Verify(r => r.AddAsync(It.Is<Merchant>(m => m.Name == dto.Name), It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

    #region Update Tests
    [Fact]
    public async Task UpdateMerchantAsync_ShouldThrowNotFound_WhenIdIsInvalid()
    {
        // Arrange
        _merchantRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Merchant?)null);

        // Act & Assert
        await _sut.Invoking(s => s.UpdateMerchantAsync(new UpdateMerchantDto { Id = 1 }))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateMerchantAsync_ShouldApplyChanges_WhenFound()
    {
        // Arrange
        var existing = new Merchant { Id = 1, Name = "Old Name" };
        var dto = new UpdateMerchantDto { Id = 1, Name = "New Name" };
        _merchantRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);

        // Act
        await _sut.UpdateMerchantAsync(dto);

        // Assert
        _merchantRepoMock.Verify(r => r.UpdateAsync(It.Is<Merchant>(m => m.Name == "New Name"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(1, "Updated Store")]
    [InlineData(99, "Nonexistent Store")]
    [InlineData(-1, "Nonexistent Store123")]
    public async Task UpdateMerchantAsync_ShouldHandleDifferentIds(int id, string newName)
    {
        if (id == 1)
        {
            var existing = new Merchant { Id = 1, Name = "Old Name" };
            _merchantRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);

            await _sut.UpdateMerchantAsync(new UpdateMerchantDto { Id = 1, Name = newName });

            existing.Name.Should().Be(newName);
            _merchantRepoMock.Verify(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
        }
        else
        {
            _merchantRepoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Merchant?)null);

            await _sut.Invoking(s => s.UpdateMerchantAsync(new UpdateMerchantDto { Id = id, Name = newName }))
                .Should().ThrowAsync<NotFoundException>();
        }
    }
    #endregion

    #region Delete Tests
    [Fact]
    public async Task DeleteMerchantAsync_ShouldThrowNotFound_WhenMerchantDoesNotExist()
    {
        // Arrange
        _merchantRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Merchant?)null);

        // Act & Assert
        await _sut.Invoking(s => s.DeleteMerchantAsync(1))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteMerchantAsync_ShouldCallDelete_WhenMerchantExists()
    {
        // Arrange
        var merchant = new Merchant { Id = 1 };
        _merchantRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchant);

        // Act
        await _sut.DeleteMerchantAsync(1);

        // Assert
        _merchantRepoMock.Verify(r => r.DeleteAsync(merchant, It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

}
