namespace ApiBackend.Models
{
    public class Login
    {
        public required string Username { get; set; }
        
        public required string Password { get; set; }
        public string? Token { get; set; } 
    }
}
