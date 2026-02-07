using FluentAssertions;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Application.Services.Implementations;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace UnitTests.Services;

public class BorrowRecordServiceTests
{
    private readonly Mock<IBorrowRecordRepository> _borrowRepoMock;
    private readonly Mock<IBookRepository> _bookRepoMock;
    private readonly Mock<IPatronRepository> _patronRepoMock;
    private readonly BorrowRecordService _service;

    public BorrowRecordServiceTests()
    {
        _borrowRepoMock = new Mock<IBorrowRecordRepository>();
        _bookRepoMock = new Mock<IBookRepository>();
        _patronRepoMock = new Mock<IPatronRepository>();

        _service = new BorrowRecordService(
            _borrowRepoMock.Object,
            _bookRepoMock.Object,
            _patronRepoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRecordExists_ReturnsDto()
    {
        var record = new BorrowRecord { Id = 1 };

        _borrowRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(record);

        var result = await _service.GetByIdAsync(1, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRecordNotFound_ThrowsNotFound()
    {
        _borrowRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((BorrowRecord?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetByIdAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_WhenDueDateBeforeBorrowDate_ThrowsInvalidOperation()
    {
        var dto = new CreateBorrowRecordDto
        {
            BorrowDate = DateTime.Today,
            DueDate = DateTime.Today,
            BookId = 1,
            PatronId = 1
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_WhenBookNotFound_ThrowsNotFound()
    {
        var dto = ValidCreateDto();

        _borrowRepoMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbContextTransaction>());

        _bookRepoMock.Setup(r => r.GetByIdAsync(dto.BookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.CreateAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_WhenBookUnavailable_ThrowsInvalidOperation()
    {
        var dto = ValidCreateDto();

        _borrowRepoMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbContextTransaction>());

        _bookRepoMock.Setup(r => r.GetByIdAsync(dto.BookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book { Quantity = 0 });

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_WhenPatronNotFound_ThrowsNotFound()
    {
        var dto = ValidCreateDto();

        _borrowRepoMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbContextTransaction>());

        _bookRepoMock.Setup(r => r.GetByIdAsync(dto.BookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book { Id = 1, Quantity = 1 });

        _patronRepoMock.Setup(r => r.GetByIdAsync(dto.PatronId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patron?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.CreateAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_WhenValid_CreatesRecordAndUpdatesBook()
    {
        var dto = ValidCreateDto();
        var transactionMock = new Mock<IDbContextTransaction>();
        var book = new Book { Id = 1, Quantity = 2 };

        _borrowRepoMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);

        _bookRepoMock.Setup(r => r.GetByIdAsync(dto.BookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        _patronRepoMock.Setup(r => r.GetByIdAsync(dto.PatronId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patron { Id = 1 });

        _bookRepoMock.Setup(r => r.UpdateAsync(book, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _borrowRepoMock.Setup(r => r.AddAsync(It.IsAny<BorrowRecord>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto, CancellationToken.None);

        result.Status.Should().Be(BorrowStatus.Borrowed);
        book.Quantity.Should().Be(1);
        transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReturnAsync_WhenRecordNotFound_ThrowsNotFound()
    {
        _borrowRepoMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbContextTransaction>());

        _borrowRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((BorrowRecord?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.ReturnAsync(1, new ReturnBorrowRecordDto(), CancellationToken.None));
    }

    [Fact]
    public async Task ReturnAsync_WhenAlreadyReturned_ThrowsInvalidOperation()
    {
        var record = new BorrowRecord { Id = 1, ReturnDate = DateTime.Today };

        _borrowRepoMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbContextTransaction>());

        _borrowRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(record);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.ReturnAsync(1, new ReturnBorrowRecordDto(), CancellationToken.None));
    }

    [Fact]
    public async Task ReturnAsync_WhenValid_UpdatesRecordAndBook()
    {
        var record = new BorrowRecord
        {
            Id = 1,
            BookId = 1,
            DueDate = DateTime.Today.AddDays(-1)
        };

        var book = new Book { Id = 1, Quantity = 0 };

        var transactionMock = new Mock<IDbContextTransaction>();

        _borrowRepoMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);

        _borrowRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(record);

        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        _borrowRepoMock.Setup(r => r.UpdateAsync(record, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _bookRepoMock.Setup(r => r.UpdateAsync(book, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var dto = new ReturnBorrowRecordDto
        {
            ReturnDate = DateTime.Today
        };

        await _service.ReturnAsync(1, dto, CancellationToken.None);

        record.Status.Should().Be(BorrowStatus.Overdue);
        book.Quantity.Should().Be(1);
        transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static CreateBorrowRecordDto ValidCreateDto() =>
        new()
        {
            BookId = 1,
            PatronId = 1,
            BorrowDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(7)
        };
}
