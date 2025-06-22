using ApiBackend.Models.DTOs;

namespace ApiBackend.Interfaces
{
    public interface ITaskServices
    {
        Task<IEnumerable<TaskDto>> GetAllAsync(int? projectId = null);
        Task<TaskDto> GetByIdAsync(int id);
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task UpdateAsync(int id, UpdateTaskDto dto);
        Task DeleteAsync(int id);
    }
}
