using System.ComponentModel.DataAnnotations;

namespace TestBasicWebApi.Models
{
    public class UsuarioModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }  // Debe ser almacenada encriptada en un entorno real
    }
}
