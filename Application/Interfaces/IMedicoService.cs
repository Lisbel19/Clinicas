using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMedicoService
    {
        Task<IEnumerable<Medico>> GetAllAsync();
        Task<Medico?> GetByIdAsync(int id);
        Task<Medico?> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Usuario>> GetUsuariosMedicosAsync();
        Task<Medico> CreateAsync(Medico medico, string correoUsuario);
        Task UpdateAsync(Medico medico);
        Task DeleteAsync(int id);
        
        // Métodos adicionales para gestión de médicos
        Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId);
        Task<bool> ExistsByUsuarioIdAsync(int usuarioId);
    }
}