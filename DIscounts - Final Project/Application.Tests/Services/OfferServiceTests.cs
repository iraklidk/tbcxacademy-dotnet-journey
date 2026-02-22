using Moq;
using Xunit;
using Domain.Entities;
using FluentAssertions;
using Domain.Constants;
using Application.Services;
using Application.Interfaces;
using Application.DTOs.Offer;
using Application.Interfaces.Repos;
using Discounts.Application.Exceptions;

namespace Application.Tests.Services;

public class OfferServiceTests
{
    private readonly OfferService _sut;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IOfferRepository> _offerRepoMock;
    private readonly Mock<IMerchantRepository> _merchantRepoMock;
    private readonly Mock<ICustomerRepository> _customerRepoMock;
    private readonly Mock<ICategoryRepository> _categoryRepoMock;
    private readonly Mock<IReservationRepository> _reservationRepoMock;
    private readonly Mock<IGlobalSettingsRepository> _settingsRepoMock;

    public OfferServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _offerRepoMock = new Mock<IOfferRepository>();
        _merchantRepoMock = new Mock<IMerchantRepository>();
        _customerRepoMock = new Mock<ICustomerRepository>();
        _categoryRepoMock = new Mock<ICategoryRepository>();
        _reservationRepoMock = new Mock<IReservationRepository>();
        _settingsRepoMock = new Mock<IGlobalSettingsRepository>();

        _sut = new OfferService(
            _uowMock.Object, _offerRepoMock.Object, _merchantRepoMock.Object,
            _customerRepoMock.Object, _categoryRepoMock.Object,
            _reservationRepoMock.Object, _settingsRepoMock.Object);
    }

    #region Create Tests
    [Fact]
    public async Task CreateOfferAsync_ShouldThrowDomainException_WhenDiscountedPriceIsHigher()
    {
        // Arrange
        var dto = new CreateOfferDto { OriginalPrice = 100, DiscountedPrice = 110 };

        // Act & Assert
        await _sut.Invoking(s => s.CreateOfferAsync(dto, default))
            .Should().ThrowAsync<DomainException>()
            .WithMessage("Discounted price must be lower than original price!");
    }

    [Fact]
    public async Task CreateOfferAsync_ShouldThrowDomainException_WhenDatesAreInvalid()
    {
        // Arrange
        var dto = new CreateOfferDto
        {
            OriginalPrice = 100,
            DiscountedPrice = 80,
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(1) // End before start
        };

        // Act & Assert
        await _sut.Invoking(s => s.CreateOfferAsync(dto, default))
            .Should().ThrowAsync<DomainException>()
            .WithMessage("End date must be after start date!");
    }

    [Fact]
    public async Task CreateOfferAsync_ShouldSucceed_WhenValid()
    {
        // Arrange
        var dto = new CreateOfferDto { UserId = 1, OriginalPrice = 100, DiscountedPrice = 50, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1) };
        _merchantRepoMock.Setup(r => r.GetMerchantByUserIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Merchant { Id = 10 });

        // Act
        var result = await _sut.CreateOfferAsync(dto, default);

        // Assert
        result.Should().NotBeNull();
        _offerRepoMock.Verify(r => r.AddAsync(It.Is<Offer>(o => o.MerchantId == 10), It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

    #region Update Tests
    [Fact]
    public async Task UpdateOfferAsync_ShouldThrowDomainException_WhenEditWindowIsExpired()
    {
        // Arrange
        var settings = new GlobalSettings { MerchantEditHours = 24 };
        var existingOffer = new Offer { Id = 1, Created = DateTime.UtcNow.AddHours(-25), StartDate = DateTime.UtcNow};
        var dto = new UpdateOfferDto { Id = 1, EndDate = DateTime.UtcNow.AddDays(5) };

        _offerRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingOffer);
        _settingsRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(settings);

        // Act & Assert
        await _sut.Invoking(s => s.UpdateOfferAsync(dto, default)).Should().ThrowAsync<DomainException>()
                  .WithMessage("*Offers cannot be edited after*");
    }

    [Fact]
    public async Task UpdateOfferAsync_ShouldThrowDomainException_WhenRemainingCouponsExceedCurrent()
    {
        // Arrange
        var existingOffer = new Offer { Id = 1, RemainingCoupons = 10, OriginalPrice = 100, Created = DateTime.UtcNow };
        var dto = new UpdateOfferDto { Id = 1, RemainingCoupons = 15, DiscountedPrice = 50, EndDate = DateTime.UtcNow.AddDays(5)};

        _offerRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingOffer);
        _settingsRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new GlobalSettings { MerchantEditHours = 24 });

        // Act & Assert
        await _sut.Invoking(s => s.UpdateOfferAsync(dto, default))
            .Should().ThrowAsync<DomainException>()
            .WithMessage("Remaining coupons cannot exceed latest remaining coupons!");
    }
    #endregion

    #region Get Tests
    [Fact]
    public async Task GetAllAsync_ShouldMapCategoryNamesCorrectly()
    {
        // Arrange
        var offers = new List<Offer>
        {
            new() { Id = 1, CategoryId = 5 },
            new() { Id = 2, CategoryId = 6 }
        };
        var categories = new List<Category>
        {
            new() { Id = 5, Name = "Food" },
            new() { Id = 6, Name = "Spa" }
        };

        _offerRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(offers);
        _categoryRepoMock.Setup(r => r.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>())).ReturnsAsync(categories);

        // Act
        var result = await _sut.GetAllWithCategoryNamesAsync().ConfigureAwait(true);

        // Assert
        result.First(x => x.Id == 1).Category.Should().Be("Food");
        result.First(x => x.Id == 2).Category.Should().Be("Spa");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFound_WhenMissing()
    {
        // Arrange
        _offerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Offer?)null);

        // Act & Assert
        await _sut.Invoking(s => s.GetByIdAsync(99, default)).Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedOffer()
    {
        // Arrange
        var offer = new Offer { Id = 1, CategoryId = 5 };
        var category = new Category { Id = 5, Name = "Food" };
        _offerRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
        _categoryRepoMock.Setup(r => r.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Category> { category });

        // Act
        var result = await _sut.GetByIdAsync(1, default);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.CategoryId.Should().Be(5);
    }
    #endregion

    #region Delete & Status Tests
    [Fact]
    public async Task DeleteOfferAsync_ShouldThrowNotFound_WhenMissing()
    {
        _offerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Offer?)null);

        await _sut.Invoking(s => s.DeleteOfferAsync(99, default))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateStatusAsync_ShouldCallUpdate_WhenValid()
    {
        // Arrange
        var offer = new Offer { Id = 1 };
        var dto = new UpdateOfferStatusDto { Id = 1, Status = OfferStatus.Approved };
        _offerRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(offer);

        // Act
        await _sut.UpdateStatusAsync(dto, default);

        // Assert
        _offerRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Offer>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

    #region Reservation & Coupon Tests
    [Fact]
    public async Task IsOfferReservedByUserAsync_ShouldThrowNotFound_WhenCustomerMissing()
    {
        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Customer?)null);

        await _sut.Invoking(s => s.IsOfferReservedByUserAsync(1, 1))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task IsOfferReservedByUserAsync_ShouldReturnTrue_WhenReservationExists()
    {
        // Arrange
        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { Id = 5 });
        _reservationRepoMock.Setup(r => r.ExistsActiveAsync(10, 5, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _sut.IsOfferReservedByUserAsync(10, 1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ChangeRemainingCouponsAsync_ShouldUpdate_WhenOfferExists()
    {
        // Arrange
        _offerRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Offer());

        // Act
        await _sut.ChangeRemainingCouponsAsync(1, 5);

        // Assert
        _offerRepoMock.Verify(r => r.ChangeRemainingCouponsAsync(1, 5, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByMerchantIdAsync_ShouldReturnMappedList()
    {
        // Arrange
        var offers = new List<Offer> { new() { Id = 1 }, new() { Id = 2 } };
        _offerRepoMock.Setup(r => r.GetByMerchantIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(offers);

        // Act
        var result = await _sut.GetByMerchantIdAsync(10);

        // Assert
        result.Should().HaveCount(2);
    }
    #endregion

}
