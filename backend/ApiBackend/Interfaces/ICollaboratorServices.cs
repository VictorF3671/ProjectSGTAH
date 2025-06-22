using ApiBackend.Models.DTOs;

namespace ApiBackend.Interfaces
{
    public interface ICollaboratorServices
    {
        Task<IEnumerable<CollaboratorDto>> GetAllAsync();
        Task<CollaboratorDto> GetByIdAsync(int id);
        Task<CollaboratorDto> CreateAsync(CreateCollaboratorDto dto);
        Task DeleteAsync(int id);
    }
}
