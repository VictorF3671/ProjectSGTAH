using System.ComponentModel.DataAnnotations;

namespace ApiBackend.Models
{
        public class Tasks
        {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        // FK para o Project
        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // 1:N → TimeTrackers
        public ICollection<TimeTracker> TimeTrackers { get; set; }
            = new List<TimeTracker>();
    }
    }
