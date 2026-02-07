using FluentAssertions;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Application.Services.Implementations;
using Mapster;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace UnitTests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepoMock;
    private readonly Mock<IAuthorRepository> _authorRepoMock;
    private readonly BookService _service;

    public BookServiceTests()
    {
        TypeAdapterConfig<CreateBookDto, Book>
            .NewConfig()
            .Ignore(dest => dest.Author);

        _bookRepoMock = new Mock<IBookRepository>();
        _authorRepoMock = new Mock<IAuthorRepository>();
        _service = new BookService(_bookRepoMock.Object, _authorRepoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookExists_ReturnsDto()
    {
        var book = new Book { Id = 1, Title = "Clean Code" };

        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        var result = await _service.GetByIdAsync(1, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Title.Should().Be("Clean Code");
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookNotFound_ThrowsNotFound()
    {
        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetByIdAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_WhenBookWithSameTitleExists_ThrowsInvalidOperation()
    {
        var dto = new CreateBookDto
        {
            Title = "DDD",
            Author = "Eric Evans"
        };

        _authorRepoMock.Setup(r =>
                r.GetByNameAsync("Eric", "Evans", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Author { Id = 1 });

        _bookRepoMock.Setup(r =>
                r.GetByTitleAsync("DDD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book());

        _bookRepoMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbContextTransaction>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_WhenAuthorDoesNotExist_CreatesAuthorAndBook()
    {
        var dto = new CreateBookDto
        {
            Title = "Refactoring",
            Author = "Martin Fowler"
        };

        var transactionMock = new Mock<IDbContextTransaction>();

        _bookRepoMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);

        _authorRepoMock.Setup(r =>
                r.GetByNameAsync("Martin", "Fowler", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Author?)null);

        _bookRepoMock.Setup(r =>
                r.GetByTitleAsync(dto.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        _authorRepoMock.Setup(r =>
                r.AddAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _bookRepoMock.Setup(r =>
                r.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto, CancellationToken.None);

        result.Title.Should().Be("Refactoring");
        transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenBookNotFound_ThrowsNotFound()
    {
        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.DeleteAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_WhenBookExists_DeletesBook()
    {
        var book = new Book { Id = 1 };

        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        await _service.DeleteAsync(1, CancellationToken.None);

        _bookRepoMock.Verify(r =>
            r.DeleteAsync(book, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task IsAvailableAsync_WhenBookNotFound_ThrowsNotFound()
    {
        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.IsAvailableAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task IsAvailableAsync_WhenBookExists_ReturnsAvailability()
    {
        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book());

        _bookRepoMock.Setup(r => r.IsAvailableAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _service.IsAvailableAsync(1, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_WhenBookNotFound_ThrowsNotFound()
    {
        var dto = new UpdateBookDto
        {
            Title = "New Title",
            Author = "Someone Else"
        };

        _bookRepoMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbContextTransaction>());

        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.UpdateAsync(1, dto, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_WhenTitleBelongsToAnotherBook_ThrowsInvalidOperation()
    {
        var book = new Book { Id = 1 };
        var existingBook = new Book { Id = 2, Title = "Duplicate" };

        var dto = new UpdateBookDto
        {
            Title = "Duplicate",
            Author = "Test Author"
        };

        _bookRepoMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbContextTransaction>());

        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        _bookRepoMock.Setup(r => r.GetByTitleAsync("Duplicate", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBook);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(1, dto, CancellationToken.None));
    }
}
