using Moq;
using Xunit;
using Domain.Entities;
using FluentAssertions;
using Application.Services;
using Application.Interfaces;
using Application.DTOs.Coupon;
using Application.Interfaces.Repos;
using Discounts.Application.Exceptions;

namespace Application.Tests.Services;

public class CouponServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IOfferRepository> _offerRepoMock;
    private readonly Mock<ICouponRepository> _couponRepoMock;
    private readonly Mock<IMerchantRepository> _merchantRepoMock;
    private readonly Mock<ICustomerRepository> _customerRepoMock;
    private readonly CouponService _sut;

    public CouponServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _offerRepoMock = new Mock<IOfferRepository>();
        _couponRepoMock = new Mock<ICouponRepository>();
        _merchantRepoMock = new Mock<IMerchantRepository>();
        _customerRepoMock = new Mock<ICustomerRepository>();

        _sut = new CouponService(
            _uowMock.Object,
            _offerRepoMock.Object,
            _couponRepoMock.Object,
            _merchantRepoMock.Object,
            _customerRepoMock.Object);
    }

    #region Create Tests
    [Fact]
    public async Task CreateCouponAsync_ShouldTransferBalanceAndReduceCoupons_WhenSuccessful()
    {
        // Arrange
        var dto = new CreateCouponDto { UserId = 1, OfferId = 10 };
        var customer = new Customer { Id = 5, Firstname = "John", Lastname = "Doe", Balance = 100 };
        var merchant = new Merchant { Id = 20, Balance = 0 };
        var offer = new Offer { Id = 10, MerchantId = 20, DiscountedPrice = 30, RemainingCoupons = 5, EndDate = DateTime.UtcNow.AddDays(1) };

        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _offerRepoMock.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
        _merchantRepoMock.Setup(r => r.GetByIdAsync(20, It.IsAny<CancellationToken>())).ReturnsAsync(merchant);

        // Act
        var result = await _sut.CreateCouponAsync(dto, CancellationToken.None);

        // Assert
        customer.Balance.Should().Be(70); // 100 - 30
        merchant.Balance.Should().Be(30); // 0 + 30
        offer.RemainingCoupons.Should().Be(4); // 5 - 1

        _couponRepoMock.Verify(r => r.AddAsync(It.Is<Coupon>(c => c.CustomerId == 5 && c.OfferId == 10), It.IsAny<CancellationToken>()), Times.Once);
        _offerRepoMock.Verify(r => r.UpdateAsync(offer, It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        result.CustomerName.Should().Be("John Doe");
        result.Code.Should().StartWith("C-1-20-");
    }

    [Fact]
    public async Task CreateCouponAsync_ShouldThrowDomainException_WhenBalanceIsTooLow()
    {
        // Arrange
        var customer = new Customer { Balance = 10 }; // Low balance
        var offer = new Offer { DiscountedPrice = 50 };

        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _offerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(offer);

        // Act & Assert
        await _sut.Invoking(s => s.CreateCouponAsync(new CreateCouponDto(), default))
            .Should().ThrowAsync<DomainException>().WithMessage("*not have enough balance*");

        _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(100, 30, true)]  // enough balance
    [InlineData(50, 50, true)]   // exact balance
    [InlineData(20, 30, false)]  // insufficient balance
    public async Task CreateCouponAsync_ShouldValidateBalance(int customerBalance, int offerPrice, bool shouldSucceed)
    {
        // Arrange
        var dto = new CreateCouponDto { UserId = 1, OfferId = 10 };
        var customer = new Customer { Id = 1, Balance = customerBalance };
        var merchant = new Merchant { Id = 1, Balance = 0 };
        var offer = new Offer { Id = 10, MerchantId = 1, DiscountedPrice = offerPrice, RemainingCoupons = 5, EndDate = DateTime.UtcNow.AddDays(1) };

        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _offerRepoMock.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
        _merchantRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(merchant);

        // Act & Assert
        if (shouldSucceed)
        {
            var result = await _sut.CreateCouponAsync(dto, CancellationToken.None);
            customer.Balance.Should().Be(customerBalance - offerPrice);
            merchant.Balance.Should().Be(offerPrice);
        }
        else
        {
            await _sut.Invoking(s => s.CreateCouponAsync(dto, CancellationToken.None))
                .Should().ThrowAsync<DomainException>();
        }
    }

    [Theory]
    [InlineData(1, 20)]
    [InlineData(5, 99)]
    [InlineData(10, 50)]
    public async Task CreateCouponAsync_ShouldGenerateCorrectCode(int customerId, int merchantId)
    {
        // Arrange
        var dto = new CreateCouponDto { UserId = customerId, OfferId = 10 };
        var customer = new Customer { Id = customerId, Balance = 100 };
        var merchant = new Merchant { Id = merchantId, Balance = 0 };
        var offer = new Offer { Id = 10, MerchantId = merchantId, DiscountedPrice = 30, RemainingCoupons = 5, EndDate = DateTime.UtcNow.AddDays(1) };

        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _offerRepoMock.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
        _merchantRepoMock.Setup(r => r.GetByIdAsync(merchantId, It.IsAny<CancellationToken>())).ReturnsAsync(merchant);

        // Act
        var result = await _sut.CreateCouponAsync(dto, CancellationToken.None);

        // Assert
        result.Code.Should().StartWith($"C-{customerId}-{merchantId}-");
    }
    #endregion

    #region Get Tests
    [Fact]
    public async Task GetCouponsByMerchantAsync_ShouldReturnCoupons_WhenMerchantExists()
    {
        // Arrange
        var merchant = new Merchant { Id = 88 };
        var coupons = new List<Coupon> { new() { Id = 1 }, new() { Id = 2 } };

        _merchantRepoMock.Setup(r => r.GetMerchantByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(merchant);
        _couponRepoMock.Setup(r => r.GetCouponsByMerchantIdAsync(88, It.IsAny<CancellationToken>())).ReturnsAsync(coupons);

        // Act
        var result = await _sut.GetCouponsByMerchantAsync(1);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByOfferAsync_ShouldThrowNotFound_WhenOfferDoesNotExist()
    {
        _offerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Offer?)null);

        await _sut.Invoking(s => s.GetByOfferAsync(1))
            .Should().ThrowAsync<NotFoundException>();
    }
    #endregion

    #region Delete Tests
    [Fact]
    public async Task DeleteCouponAsync_ShouldCallDelete_WhenExists()
    {
        // Arrange
        var coupon = new Coupon { Id = 1 };
        _couponRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(coupon);

        // Act
        await _sut.DeleteCouponAsync(1);

        // Assert
        _couponRepoMock.Verify(r => r.DeleteAsync(coupon, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCouponAsync_ShouldThrowNotFoundException_WhenCouponIsNull()
    {
        // Arrange
        _couponRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Coupon?)null);

        // Act & Assert
        await _sut.Invoking(s => s.DeleteCouponAsync(1))
            .Should().ThrowAsync<NotFoundException>().WithMessage("*Coupon with id 1 not found*");
    }
    #endregion

    #region Update Tests
    [Fact]
    public async Task UpdateCouponAsync_ShouldUpdateEntityFields_ViaMapster()
    {
        // Arrange
        var existing = new Coupon { Id = 1, ExpirationDate = DateTime.UtcNow };
        var dto = new UpdateCouponDto { Id = 1, ExpirationDate = DateTime.Today.AddDays(5) };
        _couponRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);

        // Act
        await _sut.UpdateCouponAsync(dto);

        // Assert
        existing.ExpirationDate.Should().Be(DateTime.Today.AddDays(5));
        _couponRepoMock.Verify(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCouponAsync_ShouldThrowNotFoundException_WhenCouponIsNull()
    {
        // Arrange
        var updated = new UpdateCouponDto { Id = 1, ExpirationDate = DateTime.Today.AddDays(5) };
        _couponRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Coupon?)null);

        // Act & Assert
        await _sut.Invoking(s => s.UpdateCouponAsync(updated))
            .Should().ThrowAsync<NotFoundException>().WithMessage("*Coupon with id 1 not found*");
    }
    #endregion

    #region Transaction Test
    [Fact]
    public async Task CreateCouponAsync_ShouldStartTransactionImmediately()
    {
        // Arrange
        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Fail early"));

        // Act
        try { await _sut.CreateCouponAsync(new CreateCouponDto()); } catch { }

        // Assert
        _uowMock.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

}
