using ApiBackend.Data;
using ApiBackend.Interfaces;
using ApiBackend.Models;
using ApiBackend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ApiBackend.Services
{
    public class TimeTrackerServices : ITimeTrackerServices
    {
        private readonly AppDbContext _appDbContext;
        public TimeTrackerServices(AppDbContext appDbContext)
            => _appDbContext = appDbContext;

        public async Task<IEnumerable<TimeTrackerDto>> GetAllAsync(int? taskId = null, int? collaboratorId = null)
        {
            var query = _appDbContext.TimeTracker
                .Where(tt => tt.DeletedAt == null);

            if (taskId.HasValue)
                query = query.Where(tt => tt.TaskId == taskId.Value);

            if (collaboratorId.HasValue)
                query = query.Where(tt => tt.CollaboratorId == collaboratorId.Value);

            return await query
                .Select(tt => new TimeTrackerDto
                {
                    Id = tt.Id,
                    TaskId = tt.TaskId,
                    CollaboratorId = tt.CollaboratorId,
                    StartDate = tt.StartDate,
                    EndDate = tt.EndDate,
                    TimeZoneId = tt.TimeZoneId,
                    CreatedAt = tt.CreatedAt,
                    UpdatedAt = tt.UpdatedAt,
                    DurationHours = (tt.EndDate - tt.StartDate).TotalHours
                })
                .ToListAsync();
        }

        public async Task<TimeTrackerDto> GetByIdAsync(int id)
        {
            var tt = await _appDbContext.TimeTracker
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

            if (tt == null)
                throw new KeyNotFoundException("TimeTracker não encontrado.");

            return new TimeTrackerDto
            {
                Id = tt.Id,
                TaskId = tt.TaskId,
                CollaboratorId = tt.CollaboratorId,
                StartDate = tt.StartDate,
                EndDate = tt.EndDate,
                TimeZoneId = tt.TimeZoneId,
                CreatedAt = tt.CreatedAt,
                UpdatedAt = tt.UpdatedAt,
                DurationHours = (tt.EndDate - tt.StartDate).TotalHours
            };
        }

        public async Task<TimeTrackerDto> CreateAsync(CreateTimeTrackerDto dto)
        {
            // Valida ordem de datas
            if (dto.EndDate < dto.StartDate)
                throw new InvalidOperationException("EndDate deve ser maior ou igual a StartDate.");

            // Checa Task e Collaborator
            var task = await _appDbContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == dto.TaskId && t.DeletedAt == null);
            if (task == null)
                throw new InvalidOperationException("Task não encontrada.");

            var coll = await _appDbContext.Collaborator
                .FirstOrDefaultAsync(c => c.Id == dto.CollaboratorId && c.DeletedAt == null);
            if (coll == null)
                throw new InvalidOperationException("Colaborador não encontrado.");

            var overlap = await _appDbContext.TimeTracker
                .Where(x => x.CollaboratorId == dto.CollaboratorId && x.DeletedAt == null)
                .AnyAsync(x =>
                    dto.StartDate < x.EndDate &&
                    dto.EndDate > x.StartDate
                );
            if (overlap)
                throw new InvalidOperationException("O intervalo colide com outro registro existente.");

            // Limite de 24h no dia
            var tz = TimeZoneInfo.FindSystemTimeZoneById(dto.TimeZoneId);
            var localDate = TimeZoneInfo.ConvertTimeFromUtc(dto.StartDate, tz).Date;
            var dayStartUtc = TimeZoneInfo.ConvertTimeToUtc(localDate, tz);
            var dayEndUtc = dayStartUtc.AddDays(1);

            var todayIntervals = await _appDbContext.TimeTracker
               .Where(tt => tt.CollaboratorId == dto.CollaboratorId
              && tt.DeletedAt == null
              && tt.StartDate >= dayStartUtc
              && tt.StartDate < dayEndUtc)
                .Select(tt => new { tt.StartDate, tt.EndDate })
                .ToListAsync();

            var minutesToday = todayIntervals
                .Sum(x => (int)(x.EndDate - x.StartDate).TotalMinutes);

            var newMinutes = (int)(dto.EndDate - dto.StartDate).TotalMinutes;
            if (minutesToday + newMinutes > 24 * 60)
                throw new InvalidOperationException("Total de horas no dia ultrapassa 24 horas.");

            // Cria
            var tt = new TimeTracker
            {
                TaskId = dto.TaskId,
                CollaboratorId = dto.CollaboratorId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TimeZoneId = dto.TimeZoneId
            };

            _appDbContext.TimeTracker.Add(tt);
            await _appDbContext.SaveChangesAsync();

            return new TimeTrackerDto
            {
                Id = tt.Id,
                TaskId = tt.TaskId,
                CollaboratorId = tt.CollaboratorId,
                StartDate = tt.StartDate,
                EndDate = tt.EndDate,
                TimeZoneId = tt.TimeZoneId,
                CreatedAt = tt.CreatedAt,
                UpdatedAt = tt.UpdatedAt,
                DurationHours = (tt.EndDate - tt.StartDate).TotalHours
            };
        }

        public async Task DeleteAsync(int id)
        {
            var tt = await _appDbContext.TimeTracker
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (tt == null)
                throw new KeyNotFoundException("TimeTracker não encontrado.");

            tt.DeletedAt = DateTime.UtcNow;
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<string> GetTodayTotalAsync(int collaboratorId)
        {
            var now = DateTime.UtcNow;
            var tz = TimeZoneInfo.FindSystemTimeZoneById(
                          // busca qualquer entry para extrair o fuso, ou use Local
                          _appDbContext.TimeTracker
                              .Where(x => x.CollaboratorId == collaboratorId)
                              .Select(x => x.TimeZoneId)
                              .FirstOrDefault() ??
                          TimeZoneInfo.Local.Id);

            var localDate = TimeZoneInfo.ConvertTimeFromUtc(now, tz).Date;
            var startDay = TimeZoneInfo.ConvertTimeToUtc(localDate, tz);
            var endDay = startDay.AddDays(1);

            var intervals = await _appDbContext.TimeTracker
                .Where(x => x.CollaboratorId == collaboratorId
                         && x.DeletedAt == null
                         && x.StartDate >= startDay
                         && x.StartDate < endDay)
                .Select(x => new { x.StartDate, x.EndDate })
                .ToListAsync();

            var totalMinutes = intervals
                .Sum(x => (int)(x.EndDate - x.StartDate).TotalMinutes);

            var span = TimeSpan.FromMinutes(totalMinutes);
            return $"{(int)span.TotalHours:D2}:{span.Minutes:D2}";
        }

        public async Task<string> GetMonthTotalAsync(int collaboratorId)
        {
           
            var now = DateTime.UtcNow;

            var tzId = await _appDbContext.TimeTracker
                .Where(x => x.CollaboratorId == collaboratorId)
                .Select(x => x.TimeZoneId)
                .FirstOrDefaultAsync()
                ?? TimeZoneInfo.Local.Id;
            var tz = TimeZoneInfo.FindSystemTimeZoneById(tzId);
            var localNow = TimeZoneInfo.ConvertTimeFromUtc(now, tz);
            var firstOfMonth = new DateTime(localNow.Year, localNow.Month, 1);
            var startMonth = TimeZoneInfo.ConvertTimeToUtc(firstOfMonth, tz);
            var endMonth = startMonth.AddMonths(1);

            var intervals = await _appDbContext.TimeTracker
                .Where(x =>
                    x.CollaboratorId == collaboratorId &&
                    x.DeletedAt == null &&
                    x.StartDate >= startMonth &&
                    x.StartDate < endMonth)
                .Select(x => new { x.StartDate, x.EndDate })
                .ToListAsync();

            var totalMinutes = intervals
                .Sum(x => (int)(x.EndDate - x.StartDate).TotalMinutes);

            var span = TimeSpan.FromMinutes(totalMinutes);
            return $"{(int)span.TotalHours:D2}:{span.Minutes:D2}";
        }

        public async Task<TimeTrackerDto> UpdateAsync(int id, UpdateTimeTrackerDto dto)
        {
            var tt = await _appDbContext.TimeTracker
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (tt == null)
                throw new KeyNotFoundException("TimeTracker não encontrado.");

            if (dto.EndDate < dto.StartDate)
                throw new InvalidOperationException("EndDate deve ser maior ou igual a StartDate.");

           
            var tzId = string.IsNullOrWhiteSpace(tt.TimeZoneId)
                ? TimeZoneInfo.Local.Id
                : tt.TimeZoneId!;
            var tz = TimeZoneInfo.FindSystemTimeZoneById(tzId);

            var localDay = TimeZoneInfo.ConvertTimeFromUtc(dto.StartDate, tz).Date;
            var dayStartUtc = TimeZoneInfo.ConvertTimeToUtc(localDay, tz);
            var dayEndUtc = dayStartUtc.AddDays(1);

            var overlap = await _appDbContext.TimeTracker
                .Where(x =>
                    x.CollaboratorId == tt.CollaboratorId &&
                    x.Id != id &&
                    x.DeletedAt == null &&

                    x.EndDate > x.StartDate &&

                    x.StartDate >= dayStartUtc &&
                    x.StartDate < dayEndUtc
                )
                .AnyAsync(x =>
                    dto.StartDate < x.EndDate &&
                    dto.EndDate > x.StartDate
                );

            if (overlap)
                throw new InvalidOperationException("O intervalo colide com outro registro existente.");

            tt.StartDate = dto.StartDate;
            tt.EndDate = dto.EndDate;

            await _appDbContext.SaveChangesAsync();

            return new TimeTrackerDto
            {
                Id = tt.Id,
                TaskId = tt.TaskId,
                CollaboratorId = tt.CollaboratorId,
                StartDate = tt.StartDate,
                EndDate = tt.EndDate,
                TimeZoneId = tt.TimeZoneId,
                CreatedAt = tt.CreatedAt,
                UpdatedAt = tt.UpdatedAt,
                DurationHours = (tt.EndDate - tt.StartDate).TotalHours
            };
        }
    }
}
