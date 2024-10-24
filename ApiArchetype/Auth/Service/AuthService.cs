using Domain.Entities.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Domain.Entities.Auth.Response;
using Domain.Entities;
using ApiArchetype.Repositories;
using Domain.Entities.Settings;
using Microsoft.Extensions.Options;

namespace ApiArchetype.Auth.Service;
public interface IAuthService
{
    Task<UserLoginResponse> AuthenticateAsync(string username, string password);
    Task<ServiceResponse<int>> RegisterAsync(User user);
}

public class AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings) : IAuthService
{
    public async Task<UserLoginResponse> AuthenticateAsync(string username, string password)
    {
        var user = await userRepository.GetUserByUsernameAsync(username);

        if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            return null;

        var response = new UserLoginResponse
        {
            Token = GenerateJwtToken(user)
        };

        return response;
    }

    public async Task<ServiceResponse<int>> RegisterAsync(User user)
    {
        var existingUser = await userRepository.GetUserByUsernameAsync(user.Username);
        if (existingUser != null)
            return new ServiceResponse<int>
            {
                Success = false,
                Message = "El usuario ya existe."
            };

        CreatePasswordHash(user.PasswordHash, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = Convert.ToBase64String(passwordHash);
        user.PasswordSalt = Convert.ToBase64String(passwordSalt);

        var userId = await userRepository.AddAsync(user);

        return new ServiceResponse<int>
        {
            Success = true,
            Data = userId,
            Message = "Usuario registrado exitosamente."
        };
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
    {
        byte[] storedSaltBytes = Convert.FromBase64String(storedSalt);

        using var hmac = new System.Security.Cryptography.HMACSHA512(storedSaltBytes);
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        byte[] storedHashBytes = Convert.FromBase64String(storedHash);
        for (int i = 0; i < storedHashBytes.Length; i++)
        {
            if (storedHashBytes[i] != computedHash[i])
            {
                return false;
            }
        }
        return true;
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Value.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
            ]),
            Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings.Value.ExpireHours)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
