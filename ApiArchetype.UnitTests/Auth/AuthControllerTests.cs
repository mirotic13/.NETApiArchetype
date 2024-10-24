using ApiArchetype.Auth.Controller;
using ApiArchetype.Auth.Service;
using Domain.Entities.Auth.Request;
using Domain.Entities.Auth.Response;
using Domain.Entities.Auth;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;

namespace ApiArchetype.UnitTests.Auth;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task Login_ShouldReturnOkResult_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new UserLoginRequest { Username = "test_user", Password = "valid_password" };
        var response = new UserLoginResponse { Token = "valid_token" };
        _authServiceMock.Setup(s => s.AuthenticateAsync(request.Username, request.Password))
                        .ReturnsAsync(response);

        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext = httpContext;

        // Act
        var result = await _controller.Login(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var request = new UserLoginRequest { Username = "test_user", Password = "invalid_password" };
        _authServiceMock.Setup(s => s.AuthenticateAsync(request.Username, request.Password))
                        .ReturnsAsync((UserLoginResponse)null);

        // Act
        var result = await _controller.Login(request);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Value.Should().Be("Incorrect credentials.");
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var request = new UserRegisterRequest { Username = "new_user", Password = "new_password" };
        var serviceResponse = new ServiceResponse<int> { Success = true };

        _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<User>())).ReturnsAsync(serviceResponse);

        // Act
        var result = await _controller.Register(request);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenRegistrationFails()
    {
        // Arrange
        var request = new UserRegisterRequest { Username = "existing_user", Password = "password" };
        var serviceResponse = new ServiceResponse<int> { Success = false, Message = "El usuario ya existe." };

        _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<User>())).ReturnsAsync(serviceResponse);

        // Act
        var result = await _controller.Register(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().Be("El usuario ya existe.");
    }

    [Fact]
    public void Logout_ShouldReturnOkResult_WhenCalled()
    {
        // Act
        var result = _controller.Logout();

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}
