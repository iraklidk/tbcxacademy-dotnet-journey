using FluentAssertions;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Application.Services.Implementations;
using Moq;
using Xunit;

public class AuthorServiceTests
{
    private readonly Mock<IAuthorRepository> _repoMock;
    private readonly AuthorService _service;

    public AuthorServiceTests()
    {
        _repoMock = new Mock<IAuthorRepository>();
        _service = new AuthorService(_repoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenAuthorExists_ReturnsDto()
    {
        var author = new Author { Id = 1, FirstName = "George", LastName = "Orwell" };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        var result = await _service.GetByIdAsync(1, CancellationToken.None);

        result.Should().NotBeNull();
        result!.FirstName.Should().Be("George");
    }

    [Fact]
    public async Task GetByIdAsync_WhenAuthorNotFound_ThrowsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Author?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetByIdAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task GetBooksByAuthorIdAsync_WhenAuthorNotFound_ThrowsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Author?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetBooksByAuthorIdAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task GetBooksByAuthorIdAsync_WhenAuthorExists_ReturnsBooks()
    {
        var author = new Author { Id = 1 };
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "1984" },
            new Book { Id = 2, Title = "Animal Farm" }
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        _repoMock.Setup(r => r.GetBooksByAuthorIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        var result = await _service.GetBooksByAuthorIdAsync(1, CancellationToken.None);

        result.Should().HaveCount(2);
        result.First().Title.Should().Be("1984");
    }

    [Fact]
    public async Task CreateAsync_WhenAuthorAlreadyExists_ThrowsInvalidOperation()
    {
        var dto = new CreateAuthorDto
        {
            FirstName = "George",
            LastName = "Orwell"
        };

        _repoMock.Setup(r =>
                r.GetByNameAsync("George", "Orwell", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Author());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_WhenValid_AddsAuthor()
    {
        var dto = new CreateAuthorDto
        {
            FirstName = "Aldous",
            LastName = "Huxley"
        };

        _repoMock.Setup(r =>
                r.GetByNameAsync("Aldous", "Huxley", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Author?)null);

        _repoMock.Setup(r =>
                r.AddAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto, CancellationToken.None);

        result.FirstName.Should().Be("Aldous");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenAuthorNotFound_ThrowsNotFound()
    {
        var dto = new UpdateAuthorDto
        {
            FirstName = "New",
            LastName = "Name"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Author?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.UpdateAsync(1, dto, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_WhenNameBelongsToAnotherAuthor_ThrowsInvalidOperation()
    {
        var currentAuthor = new Author { Id = 1 };
        var existingAuthor = new Author { Id = 2, FirstName = "Jane", LastName = "Austen" };

        var dto = new UpdateAuthorDto
        {
            FirstName = "Jane",
            LastName = "Austen"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentAuthor);

        _repoMock.Setup(r =>
                r.GetByNameAsync("Jane", "Austen", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAuthor);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(1, dto, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_WhenValid_UpdatesAuthor()
    {
        var author = new Author { Id = 1, FirstName = "Old", LastName = "Name" };
        var dto = new UpdateAuthorDto
        {
            FirstName = "New",
            LastName = "Name"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        _repoMock.Setup(r =>
                r.GetByNameAsync("New", "Name", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Author?)null);

        _repoMock.Setup(r =>
                r.UpdateAsync(author, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _service.UpdateAsync(1, dto, CancellationToken.None);

        author.FirstName.Should().Be("New");
        _repoMock.Verify(r => r.UpdateAsync(author, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenAuthorNotFound_ThrowsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Author?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.DeleteAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_WhenAuthorExists_DeletesAuthor()
    {
        var author = new Author { Id = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        await _service.DeleteAsync(1, CancellationToken.None);

        _repoMock.Verify(r =>
            r.DeleteAsync(author, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByNameAsync_ReturnsAuthor()
    {
        var author = new Author { FirstName = "Leo", LastName = "Tolstoy" };

        _repoMock.Setup(r =>
                r.GetByNameAsync("Leo", "Tolstoy", It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        var result = await _service.GetByNameAsync("Leo", "Tolstoy", CancellationToken.None);

        result.Should().NotBeNull();
        result!.FirstName.Should().Be("Leo");
    }
}
