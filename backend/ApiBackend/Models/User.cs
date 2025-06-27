using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBackend.Models
{

   
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        public  DateTime CreatedAt { get; set; } 

        public  DateTime UpdatedAt { get; set; }

        public  DateTime? DeletedAt { get; set; }

        public Collaborator? Collaborator { get; set; }
    }
}
