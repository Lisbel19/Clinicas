using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly ClinicaDbContext _context;

        public MedicoRepository(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medico>> GetAllAsync() =>
            await _context.Medicos
                .Include(m => m.Especialidad)
                .Include(m => m.Usuario)
                    .ThenInclude(u => u.Clinica)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Medico?> GetByIdAsync(int id) =>
            await _context.Medicos
                .Include(m => m.Especialidad)
                .Include(m => m.Usuario)
                    .ThenInclude(u => u.Clinica)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<Medico?> GetByUsuarioIdAsync(int usuarioId) =>
            await _context.Medicos
                .Include(m => m.Especialidad)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.UsuarioId == usuarioId);

        public async Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId) =>
            await _context.Medicos
                .Include(m => m.Usuario)
                .Where(m => m.EspecialidadId == especialidadId)
                .AsNoTracking()
                .ToListAsync();
        
        public async Task<IEnumerable<Especialidad>> GetEspecialidadesByClinicaAsync(int clinicaId)
        {
            return await _context.Medicos
                .Where(m => m.Usuario.ClinicaId == clinicaId)
                .Select(m => m.Especialidad)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<Medico>> GetByClinicaAndEspecialidadAsync(int clinicaId, int especialidadId)
        {
            return await _context.Medicos
                .Include(m => m.Usuario)
                .Where(m => m.Usuario.ClinicaId == clinicaId && m.EspecialidadId == especialidadId)
                .Select(m => new Medico 
                { 
                    Id = m.Id, 
                    Nombre = m.Nombre, 
                    Apellido = m.Apellido,
                    Especialidad = new Especialidad { Id = m.Especialidad.Id, Nombre = m.Especialidad.Nombre }
                })
                .ToListAsync();
        }

        public async Task<bool> ExistsByUsuarioIdAsync(int usuarioId) =>
            await _context.Medicos
                .AnyAsync(m => m.UsuarioId == usuarioId);

        public async Task<Medico> AddAsync(Medico medico)
        {
            if (await ExistsByUsuarioIdAsync(medico.UsuarioId))
            {
                throw new InvalidOperationException("Ya existe un médico asociado a este usuario");
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == medico.UsuarioId && u.TipoUsuario == "medico");
            
            if (usuario == null)
            {
                throw new InvalidOperationException("Usuario no encontrado o no es de tipo médico");
            }

            medico.Correo = usuario.Correo;
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();
            return medico;
        }

        public async Task UpdateAsync(Medico medico)
        {
            var medicoExistente = await GetByIdAsync(medico.Id) ?? 
                throw new InvalidOperationException("Médico no encontrado");

            // Preservar la relación con el usuario
            medico.UsuarioId = medicoExistente.UsuarioId;
            medico.Correo = medicoExistente.Correo;

            _context.Entry(medicoExistente).CurrentValues.SetValues(medico);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico != null)
            {
                _context.Medicos.Remove(medico);
                await _context.SaveChangesAsync();
            }
        }
    }
}