using System.ComponentModel.DataAnnotations;

namespace ApiBackend.Models.DTOs
{
    public class CreateCollaboratorDto
    {
        [Required]
        public int UserId { get; set; }
    }
}
