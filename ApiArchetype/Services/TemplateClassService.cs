using ApiArchetype.Repositories;
using Domain.Entities.Template;

namespace ApiArchetype.Services;
public interface ITemplateClassService
{
    Task<IEnumerable<TemplateClass>> GetAllAsync();
    Task<TemplateClass?> GetByIdAsync(int id);
    Task AddAsync(TemplateClass template);
    Task UpdateAsync(TemplateClass template);
    Task DeleteAsync(int id);
}

public class TemplateClassService(ITemplateClassRepository repository) : ITemplateClassService
{
    public async Task<IEnumerable<TemplateClass>> GetAllAsync() => await repository.GetAllAsync();
    public async Task<TemplateClass?> GetByIdAsync(int id) => await repository.GetByIdAsync(id);
    public async Task AddAsync(TemplateClass template) => await repository.AddAsync(template);
    public async Task UpdateAsync(TemplateClass template) => await repository.UpdateAsync(template);
    public async Task DeleteAsync(int id) => await repository.DeleteAsync(id);
}
