using System.ComponentModel.DataAnnotations;

namespace ApiBackend.Models.DTOs
{
    public class CreateTaskDto
    {
        [Required, MaxLength(250)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public int ProjectId { get; set; }
    }
}
