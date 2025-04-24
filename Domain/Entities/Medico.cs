using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Medico
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Column("telefono")]
        public string Telefono { get; set; } = string.Empty;

        [Column("correo")]
        public string Correo { get; set; } = string.Empty;

        [Column("horario_consulta")]
        public string? HorarioConsulta { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Column("especialidad_id")]
        public int? EspecialidadId { get; set; }
        public Especialidad? Especialidad { get; set; }

        public ICollection<Cita>? Citas { get; set; }
        public ICollection<Diagnostico>? Diagnosticos { get; set; }
    }
}
