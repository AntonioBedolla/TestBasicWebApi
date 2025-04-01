namespace TestBasicWebApi.Dtos
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public bool Completada { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioId { get; set; }

        // Solo muestra los datos que necesitas del usuario
        public string UsuarioNombre { get; set; }
        public string UsuarioEmail { get; set; }
    }
}
