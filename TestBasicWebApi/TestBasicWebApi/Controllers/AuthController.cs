using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestBasicWebApi.Data;
using TestBasicWebApi.Models;
using TestBasicWebApi.Services;

namespace TestBasicWebApi.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // Verifica si el usuario ya existe
            if (await _context.Usuarios.AnyAsync(u => u.Username == model.Username))
            {
                return BadRequest(new { message = "El nombre de usuario ya está en uso" });
            }

            // Hashea la contraseña antes de guardarla
            var usuario = new UsuarioModel
            {
                Username = model.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado exitosamente" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Busca el usuario en la base de datos
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            // Verifica si el usuario existe y la contraseña es correcta
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password))
                return Unauthorized(new { message = "Credenciales incorrectas" });

            // Genera el token JWT
            var token = _jwtService.GenerateToken(usuario.Username);

            return Ok(new { token });
        }
    }
}
