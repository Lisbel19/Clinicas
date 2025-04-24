using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Clinica
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("rnc")]
        public string Rnc { get; set; } = string.Empty;

        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("telefono")]
        public string Telefono { get; set; } = string.Empty;

        [Column("direccion")]
        public string Direccion { get; set; } = string.Empty;

        [Column("correo")]
        public string Correo { get; set; } = string.Empty;

        [Column("horario")]
        public string Horario { get; set; } = string.Empty;

        public ICollection<Usuario>? Usuarios { get; set; }
        public ICollection<Cita>? Citas { get; set; }
    }
}
