using ApiBackend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using ApiBackend.Interfaces;

namespace ApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _services;
        public ProjectController(IProjectServices services) => _services = services;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _services.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) =>
            Ok(await _services.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] ProjectRequestDto dto)
        {
            var p = await _services.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, [FromBody] ProjectRequestDto dto)
        {
            await _services.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _services.DeleteAsync(id);
            return NoContent();
        }
    }
}