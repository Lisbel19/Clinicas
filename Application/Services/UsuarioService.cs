using Application.Interfaces;
using Domain.Entities;
using Persistence.Repositories;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            return await _usuarioRepository.GetByCorreoAsync(correo);
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosPacientesAsync()
        {
            return await _usuarioRepository.GetUsuariosPacientesAsync();
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            return await _usuarioRepository.AddAsync(usuario);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task DeleteAsync(int id)
        {
            await _usuarioRepository.DeleteAsync(id);
        }
    }
}
