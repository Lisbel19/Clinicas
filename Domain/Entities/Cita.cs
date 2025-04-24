using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Cita
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("paciente_id")]
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        [Column("medico_id")]
        public int MedicoId { get; set; }
        public Medico? Medico { get; set; }

        [Column("clinica_id")]
        public int ClinicaId { get; set; }
        public Clinica? Clinica { get; set; }
    }
}
