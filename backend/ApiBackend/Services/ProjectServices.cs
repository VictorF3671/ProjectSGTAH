using ApiBackend.Data;
using ApiBackend.Interfaces;
using ApiBackend.Models;
using ApiBackend.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ApiBackend.Services
{
    public class ProjectServices : IProjectServices
    {
        private readonly AppDbContext _appDbContext;
        public ProjectServices(AppDbContext appDbContext)
            => _appDbContext = appDbContext;

        public async Task<ProjectDto> GetByIdAsync(int id)
        {
            var p = await _appDbContext.Project    
                .Include(x => x.Tasks)
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

            if (p == null)
                throw new KeyNotFoundException("Projeto não encontrado.");

            return new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                TaskCount = p.Tasks.Count
            };
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            return await _appDbContext.Project
                .Where(x => x.DeletedAt == null)
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    TaskCount = p.Tasks.Count
                })
                .ToListAsync();
        }

        public async Task<ProjectDto> CreateAsync(ProjectRequestDto dto)
        {
            var proj = new Project
            {
                Name = dto.Name
            };
            _appDbContext.Project.Add(proj);
            await _appDbContext.SaveChangesAsync();

            return new ProjectDto
            {
                Id = proj.Id,
                Name = proj.Name,
                CreatedAt = proj.CreatedAt,
                UpdatedAt = proj.UpdatedAt,
                TaskCount = 0
            };
        }

       public async Task UpdateAsync(int id, ProjectRequestDto dto)
    {
        var proj = await _appDbContext.Project.FindAsync(id);
        if (proj == null || proj.DeletedAt != null)
            throw new KeyNotFoundException("Projeto não encontrado.");

        proj.Name      = dto.Name;
        proj.UpdatedAt = DateTime.UtcNow;
        await _appDbContext.SaveChangesAsync();
    }

    
    public async Task DeleteAsync(int id)
    {
        var proj = await _appDbContext.Project.FindAsync(id);
        if (proj == null || proj.DeletedAt != null)
            throw new KeyNotFoundException("Projeto não encontrado.");

        proj.DeletedAt = DateTime.UtcNow;
        await _appDbContext.SaveChangesAsync();
    }

    }
}
