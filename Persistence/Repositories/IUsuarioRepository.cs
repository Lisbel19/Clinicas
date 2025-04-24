using Domain.Entities;

namespace Persistence.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByCorreoAsync(string correo);
        Task<IEnumerable<Usuario>> GetUsuariosMedicosAsync();
        Task<IEnumerable<Usuario>> GetUsuariosPacientesAsync();
        Task<Usuario> AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
    }
}