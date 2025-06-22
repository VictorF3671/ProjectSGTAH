using ApiBackend.Models.DTOs;

namespace ApiBackend.Interfaces
{
    public interface ITimeTrackerServices
    {
        
   
        /// Lista todos os apontamentos, podendo filtrar por task ou colaborador.
        Task<IEnumerable<TimeTrackerDto>> GetAllAsync(int? taskId = null, int? collaboratorId = null);

        
        /// Retorna um apontamento pelo id.
        Task<TimeTrackerDto> GetByIdAsync(int id);

        /// Cria um novo apontamento, validando colisões, limites de 24h/dia e ordem de datas.
        
        Task<TimeTrackerDto> CreateAsync(CreateTimeTrackerDto dto);

        /// Marca um apontamento como excluído 
       
        Task DeleteAsync(int id);

       
        /// Retorna o total de horas gastas no dia corrente (Horas:Minutos).
       
        Task<string> GetTodayTotalAsync(int collaboratorId);

        /// Retorna o total de horas gastas no mês corrente (Horas:Minutos).
        Task<string> GetMonthTotalAsync(int collaboratorId);
    }
}

