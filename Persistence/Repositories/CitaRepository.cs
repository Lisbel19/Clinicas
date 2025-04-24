using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CitaRepository : ICitaRepository
    {
        private readonly ClinicaDbContext _context;

        public CitaRepository(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cita>> GetAllAsync()
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Include(c => c.Clinica)
                .ToListAsync();
        }

        public async Task<Cita?> GetByIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Include(c => c.Clinica)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Cita>> GetByPacienteEmailAsync(string email)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .ThenInclude(p => p.Usuario)
                .Include(c => c.Medico)
                .ThenInclude(m => m.Especialidad)
                .Include(c => c.Clinica)
                .Where(c => c.Paciente.Usuario.Correo == email)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByMedicoEmailAsync(string email)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                    .ThenInclude(p => p.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m.Especialidad)
                .Include(c => c.Clinica)
                .Where(c => c.Medico.Usuario.Correo == email)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }

        public async Task<Cita> AddAsync(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return cita;
        }

        public async Task UpdateAsync(Cita cita)
        {
            _context.Entry(cita).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId)
        {
            return await _context.Citas
                .Where(c => c.PacienteId == pacienteId)
                .Include(c => c.Medico)
                .Include(c => c.Clinica)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId)
        {
            return await _context.Citas
                .Where(c => c.MedicoId == medicoId)
                .Include(c => c.Paciente)
                .Include(c => c.Clinica)
                .ToListAsync();
        }
    }
}