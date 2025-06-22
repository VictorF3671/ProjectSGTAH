namespace ApiBackend.Models.DTOs
{
    public class TimeTrackerDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int CollaboratorId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimeZoneId { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // duração em horas (double) ou um TimeSpan/string
        public double DurationHours { get; set; }
    }
}
