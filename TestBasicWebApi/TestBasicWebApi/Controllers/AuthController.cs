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
            if (await _context.Usuarios.AnyAsync(u => u.Email == model.Username))
            {
                return BadRequest(new { message = "El nombre de usuario ya está en uso" });
            }

            // Hashea la contraseña antes de guardarla
            var usuario = new UsuarioModel
            {
                Email = model.Username,
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
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            //Validar que el usuario no llegue nulo
            if (usuario == null)
            {
                return Unauthorized(new { message = "Usuario no encontrado" });
            }

            //validar quee el hash almacenado es valido antes de verificar
            if(string.IsNullOrWhiteSpace(usuario.Password) || !usuario.Password.StartsWith("$2"))
            {
                return BadRequest(new { message = "Formato de contraseña inválido en la base de datos." });
            }

            // Verifica si el usuario existe y la contraseña es correcta
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password))
                return Unauthorized(new { message = "Credenciales incorrectas" });

            // Genera el token JWT
            var token = _jwtService.GenerateToken(usuario.Id, usuario.Email);

            return Ok(new { token });
        }

        //Este motodo es por si las contraseñas almacenadas no estan encriptadas
        [HttpPost("encrypt-passwords")]
        [EndpointDescription("Este motodo es por si las contraseñas almacenadas no estan encriptadas")]
        public IActionResult EncryptExistingPasswords()
        {
            var users = _context.Usuarios.Where(u => !u.Password.StartsWith("$2b$")).ToList(); // Filtra los que no están encriptados

            foreach (var user in users)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            _context.SaveChanges();

            return Ok(new { message = $"Se han encriptado {users.Count} contraseñas." });
        }
    }
}
