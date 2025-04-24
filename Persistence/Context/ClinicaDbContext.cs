using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.Context
{
    public class ClinicaDbContext : DbContext
    {
        public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options)
            : base(options) { }

        public DbSet<Clinica> Clinicas => Set<Clinica>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Medico> Medicos => Set<Medico>();
        public DbSet<Paciente> Pacientes => Set<Paciente>();
        public DbSet<Cita> Citas => Set<Cita>();
        public DbSet<Diagnostico> Diagnosticos => Set<Diagnostico>();
        public DbSet<Especialidad> Especialidades => Set<Especialidad>();
    }
}
