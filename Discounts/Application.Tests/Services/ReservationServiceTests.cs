using Moq;
using Xunit;
using Domain.Entities;
using FluentAssertions;
using Application.Services;
using Application.Interfaces;
using Application.DTOs.Reservation;
using Application.Interfaces.Repos;
using Discounts.Application.Exceptions;

namespace Application.Tests.Services;

public class ReservationServiceTests
{
    private readonly ReservationService _sut;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IOfferRepository> _offerRepoMock;
    private readonly Mock<ICustomerRepository> _customerRepoMock;
    private readonly Mock<IReservationRepository> _reservationRepoMock;
    private readonly Mock<IGlobalSettingsRepository> _settingsRepoMock;

    public ReservationServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _offerRepoMock = new Mock<IOfferRepository>();
        _customerRepoMock = new Mock<ICustomerRepository>();
        _reservationRepoMock = new Mock<IReservationRepository>();
        _settingsRepoMock = new Mock<IGlobalSettingsRepository>();

        _sut = new ReservationService(
            _uowMock.Object,
            _offerRepoMock.Object,
            _customerRepoMock.Object,
            _reservationRepoMock.Object,
            _settingsRepoMock.Object);
    }

    #region Create Tests
    [Fact]
    public async Task CreateReservationAsync_ShouldSucceed_WhenAllConditionsAreMet()
    {
        // Arrange
        var dto = new CreateReservationDto { UserId = 1, OfferId = 10, ExpiresAt = DateTime.UtcNow.AddDays(1) };
        var customer = new Customer { Id = 5 };
        var offer = new Offer { Id = 10, RemainingCoupons = 5 };
        var settings = new GlobalSettings { ReservationPrice = 10, BookingDurationMinutes = 30 };

        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(dto.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _offerRepoMock.Setup(r => r.GetByIdAsync(dto.OfferId, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
        _settingsRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(settings);

        // Act
        var result = await _sut.CreateReservationAsync(dto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        customer.Balance.Should().Be(4990); // Entity's default balance is 5000, minus reservation price
        _offerRepoMock.Verify(r => r.ChangeRemainingCouponsAsync(dto.OfferId, -1, It.IsAny<CancellationToken>()), Times.Once);
        _reservationRepoMock.Verify(r => r.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldThrowDomainException_WhenBalanceIsInsufficient()
    {
        // Arrange
        var dto = new CreateReservationDto { UserId = 1, OfferId = 10 };
        var customer = new Customer { Balance = 5 }; // less than price
        var settings = new GlobalSettings { ReservationPrice = 10 };

        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(dto.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _offerRepoMock.Setup(r => r.GetByIdAsync(dto.OfferId, It.IsAny<CancellationToken>())).ReturnsAsync(new Offer());
        _settingsRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(settings);

        // Act
        var act = () => _sut.CreateReservationAsync(dto, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>().WithMessage("Customer does not have enough balance*");
        _offerRepoMock.Verify(r => r.ChangeRemainingCouponsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _reservationRepoMock.Verify(r => r.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()), Times.Never);
        _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldThrowDomainException_WhenNoCouponsLeft()
    {
        // Arrange
        var dto = new CreateReservationDto { UserId = 1, OfferId = 10 };
        var customer = new Customer { Balance = 100 };
        var offer = new Offer { RemainingCoupons = 0 }; // Out of stock
        var settings = new GlobalSettings { ReservationPrice = 10 };

        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(dto.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _offerRepoMock.Setup(r => r.GetByIdAsync(dto.OfferId, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
        _settingsRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(settings);

        // Act & Assert
        await _sut.Invoking(s => s.CreateReservationAsync(dto, CancellationToken.None))
            .Should().ThrowAsync<DomainException>().WithMessage("No coupons available*");
        _offerRepoMock.Verify(r => r.ChangeRemainingCouponsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _reservationRepoMock.Verify(r => r.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()), Times.Never);
        _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldThrowDomainException_WhenExpirationIsInPast()
    {
        // Arrange
        var dto = new CreateReservationDto { UserId = 1, OfferId = 10, ExpiresAt = DateTime.UtcNow.AddHours(-1) };
        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { Balance = 100 });
        _offerRepoMock.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(new Offer { RemainingCoupons = 1 });
        _settingsRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new GlobalSettings { ReservationPrice = 10 });

        // Act & Assert
        await _sut.Invoking(s => s.CreateReservationAsync(dto, CancellationToken.None))
            .Should().ThrowAsync<DomainException>().WithMessage("Expiration date must be in the future!");
        _offerRepoMock.Verify(r => r.ChangeRemainingCouponsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _reservationRepoMock.Verify(r => r.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()), Times.Never);
        _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(5000, 10, 5, true)]   // enough balance, coupons available
    [InlineData(5, 10, 5, false)]     // insufficient balance
    [InlineData(100, 10, 0, false)]   // no coupons
    public async Task CreateReservationAsync_ShouldValidateBalanceAndStock(int balance, int price, int coupons, bool suceed)
    {
        // Arrange
        var dto = new CreateReservationDto { UserId = 1, OfferId = 10, ExpiresAt = DateTime.UtcNow.AddHours(1) };
        var customer = new Customer { Id = 1, Balance = balance };
        var offer = new Offer { Id = 10, RemainingCoupons = coupons };
        var settings = new GlobalSettings { ReservationPrice = price };

        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(dto.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _offerRepoMock.Setup(r => r.GetByIdAsync(dto.OfferId, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
        _settingsRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(settings);

        // Act & Assert
        if (suceed)
        {
            var result = await _sut.CreateReservationAsync(dto, CancellationToken.None);
            customer.Balance.Should().Be(balance - price);
            _reservationRepoMock.Verify(r => r.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        else
        {
            await _sut.Invoking(s => s.CreateReservationAsync(dto, CancellationToken.None)).Should().ThrowAsync<DomainException>();
            _reservationRepoMock.Verify(r => r.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()), Times.Never);
            _customerRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Theory]
    [InlineData(-1, false)]  // expired
    [InlineData(0, false)]    // expires now
    [InlineData(5, true)]    // future expiration
    public async Task CreateReservationAsync_ShouldValidateExpiration(int offsetHours, bool shouldSucceed)
    {
        // Arrange
        var dto = new CreateReservationDto { UserId = 1, OfferId = 10, ExpiresAt = DateTime.UtcNow.AddHours(offsetHours) };
        var customer = new Customer { Balance = 100 };
        var offer = new Offer { Id = 10, RemainingCoupons = 5 };
        var settings = new GlobalSettings { ReservationPrice = 10 };

        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(dto.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _offerRepoMock.Setup(r => r.GetByIdAsync(dto.OfferId, It.IsAny<CancellationToken>())).ReturnsAsync(offer);
        _settingsRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(settings);

        // Act & Assert
        if (shouldSucceed)
        {
            var result = await _sut.CreateReservationAsync(dto, CancellationToken.None);
            result.Should().NotBeNull();
        }
        else
        {
            await _sut.Invoking(s => s.CreateReservationAsync(dto, CancellationToken.None))
                .Should().ThrowAsync<DomainException>().WithMessage("*future*");
        }
    }
    #endregion

    #region Get Tests
    [Fact]
    public async Task GetByCustomerAsync_ShouldThrowNotFound_WhenCustomerMissing()
    {
        _customerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Customer?)null);

        await _sut.Invoking(s => s.GetByCustomerAsync(1, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetByOfferAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var offerId = 1;
        _offerRepoMock.Setup(r => r.GetByIdAsync(offerId, It.IsAny<CancellationToken>())).ReturnsAsync(new Offer { Id = offerId });
        _reservationRepoMock.Setup(r => r.GetByOfferAsync(offerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Reservation> { new() { Id = 101 } });

        // Act
        var result = await _sut.GetByOfferAsync(offerId, CancellationToken.None);

        // Assert
        result.Should().ContainSingle().Which.Id.Should().Be(101);
    }

    [Fact]
    public async Task GetByUserAsync_ShouldReturnEmptyList_WhenNoReservationsFound()
    {
        _reservationRepoMock.Setup(r => r.GetByUserAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Reservation>());

        var result = await _sut.GetByUserAsync(1);

        result.Should().BeEmpty();
    }
    #endregion

    #region Update & Delete Tests
    [Fact]
    public async Task UpdateReservationAsync_ShouldThrowDomainException_WhenDatesAreInvalid()
    {
        // Arrange
        var existing = new Reservation { Id = 1, ReservedAt = DateTime.UtcNow };
        var dto = new UpdateReservationDto { Id = 1, ExpiresAt = existing.ReservedAt.AddMinutes(-5) }; // Invalid: expires before reserved

        _reservationRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);

        // Act & Assert
        await _sut.Invoking(s => s.UpdateReservationAsync(dto, CancellationToken.None))
            .Should().ThrowAsync<DomainException>().WithMessage("Reservation expiration must be after reserved date!");
    }

    [Fact]
    public async Task UpdateReservationAsync_ShouldSucceed_WhenValid()
    {
        // Arrange
        var existing = new Reservation { Id = 1, ReservedAt = DateTime.UtcNow.AddDays(-1) };
        var dto = new UpdateReservationDto { Id = 1, ExpiresAt = DateTime.UtcNow.AddDays(1) };
        _reservationRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);

        // Act
        await _sut.UpdateReservationAsync(dto, CancellationToken.None);

        // Assert
        _reservationRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteReservationAsync_ShouldThrowNotFound_WhenMissing()
    {
        // Arrange
        _reservationRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Reservation?)null);

        // Act & Assert
        await _sut.Invoking(s => s.DeleteReservationAsync(1, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData(-5, false)]  // expires before reserved date
    [InlineData(10, true)]   // valid expiration
    [InlineData(0, false)]
    public async Task UpdateReservationAsync_ShouldValidateExpirationOffset(int offsetMinutes, bool shouldSucceed)
    {
        // Arrange
        var existing = new Reservation { Id = 1, ReservedAt = DateTime.UtcNow };
        var dto = new UpdateReservationDto { Id = 1, ExpiresAt = existing.ReservedAt.AddMinutes(offsetMinutes) };

        _reservationRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);

        // Act & Assert
        if (shouldSucceed)
        {
            await _sut.UpdateReservationAsync(dto, CancellationToken.None);
            _reservationRepoMock.Verify(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
        }
        else
        {
            await _sut.Invoking(s => s.UpdateReservationAsync(dto, CancellationToken.None))
                .Should().ThrowAsync<DomainException>();
            _reservationRepoMock.Verify(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Never);
        }
    }
    #endregion

    #region Transaction & Mapping Verification
    [Fact]
    public async Task CreateReservationAsync_ShouldStartTransaction_AtTheBeginning()
    {
        // Arrange
        var dto = new CreateReservationDto { UserId = 1 };
        _customerRepoMock.Setup(r => r.GetCustomerByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Customer?)null);

        // Act
        try { await _sut.CreateReservationAsync(dto, CancellationToken.None); } catch { }

        // Assert
        _uowMock.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExistsActiveAsync_ShouldCallRepositoryMethod()
    {
        _reservationRepoMock.Setup(r => r.ExistsActiveAsync(1, 1, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _sut.ExistsActiveAsync(1, 1);

        result.Should().BeTrue();
    }
    #endregion

}
