using ApiArchetype.Services;
using Domain.Entities.Template;
using Microsoft.AspNetCore.Mvc;

namespace ApiArchetype.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateClassController(ITemplateClassService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TemplateClass template)
    {
        await service.AddAsync(template);
        return CreatedAtAction(nameof(GetById), new { id = template.Id }, template);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TemplateClass template)
    {
        if (id != template.Id) return BadRequest();
        await service.UpdateAsync(template);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
