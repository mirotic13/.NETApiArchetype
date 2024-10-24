using ApiArchetype.Controllers;
using ApiArchetype.Services;
using Domain.Entities.Template;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ApiArchetype.UnitTests.Controllers;

public class TemplateClassControllerTests
{
    private readonly Mock<ITemplateClassService> _serviceMock;
    private readonly TemplateClassController _controller;

    public TemplateClassControllerTests()
    {
        _serviceMock = new Mock<ITemplateClassService>();
        _controller = new TemplateClassController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithListOfTemplates()
    {
        // Arrange
        var templates = new List<TemplateClass>
        {
            new() { Id = 1, Name = "Template 1" },
            new() { Id = 2, Name = "Template 2" }
        };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(templates);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(templates);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkResult_WhenTemplateExists()
    {
        // Arrange
        var template = new TemplateClass { Id = 1, Name = "Template 1" };
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(template);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(template);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenTemplateDoesNotExist()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((TemplateClass)null);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction_WhenTemplateIsCreated()
    {
        // Arrange
        var template = new TemplateClass { Id = 1, Name = "New Template" };
        _serviceMock.Setup(s => s.AddAsync(template)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(template);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdAtActionResult = result as CreatedAtActionResult;
        createdAtActionResult.ActionName.Should().Be(nameof(_controller.GetById));
        createdAtActionResult.RouteValues.Should().Contain("id", template.Id);
        createdAtActionResult.Value.Should().BeEquivalentTo(template);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenTemplateIsUpdated()
    {
        // Arrange
        var template = new TemplateClass { Id = 1, Name = "Updated Template" };
        _serviceMock.Setup(s => s.UpdateAsync(template)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(1, template);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        var template = new TemplateClass { Id = 2, Name = "Updated Template" };

        // Act
        var result = await _controller.Update(1, template);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenTemplateIsDeleted()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}
