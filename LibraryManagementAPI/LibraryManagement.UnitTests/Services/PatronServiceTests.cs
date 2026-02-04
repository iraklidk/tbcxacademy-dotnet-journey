using FluentAssertions;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Application.Services.Implementations;
using Moq;
using Xunit;

public class PatronServiceTests
{
    private readonly Mock<IPatronRepository> _repoMock;
    private readonly PatronService _service;

    public PatronServiceTests()
    {
        _repoMock = new Mock<IPatronRepository>();
        _service = new PatronService(_repoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPatronExists_ReturnsDto()
    {
        var patron = new Patron { Id = 1, FirstName = "John", LastName = "Doe" };
        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(patron);

        var result = await _service.GetByIdAsync(1, CancellationToken.None);

        result.Should().NotBeNull();
        result!.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task GetByIdAsync_WhenPatronDoesNotExist_ThrowsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Patron?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetByIdAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_WhenPatronAlreadyExists_ThrowsInvalidOperation()
    {
        var dto = new CreatePatronDto { FirstName = "Jane", LastName = "Doe" };

        _repoMock.Setup(r =>
                r.GetByNameAsync("Jane", "Doe", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patron());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_WhenValid_AddsPatron()
    {
        var dto = new CreatePatronDto { FirstName = "Jane", LastName = "Doe" };

        _repoMock.Setup(r =>
                r.GetByNameAsync("Jane", "Doe", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patron?)null);

        _repoMock.Setup(r =>
                r.AddAsync(It.IsAny<Patron>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto, CancellationToken.None);

        result.FirstName.Should().Be("Jane");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Patron>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenPatronNotFound_ThrowsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Patron?)null);

        var dto = new UpdatePatronDto { FirstName = "New", LastName = "Name" };

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.UpdateAsync(1, dto, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_WhenNameBelongsToAnotherPatron_ThrowsInvalidOperation()
    {
        var existingPatron = new Patron { Id = 2, FirstName = "Jane", LastName = "Doe" };
        var currentPatron = new Patron { Id = 1, FirstName = "Old", LastName = "Name" };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(currentPatron);

        _repoMock.Setup(r => r.GetByNameAsync("Jane", "Doe", It.IsAny<CancellationToken>()))
                 .ReturnsAsync(existingPatron);

        var dto = new UpdatePatronDto { FirstName = "Jane", LastName = "Doe" };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(1, dto, CancellationToken.None));
    }


    [Fact]
    public async Task DeleteAsync_WhenPatronExists_Deletes()
    {
        var patron = new Patron { Id = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(patron);

        await _service.DeleteAsync(1, CancellationToken.None);

        _repoMock.Verify(r => r.DeleteAsync(patron, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBorrowedBooksAsync_WhenPatronNotFound_ThrowsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Patron?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetBorrowedBooksAsync(1, CancellationToken.None));
    }
}
