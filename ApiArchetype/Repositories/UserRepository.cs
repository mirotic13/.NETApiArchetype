using Dapper;
using Domain.Entities.Auth;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace ApiArchetype.Repositories;
public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task<int> AddAsync(User user);
}

[ExcludeFromCodeCoverage]
public class UserRepository(IDbConnection dbConnection) : IUserRepository
{
    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var query = "SELECT * FROM Users WHERE Username = @Username";
        return await dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Username = username });
    }

    public async Task<int> AddAsync(User user)
    {
        var query = @"INSERT INTO Users (Username, PasswordHash, Role)
                      VALUES (@Username, @PasswordHash, @Role);
                      SELECT SCOPE_IDENTITY();";
        return await dbConnection.ExecuteScalarAsync<int>(query, user);
    }
}
