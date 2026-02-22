using Moq;
using Xunit;
using Domain.Entities;
using FluentAssertions;
using Application.Services;
using Application.DTOs.Category;
using Application.Interfaces.Repos;
using Discounts.Application.Exceptions;

namespace Application.Tests.Services;

public class CategoryServiceTests
{
    private readonly CategoryService _sut;
    private readonly Mock<ICategoryRepository> _repositoryMock;

    public CategoryServiceTests()
    {
        _repositoryMock = new Mock<ICategoryRepository>();
        _sut = new CategoryService(_repositoryMock.Object);
    }

    #region Get Tests
    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenCategoryIsNull()
    {
        // Arrnage
        var categoryId = 1;
        _repositoryMock.Setup(repo => repo.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        // Act & Assert
        await _sut.Invoking(s => s.GetByIdAsync(categoryId)).Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category with Id {categoryId} was not found!");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldMapAllPropertiesCorrectly_WhenFound()
    {
        // Arrange
        var entity = new Category { Id = 10, Name = "Test", Description = "Test Desc" };
        _repositoryMock.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        // Act
        var result = await _sut.GetByIdAsync(10);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(entity.Id);
        result.Name.Should().Be(entity.Name);
        result.Description.Should().Be(entity.Description);
    }

    [Fact]
    public async Task GetByIdsAsync_ShouldReturnEmpty_WhenInputListIsEmpty()
    {
        // Arrange
        var ids = new List<int>();
        _repositoryMock.Setup(r => r.GetByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>());

        // Act
        var result = await _sut.GetByIdsAsync(ids);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdsAsync_ShouldReturnOnlyRequestedItems()
    {
        // Arrange
        var ids = new List<int> { 1, 2 };
        var returnedFromRepo = new List<Category> { new() { Id = 1 }, new() { Id = 2 } };
        _repositoryMock.Setup(r => r.GetByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnedFromRepo);

        // Act
        var result = await _sut.GetByIdsAsync(ids);

        // Assert
        result.Should().HaveCount(2);
        result.Select(x => x.Id).Should().Contain(new List<int> { 1, 2 });
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos_WhenDataExists()
    {
        // Arrange
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Tech" },
            new() { Id = 2, Name = "Home" }
        };

        _repositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Tech");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoCategoriesInDb()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>());

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }
    #endregion

    #region Create Tests
    [Theory]
    [InlineData("New", false)]
    [InlineData("Existing", true)]
    public async Task CreateAsync_BehavesCorrectly_BasedOnName(string name, bool exists)
    {
        var dto = new CreateCategoryDto { Name = name };
        _repositoryMock.Setup(r => r.GetByNameAsync(name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exists ? new Category() : null);

        if (exists)
        {
            var act = () => _sut.CreateAsync(dto);
            await act.Should().ThrowAsync<DomainException>();
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        else
        {
            var result = await _sut.CreateAsync(dto);
            result.Name.Should().Be(name);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
    #endregion

    #region Update Tests
    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingEntity_WhenFound()
    {
        // Arrange
        var existing = new Category { Id = 1, Name = "Old", Description = "Old" };
        var updateDto = new UpdateCategoryDto { Id = 1, Name = "New", Description = "New" };

        _repositoryMock.Setup(repo => repo.GetByIdAsync(updateDto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        // Act
        await _sut.UpdateAsync(updateDto);

        // Assert
        existing.Name.Should().Be("New");
        existing.Description.Should().Be("New");
        _repositoryMock.Verify(repo => repo.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFound_WhenIdIsInvalid()
    {
        // Arrange
        var dto = new UpdateCategoryDto { Id = -1, Name = "Fail" };
        _repositoryMock.Setup(r => r.GetByIdAsync(-1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        // Act & Assert
        await _sut.Invoking(s => s.UpdateAsync(dto))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData("OldName", "NewName")]
    [InlineData("NewName", "NewName")]
    [InlineData("OldName", "OldName")]  // same name, should still update
    public async Task UpdateAsync_ShouldValidateNameChanges(string original, string updated)
    {
        // Arrange
        var existing = new Category { Id = 1, Name = original };
        var dto = new UpdateCategoryDto { Id = 1, Name = updated };

        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);

        // Act & Assert
        await _sut.UpdateAsync(dto);
        existing.Name.Should().Be(updated);

    }
    #endregion

    #region Delete Tests
    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryDelete_WhenCategoryExists()
    {
        // Arrange
        var existing = new Category { Id = 1 };
        _repositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        // Act
        await _sut.DeleteAsync(1);

        // Assert
        _repositoryMock.Verify(repo => repo.DeleteAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowNotFound_WhenCategoryMissing()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        // Act & Assert
        await _sut.Invoking(s => s.DeleteAsync(55))
            .Should().ThrowAsync<NotFoundException>();
    }
    #endregion

}
