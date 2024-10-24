using ApiArchetype.Auth.Service;
using Domain.Entities.Auth;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using ApiArchetype.Repositories;
using System.Text;

namespace ApiArchetype.UnitTests.Auth;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _configurationMock = new Mock<IConfiguration>();

        _configurationMock.Setup(c => c["Jwt:Secret"]).Returns("super_secret_key_12345_for_jwt_authentication");
        _configurationMock.Setup(c => c["Jwt:ExpireHours"]).Returns("1");

        _authService = new AuthService(_userRepositoryMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var password = "valid_password";
        byte[] passwordHash, passwordSalt;

        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        var user = new User
        {
            Id = 1,
            Username = "test_user",
            PasswordHash = Convert.ToBase64String(passwordHash),
            PasswordSalt = Convert.ToBase64String(passwordSalt),
            Role = Role.User
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(user.Username))
                           .ReturnsAsync(user);

        // Act
        var result = await _authService.AuthenticateAsync(user.Username, password);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
    }


    [Fact]
    public async Task AuthenticateAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()))
                           .ReturnsAsync((User)null);

        // Act
        var result = await _authService.AuthenticateAsync("nonexistent_user", "password");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnSuccess_WhenUserIsNew()
    {
        // Arrange
        var user = new User
        {
            Username = "new_user",
            PasswordHash = "password",
            PasswordSalt = "salt"
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(user.Username))
                           .ReturnsAsync((User)null);

        _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                           .ReturnsAsync(1);

        // Act
        var result = await _authService.RegisterAsync(user);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().Be(1);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnFailure_WhenUserAlreadyExists()
    {
        // Arrange
        var user = new User
        {
            Username = "existing_user",
            PasswordHash = "password",
            PasswordSalt = "salt"
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(user.Username))
                           .ReturnsAsync(user);

        // Act
        var result = await _authService.RegisterAsync(user);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("El usuario ya existe.");
    }
}
