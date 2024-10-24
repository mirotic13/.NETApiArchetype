namespace Domain.Entities.Auth;

public class User
{
    public int Id { get; set; }

    public required string Username { get; set; }

    public required string PasswordHash { get; set; }

    public string PasswordSalt { get; set; } = string.Empty;

    public Role Role { get; set; } = Role.User;
}

public enum Role
{
    User,
    Admin
}
