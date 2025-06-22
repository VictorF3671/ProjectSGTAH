using ApiBackend.Interfaces;
using ApiBackend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskServices _services;
        public TaskController(ITaskServices services) => _services = services;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? projectId)
            => Ok(await _services.GetAllAsync(projectId));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => Ok(await _services.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var t = await _services.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = t.Id }, t);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
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

