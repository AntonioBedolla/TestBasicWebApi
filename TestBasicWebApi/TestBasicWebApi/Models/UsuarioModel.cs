using System.ComponentModel.DataAnnotations;

namespace TestBasicWebApi.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }  // Debe ser almacenada encriptada en un entorno real
    }
}
