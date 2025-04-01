using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TestBasicWebApi.Data;
using TestBasicWebApi.Models;

namespace TestBasicWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // 🔒 Protegemos con JWT
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks()
        {
            // Obtener el ID del usuario desde el token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Validar que el ID sea un número entero
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Usuario no autorizado. ID inválido." });
            }

            // Obtener tareas del usuario
            var tasks = _context.Tareas.Where(t => t.UsuarioId == userId).ToList();
            return Ok(tasks);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> GetTask(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var task = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);

            if (task == null)
                return NotFound(new { message = "Tarea no encontrada" });

            return task;
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskModel>> CreateTask(TaskModel task)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            task.UsuarioId = userId; // Asigna el usuario autenticado

            _context.Tareas.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskModel updatedTask)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var task = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);

            if (task == null)
                return NotFound(new { message = "Tarea no encontrada" });

            task.Titulo = updatedTask.Titulo;
            task.Descripcion = updatedTask.Descripcion;
            task.Completada = updatedTask.Completada;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var task = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);

            if (task == null)
                return NotFound(new { message = "Tarea no encontrada" });

            _context.Tareas.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
