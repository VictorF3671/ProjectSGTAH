using System.ComponentModel.DataAnnotations;

namespace ApiBackend.Models.DTOs
{
    public class ProjectRequestDto
    {
        
        public required string Name { get; set; } = null!;

    }
}
