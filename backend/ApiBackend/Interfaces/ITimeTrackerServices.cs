using ApiBackend.Models.DTOs;

namespace ApiBackend.Interfaces
{
    public interface ITimeTrackerServices
    {
        
   
        Task<IEnumerable<TimeTrackerDto>> GetAllAsync(int? taskId = null, int? collaboratorId = null);
        Task<TimeTrackerDto> GetByIdAsync(int id);
        Task<TimeTrackerDto> CreateAsync(CreateTimeTrackerDto dto);    
        Task DeleteAsync(int id);
        Task<TimeTrackerDto> UpdateAsync(int id, UpdateTimeTrackerDto dto);
        Task<string> GetTodayTotalAsync(int collaboratorId);
        Task<string> GetMonthTotalAsync(int collaboratorId);
    }
}

