using ApiArchetype.Repositories;
using ApiArchetype.Services;
using Domain.Entities.Template;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApiArchetype.UnitTests.Services;
public class TemplateClassServiceTests
{
    private readonly Mock<ITemplateClassRepository> _repositoryMock;
    private readonly TemplateClassService _service;

    public TemplateClassServiceTests()
    {
        _repositoryMock = new Mock<ITemplateClassRepository>();
        _service = new TemplateClassService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTemplates()
    {
        // Arrange
        var templates = new List<TemplateClass>
    {
        new() { Id = 1, Name = "Template1" },
        new() { Id = 2, Name = "Template2" }
    };

        _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(templates);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2).And.Contain(templates);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTemplateById()
    {
        // Arrange
        var template = new TemplateClass { Id = 1, Name = "Template1" };
        _repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(template);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(template);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTemplateNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TemplateClass)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }
}
