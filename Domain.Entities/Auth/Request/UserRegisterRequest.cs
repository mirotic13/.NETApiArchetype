namespace Domain.Entities.Auth.Request;

public class UserRegisterRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
