using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Paciente
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Column("telefono")]
        public string Telefono { get; set; } = string.Empty;

        [Column("direccion")]
        public string Direccion { get; set; } = string.Empty;

        [Column("correo")]
        public string Correo { get; set; } = string.Empty;
        
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public ICollection<Cita>? Citas { get; set; }
        public ICollection<Diagnostico>? Diagnosticos { get; set; }
    }
}
