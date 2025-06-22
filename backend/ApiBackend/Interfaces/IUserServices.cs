using ApiBackend.Models;

namespace ApiBackend.Interfaces
{
    public interface IUserServices
    {
        string? Authenticate(string username, string password);
        string GenerateHashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        string GenerateJwtToken(User user);

        Task<User> Create(CreateUserDto userDto);

        Task<User?> GetById(int id);

        Task<IEnumerable<object>> GetAllUser();
    }
}
