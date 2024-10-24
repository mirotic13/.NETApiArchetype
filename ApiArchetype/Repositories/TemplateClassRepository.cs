using Domain.Entities.Template;
using System.Data;
using Dapper;
using System.Diagnostics.CodeAnalysis;

namespace ApiArchetype.Repositories;
public interface ITemplateClassRepository
{
    Task<IEnumerable<TemplateClass>> GetAllAsync();
    Task<TemplateClass?> GetByIdAsync(int id);
    Task AddAsync(TemplateClass template);
    Task UpdateAsync(TemplateClass template);
    Task DeleteAsync(int id);
}

[ExcludeFromCodeCoverage]
public class TemplateClassRepository(IDbConnection dbConnection) : ITemplateClassRepository
{
    public async Task<IEnumerable<TemplateClass>> GetAllAsync()
    {
        var sql = "SELECT * FROM TemplateClasses";
        return await dbConnection.QueryAsync<TemplateClass>(sql);
    }

    public async Task<TemplateClass?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM TemplateClasses WHERE Id = @Id";
        return await dbConnection.QueryFirstOrDefaultAsync<TemplateClass>(sql, new { Id = id });
    }

    public async Task AddAsync(TemplateClass template)
    {
        var sql = "INSERT INTO TemplateClasses (Name, CreatedAt, Status, Amount) VALUES (@Name, @CreatedAt, @Status, @Amount)";
        await dbConnection.ExecuteAsync(sql, new
        {
            template.Name,
            template.CreatedAt,
            template.Status,
            template.Amount
        });
    }

    public async Task UpdateAsync(TemplateClass template)
    {
        var sql = "UPDATE TemplateClasses SET Name = @Name, CreatedAt = @CreatedAt, Status = @Status, Amount = @Amount WHERE Id = @Id";
        await dbConnection.ExecuteAsync(sql, new
        {
            template.Name,
            template.CreatedAt,
            template.Status,
            template.Amount,
            template.Id
        });
    }

    public async Task DeleteAsync(int id)
    {
        var sql = "DELETE FROM TemplateClasses WHERE Id = @Id";
        await dbConnection.ExecuteAsync(sql, new { Id = id });
    }
}
