using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Diagnostico
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("sintomas")]
        public string Sintomas { get; set; } = string.Empty;

        [Column("conclusion")]
        public string Conclusion { get; set; } = string.Empty;

        [Column("tratamiento")]
        public string Tratamiento { get; set; } = string.Empty;

        [Column("observaciones")]
        public string? Observaciones { get; set; }

        [Column("cita_id")]
        public int CitaId { get; set; }
        public Cita? Cita { get; set; }

        [Column("medico_id")]
        public int MedicoId { get; set; }
        public Medico? Medico { get; set; }

        [Column("paciente_id")]
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }
    }
}
