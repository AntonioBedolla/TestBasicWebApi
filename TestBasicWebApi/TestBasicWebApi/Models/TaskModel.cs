using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestBasicWebApi.Models
{
    public class TaskModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public bool Completada { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relación con el usuario
        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        [JsonIgnore]
        public UsuarioModel? Usuario { get; set; }
    }
}
