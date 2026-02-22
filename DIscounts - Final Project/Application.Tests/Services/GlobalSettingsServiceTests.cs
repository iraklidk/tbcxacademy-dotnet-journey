using Moq;
using Xunit;
using Domain.Entities;
using FluentAssertions;
using Application.Services;
using Application.Interfaces.Repos;
using Application.DTOs.GlobalSettings;
using Discounts.Application.Exceptions;

namespace Application.Tests.Services;

public class GlobalSettingsServiceTests
{
    private readonly GlobalSettingsService _sut;
    private readonly Mock<IGlobalSettingsRepository> _repositoryMock;

    public GlobalSettingsServiceTests()
    {
        _repositoryMock = new Mock<IGlobalSettingsRepository>();
        _sut = new GlobalSettingsService(_repositoryMock.Object);
    }

    #region Get Tests
    [Fact]
    public async Task GetSettingsAsync_ShouldReturnSettingsWithCorrectValues()
    {
        // Arrange
        var entity = new GlobalSettings
        {
            Id = 1,
            ReservationPrice = 50,
            MerchantEditHours = 48,
            BookingDurationMinutes = 60
        };
        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        // Act
        var result = await _sut.GetSettingsAsync(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ReservationPrice.Should().Be(50);
        result.MerchantEditHours.Should().Be(48);
        result.BookingDurationMinutes.Should().Be(60);
    }

    [Fact]
    public async Task GetSettingsAsync_ShouldCallGetByIdWithStaticIdOne()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GlobalSettings { Id = 1 });

        // Act
        await _sut.GetSettingsAsync(CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

    #region Update Tests
    [Fact]
    public async Task UpdateSettingsAsync_ShouldUpdateAllEntityFieldsCorrectly()
    {
        // Arrange
        var existingEntity = new GlobalSettings
        {
            Id = 1,
            ReservationPrice = 10,
            MerchantEditHours = 24,
            BookingDurationMinutes = 30
        };

        var dto = new UpdateGlobalSettingsDto
        {
            ReservationPrice = 25,
            MerchantEditHours = 12,
            BookingDurationMinutes = 15
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEntity);

        // Act
        await _sut.UpdateSettingsAsync(dto, CancellationToken.None);

        // Assert
        existingEntity.ReservationPrice.Should().Be(25);
        existingEntity.MerchantEditHours.Should().Be(12);
        existingEntity.BookingDurationMinutes.Should().Be(15);
        _repositoryMock.Verify(r => r.UpdateAsync(existingEntity, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSettingsAsync_ShouldNotChangeId_WhenUpdating()
    {
        // Arrange
        var entity = new GlobalSettings { Id = 1 };
        var dto = new UpdateGlobalSettingsDto { ReservationPrice = 99 };
        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        // Act
        await _sut.UpdateSettingsAsync(dto, CancellationToken.None);

        // Assert
        entity.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateSettingsAsync_ShouldThrowException_WhenSettingsRecordMissing()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GlobalSettings?)null);

        // Act
        var act = () => _sut.UpdateSettingsAsync(new UpdateGlobalSettingsDto(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
    #endregion

    #region Edge Cases & Mapping
    [Fact]
    public async Task GetSettingsAsync_ShouldHandleDefaultEntityValues()
    {
        // Arrange
        var defaultEntity = new GlobalSettings();
        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(defaultEntity);

        // Act
        var result = await _sut.GetSettingsAsync(CancellationToken.None);

        // Assert
        result.ReservationPrice.Should().Be(10);
        result.MerchantEditHours.Should().Be(24);
        result.BookingDurationMinutes.Should().Be(30);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1000)]
    public async Task UpdateSettingsAsync_ShouldHandleVaryingReservationPrices(int price)
    {
        // Arrange
        var entity = new GlobalSettings { Id = 1 };
        var dto = new UpdateGlobalSettingsDto { ReservationPrice = price };
        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        // Act
        await _sut.UpdateSettingsAsync(dto, CancellationToken.None);

        // Assert
        entity.ReservationPrice.Should().Be(price);
    }

    [Fact]
    public async Task UpdateSettingsAsync_ShouldBeCalledExactlyOnce()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GlobalSettings());

        // Act
        await _sut.UpdateSettingsAsync(new UpdateGlobalSettingsDto(), CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<GlobalSettings>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    #endregion

}
