using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestBasicWebApi.Data;
using TestBasicWebApi.Dtos;
using TestBasicWebApi.Models;

namespace TestBasicWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
                _context = context;
        }

        // ✅ 1. Obtener todos los usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioModel>>> GetUsers()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // ✅ 2. Obtener un usuario por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioModel>> GetUser(int id)
        {
            var users = await _context.Usuarios
        .Select(u => new UserDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Email = u.Email
        })
        .ToListAsync();

            return Ok(users);
        }

        // ✅ 3. Actualizar un usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UsuarioModel updatedUser)
        {
            try
            {
                var user = await _context.Usuarios.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                // Si la contraseña enviada es diferente a la guardada, la encriptamos
                if (!string.IsNullOrEmpty(updatedUser.Password) && !BCrypt.Net.BCrypt.Verify(updatedUser.Password, user.Password))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password);
                }

                user.Nombre = updatedUser.Nombre;
                user.Email = updatedUser.Email;

                _context.Usuarios.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // ✅ 4. Eliminar un usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
