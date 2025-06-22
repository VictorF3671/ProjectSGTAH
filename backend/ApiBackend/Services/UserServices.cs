using ApiBackend.Data;
using ApiBackend.Interfaces;
using ApiBackend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiBackend.Services
{
    public class UserServices : IUserServices
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserServices(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    

        public string? Authenticate(string username, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return null;

            if (!VerifyPassword(password, user.Password))
                return null;

            var token = GenerateJwtToken(user);
            return token;
        }
        

        public string GenerateHashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A senha não pode ser vazia.", nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       
        public Task<User> Create([FromBody] CreateUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Password = GenerateHashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DeletedAt = null
            };

            _context.User.Add(user);
            _context.SaveChanges();

            return Task.FromResult(user);
        }

 
        public async Task<IEnumerable<object>> GetAllUser()
        {
            var users = await _context.User
            .Where(u => u.DeletedAt == null)   
            .Select(u => new UserDtoOutput
            {
                Id = u.Id,
                Username = u.Username,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .ToListAsync();

            return users;
        }



        public Task<User?> GetById(int id)
        {
            var user = _context.User.Find(id);
            return Task.FromResult(user);
        }


    }
}
