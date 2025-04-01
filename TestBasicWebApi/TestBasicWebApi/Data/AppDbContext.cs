using Microsoft.EntityFrameworkCore;
using TestBasicWebApi.Models;

namespace TestBasicWebApi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<TaskModel> Tareas { get; set; }

    }
}
