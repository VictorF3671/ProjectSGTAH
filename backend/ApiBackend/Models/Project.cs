using System.ComponentModel.DataAnnotations;

namespace ApiBackend.Models
{

    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<Tasks> Tasks { get; set; }
            = new List<Tasks>();
    }
}
