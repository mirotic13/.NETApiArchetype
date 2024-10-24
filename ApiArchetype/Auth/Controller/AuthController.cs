using Microsoft.AspNetCore.Mvc;
using Domain.Entities.Auth;
using Domain.Entities.Auth.Request;
using ApiArchetype.Auth.Service;

namespace ApiArchetype.Auth.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest model)
    {
        var response = await authService.AuthenticateAsync(model.Username, model.Password);

        if (response == null)
        {
            logger.LogWarning("Invalid credentials for user: {Username}", model.Username);
            return Unauthorized("Invalid credentials.");
        }
            
        SetHttpCookie(response.Token);
        logger.LogInformation("Logging successfull for user: {Username}", model.Username);

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterRequest model)
    {
        var user = new User
        {
            Username = model.Username,
            PasswordHash = model.Password
        };

        var result = await authService.RegisterAsync(user);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok();
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        if (Request.Cookies["AuthToken"] != null)
            Response.Cookies.Delete("AuthToken");

        return Ok();
    }

    private void SetHttpCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        Response.Cookies.Append("AuthToken", token, cookieOptions);
    }
}