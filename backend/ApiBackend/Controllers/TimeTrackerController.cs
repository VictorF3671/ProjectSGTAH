using ApiBackend.Interfaces;
using ApiBackend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeTrackerController : ControllerBase
    {
        private readonly ITimeTrackerServices _services;
        public TimeTrackerController(ITimeTrackerServices svc)
            => _services = svc;

        
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? taskId, [FromQuery] int? collaboratorId)
            => Ok(await _services.GetAllAsync(taskId, collaboratorId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _services.GetByIdAsync(id));

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTimeTrackerDto dto)
        {
            var tt = await _services.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = tt.Id }, tt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTimeTrackerDto dto)
        {
            var updated = await _services.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _services.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("today/{collaboratorId}")]
        public async Task<IActionResult> GetTodayTotal(int collaboratorId)
            => Ok(await _services.GetTodayTotalAsync(collaboratorId));

        [HttpGet("month/{collaboratorId}")]
        public async Task<IActionResult> GetMonthTotal(int collaboratorId)
            => Ok(await _services.GetMonthTotalAsync(collaboratorId));
    }
}
