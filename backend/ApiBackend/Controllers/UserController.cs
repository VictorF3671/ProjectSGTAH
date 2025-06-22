using ApiBackend.Data;
using Microsoft.AspNetCore.Mvc;
using ApiBackend.Interfaces;
using ApiBackend.Models;
using Microsoft.AspNetCore.Mvc.Formatters;


namespace ApiBackend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUserServices _userServices;

        public UserController(AppDbContext context, IUserServices userServices)
        {
            _context = context;
            _userServices = userServices;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDtoInput login)
        {
            if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Dados Inválidos para Login.");
            }

            var token = _userServices.Authenticate(login.Username, login.Password);
            if (token == null)
            {
                return Unauthorized("Usuario ou senha incorretos");
            }

            return Ok(new { token });
        }


        [HttpPost]
        public IActionResult Register([FromBody] CreateUserDto userDto)
        {
            if (userDto == null || string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest("Dados Inválidos para Registro.");
            }

            var user = _userServices.Create(userDto);

            if (user == null)
            {
                return BadRequest("Impossivel Criar Usuario");
            }
            return Ok(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userServices.GetAllUser();
            if (users == null)
            {
                return NotFound("Nenhum produto encontrado.");
            }
            return Ok(users);
        }

    }
}
