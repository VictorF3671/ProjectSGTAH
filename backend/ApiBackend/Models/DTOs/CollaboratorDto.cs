namespace ApiBackend.Models.DTOs
{
    public class CollaboratorDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
    }
}
