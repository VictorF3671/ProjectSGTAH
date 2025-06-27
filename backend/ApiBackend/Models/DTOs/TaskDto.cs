namespace ApiBackend.Models.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TimeEntries { get; set; }  
    }
}
