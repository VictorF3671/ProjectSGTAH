using ApiBackend.Data;
using ApiBackend.Interfaces;
using ApiBackend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ApiBackend.Services
{
    public class TaskServices : ITaskServices
    {
        private readonly AppDbContext _appDbContext;
        public TaskServices(AppDbContext appDbContext)
            => _appDbContext = appDbContext;

        public async Task<IEnumerable<TaskDto>> GetAllAsync(int? projectId = null)
        {
            var query = _appDbContext.Tasks
                .Where(t => t.DeletedAt == null);

            if (projectId.HasValue)
                query = query.Where(t => t.ProjectId == projectId.Value);

            return await query
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    ProjectId = t.ProjectId,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    TimeEntries = t.TimeTrackers.Count
                })
                .ToListAsync();
        }

        public async Task<TaskDto> GetByIdAsync(int id)
        {
            var t = await _appDbContext.Tasks
                .Include(t => t.TimeTrackers)
                .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);

            if (t == null)
                throw new KeyNotFoundException("Task não encontrada.");

            return new TaskDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                ProjectId = t.ProjectId,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                TimeEntries = t.TimeTrackers.Count
            };
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            // valida projeto
            var proj = await _appDbContext.Project
                .FirstOrDefaultAsync(p => p.Id == dto.ProjectId && p.DeletedAt == null);
            if (proj == null)
                throw new InvalidOperationException("Projeto não encontrado.");

            var t = new Models.Tasks
            {
                Name = dto.Name,
                Description = dto.Description,
                ProjectId = dto.ProjectId
            };

            _appDbContext.Tasks.Add(t);
            await _appDbContext.SaveChangesAsync();

            return new TaskDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                ProjectId = t.ProjectId,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                TimeEntries = 0
            };
        }

        public async Task UpdateAsync(int id, UpdateTaskDto dto)
        {
            var t = await _appDbContext.Tasks.FindAsync(id);
            if (t == null || t.DeletedAt != null)
                throw new KeyNotFoundException("Task não encontrada.");

            t.Name = dto.Name;
            t.Description = dto.Description;
            t.UpdatedAt = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var t = await _appDbContext.Tasks.FindAsync(id);
            if (t == null || t.DeletedAt != null)
                throw new KeyNotFoundException("Task não encontrada.");

            t.DeletedAt = DateTime.UtcNow;
            await _appDbContext.SaveChangesAsync();
        }
    }
}
