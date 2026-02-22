using Moq;
using Xunit;
using Domain.Entities;
using Domain.Constants;
using FluentAssertions;
using Application.Services;
using Application.Interfaces.Repos;

namespace Application.Tests.Services;

public class CleanupServiceTests
{
    private readonly CleanupService _sut;
    private readonly Mock<IOfferRepository> _offerRepoMock;
    private readonly Mock<IReservationRepository> _resRepoMock;

    public CleanupServiceTests()
    {
        _offerRepoMock = new Mock<IOfferRepository>();
        _resRepoMock = new Mock<IReservationRepository>();
        _sut = new CleanupService(_offerRepoMock.Object, _resRepoMock.Object);
    }

    #region CleanupAsync Tests
    [Fact]
    public async Task CleanupAsync_ShouldUpdateExpiredOffersStatus()
    {
        // Arrange
        var expiredOffers = new List<Offer>
        {
            new() { Id = 1, Status = OfferStatus.Approved },
            new() { Id = 2, Status = OfferStatus.Pending }
        };
        _offerRepoMock.Setup(r => r.GetExpiredOffersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expiredOffers);
        _resRepoMock.Setup(r => r.GetExpiredReservationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Reservation>());

        // Act
        await _sut.CleanupAsync(default);

        // Assert
        expiredOffers.Should().AllSatisfy(o => o.Status.Should().Be(OfferStatus.Expired));
        _offerRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CleanupAsync_ShouldDeactivateReservationsAndReturnCoupons()
    {
        // Arrange
        var expiredReservations = new List<Reservation>
        {
            new() { Id = 101, OfferId = 1, IsActive = true },
            new() { Id = 102, OfferId = 1, IsActive = true },
            new() { Id = 103, OfferId = 2, IsActive = true } 
        };

        var offersToUpdate = new List<Offer>
        {
            new() { Id = 1, RemainingCoupons = 5 },
            new() { Id = 2, RemainingCoupons = 10 }
        };

        _offerRepoMock.Setup(r => r.GetExpiredOffersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Offer>());
        _resRepoMock.Setup(r => r.GetExpiredReservationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expiredReservations);
        _offerRepoMock.Setup(r => r.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(offersToUpdate);

        // Act
        await _sut.CleanupAsync(default);

        // Assert
        expiredReservations.Should().AllSatisfy(r => r.IsActive.Should().BeFalse());
        offersToUpdate.First(o => o.Id == 1).RemainingCoupons.Should().Be(7); // 5 + 2
        offersToUpdate.First(o => o.Id == 2).RemainingCoupons.Should().Be(11); // 10 + 1
        _offerRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CleanupAsync_ShouldHandleEmptyExpiredDataGracefully()
    {
        // Arrange
        _offerRepoMock.Setup(r => r.GetExpiredOffersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Offer>());
        _resRepoMock.Setup(r => r.GetExpiredReservationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Reservation>());
        _offerRepoMock.Setup(r => r.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Offer>());

        // Act
        await _sut.CleanupAsync(default);

        // Assert
        _offerRepoMock.Verify(r => r.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()), Times.Never);
        _offerRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

}
