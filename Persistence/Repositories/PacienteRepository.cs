using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly ClinicaDbContext _context;

        public PacienteRepository(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync() =>
            await _context.Pacientes.ToListAsync();

        public async Task<Paciente?> GetByIdAsync(int id) =>
            await _context.Pacientes.FindAsync(id);

        public async Task<Paciente?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        }

        public async Task<IEnumerable<Paciente>> GetByClinicaIdAsync(int clinicaId)
        {
            return await _context.Pacientes
                .Include(p => p.Usuario)
                .Where(p => p.Usuario.ClinicaId == clinicaId)
                .Select(p => new Paciente 
                { 
                    Id = p.Id, 
                    Nombre = p.Nombre, 
                    Apellido = p.Apellido 
                })
                .ToListAsync();
        }

        public async Task<Paciente> AddAsync(Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
            return paciente;
        }

        public async Task UpdateAsync(Paciente paciente)
        {
            _context.Entry(paciente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
            }
        }
    }
}