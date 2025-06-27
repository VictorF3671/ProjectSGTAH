using System.ComponentModel.DataAnnotations;

namespace ApiBackend.Models.DTOs
{
    public class CreateTimeTrackerDto
    {
        [Required]
        public int TaskId { get; set; }

        [Required]
        public int CollaboratorId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }  

        [Required]
        public DateTime EndDate { get; set; }  

        [Required, MaxLength(200)]
        public string TimeZoneId { get; set; } = null!;
    }
}
