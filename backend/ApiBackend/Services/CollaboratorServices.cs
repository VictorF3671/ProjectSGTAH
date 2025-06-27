using ApiBackend.Data;
using ApiBackend.Interfaces;
using ApiBackend.Models;
using Microsoft.EntityFrameworkCore;
using ApiBackend.Models.DTOs;

namespace ApiBackend.Services
{
    public class CollaboratorServices : ICollaboratorServices
    {
        private readonly AppDbContext _appDbContext;
        public CollaboratorServices(AppDbContext appDbContext)
            => _appDbContext = appDbContext;

        public async Task<IEnumerable<CollaboratorDto>> GetAllAsync()
        {
            return await _appDbContext.Collaborator
                .Where(c => c.DeletedAt == null && c.User.DeletedAt == null)
                .Select(c => new CollaboratorDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Username = c.Name
                })
                .ToListAsync();
        }

        public async Task<CollaboratorDto> GetByIdAsync(int id)
        {
            var c = await _appDbContext.Collaborator
                .Include(c => c.User)
                .Where(c => c.Id == id && c.DeletedAt == null && c.User.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (c == null)
                throw new KeyNotFoundException("Colaborador não encontrado.");

            return new CollaboratorDto
            {
                Id = c.Id,
                UserId = c.UserId,
                Username = c.Name
            };
        }

        public async Task<CollaboratorDto> CreateAsync(CreateCollaboratorDto dto)
        {
            
            var user = await _appDbContext.User
                .FirstOrDefaultAsync(u => u.Id == dto.UserId && u.DeletedAt == null);
            if (user == null)
                throw new InvalidOperationException("Usuário não encontrado.");

            
            if (await _appDbContext.Collaborator.AnyAsync(c => c.UserId == dto.UserId))
                throw new InvalidOperationException("Já existe colaborador para este usuário.");

            
            var collab = new Collaborator
            {
                UserId = dto.UserId,
                Name = user.Username
            };

            _appDbContext.Collaborator.Add(collab);
            await _appDbContext.SaveChangesAsync();

            return new CollaboratorDto
            {
                Id = collab.Id,
                UserId = collab.UserId,
                Username = collab.Name
            };
        }

        public async Task DeleteAsync(int id)
        {
            var collab = await _appDbContext.Collaborator
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
            if (collab == null)
                throw new KeyNotFoundException("Colaborador não encontrado.");

            collab.DeletedAt = DateTime.UtcNow;
            await _appDbContext.SaveChangesAsync();
        }
    }
}
