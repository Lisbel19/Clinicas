using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ClinicaDbContext _context;

        public UsuarioRepository(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync() =>
            await _context.Usuarios
                .Include(u => u.Clinica)
                .ToListAsync();

        public async Task<Usuario?> GetByIdAsync(int id) =>
            await _context.Usuarios
                .Include(u => u.Clinica)
                .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<Usuario?> GetByCorreoAsync(string correo) =>
            await _context.Usuarios
                .Include(u => u.Clinica)
                .FirstOrDefaultAsync(u => u.Correo == correo);

        public async Task<IEnumerable<Usuario>> GetUsuariosMedicosAsync()
        {
            return await _context.Usuarios
                .Where(u => u.TipoUsuario == "medico" && !_context.Medicos.Any(m => m.UsuarioId == u.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosPacientesAsync()
        {
            return await _context.Usuarios
                .Where(u => u.TipoUsuario == "paciente" && !_context.Pacientes.Any(p => p.UsuarioId == u.Id))
                .ToListAsync();
        }

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            var usuarioExistente = await _context.Usuarios
                .Include(u => u.Medico)
                .FirstOrDefaultAsync(u => u.Id == usuario.Id);

            if (usuarioExistente == null)
                throw new InvalidOperationException("Usuario no encontrado");

            // Si es médico y cambió el correo, actualizar también en Médico
            if (usuarioExistente.TipoUsuario == "medico" && 
                usuarioExistente.Correo != usuario.Correo && 
                usuarioExistente.Medico != null)
            {
                usuarioExistente.Medico.Correo = usuario.Correo;
            }

            _context.Entry(usuarioExistente).CurrentValues.SetValues(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
    }
}