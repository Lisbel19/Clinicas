using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Usuario
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("correo")]
        public string Correo { get; set; } = string.Empty;

        [Column("contrasena")]
        public string Contrasena { get; set; } = string.Empty;

        [Column("tipo_usuario")]
        public string TipoUsuario { get; set; } = string.Empty;

        [Column("clinica_id")]
        public int? ClinicaId { get; set; }
        public Clinica? Clinica { get; set; }

        public Medico? Medico { get; set; }
        public Paciente? Paciente { get; set; }
    }
}
