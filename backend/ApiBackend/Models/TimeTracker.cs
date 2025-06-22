using System.ComponentModel.DataAnnotations;

namespace ApiBackend.Models
{
    public class TimeTracker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }   

        [Required]
        public DateTime EndDate { get; set; }  

        [Required, MaxLength(200)]
        public string TimeZoneId { get; set; } = null!; // "America/Sao_Paulo"

        [Required]
        public int TaskId { get; set; }
        public Tasks Task { get; set; } = null!;

        [Required]
        public int CollaboratorId { get; set; }
        public Collaborator Collaborator { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
