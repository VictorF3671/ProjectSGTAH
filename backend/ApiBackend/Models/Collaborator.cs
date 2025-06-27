using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ApiBackend.Models
{
    [Index(nameof(UserId), IsUnique = true)]
    public class Collaborator
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; } = null!;

        
        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
