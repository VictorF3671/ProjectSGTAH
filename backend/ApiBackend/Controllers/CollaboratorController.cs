using ApiBackend.Interfaces;
using ApiBackend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ApiBackend.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class CollaboratorController : ControllerBase
        {
            private readonly ICollaboratorServices _services;
            public CollaboratorController(ICollaboratorServices svc)
                => _services = svc;

            [HttpGet]
            public async Task<IActionResult> GetAll()
                => Ok(await _services.GetAllAsync());

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
                => Ok(await _services.GetByIdAsync(id));

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateCollaboratorDto dto)
            {
                var coll = await _services.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = coll.Id }, coll);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                await _services.DeleteAsync(id);
                return NoContent();
            }
        }
    }

