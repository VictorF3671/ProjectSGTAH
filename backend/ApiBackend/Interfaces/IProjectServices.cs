
using ApiBackend.Models.DTOs;

namespace ApiBackend.Interfaces
{
    public interface IProjectServices
    {

        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<ProjectDto> GetByIdAsync(int id);
        Task<ProjectDto> CreateAsync(ProjectRequestDto dto);
        Task UpdateAsync(int id, ProjectRequestDto dto);
        Task DeleteAsync(int id);
    }
}
