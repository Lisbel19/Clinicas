using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task<Paciente?> GetByIdAsync(int id);
        Task<Paciente> CreateAsync(Paciente paciente, string correoUsuario);
        Task UpdateAsync(Paciente paciente);
        Task DeleteAsync(int id);
        Task<bool> ExistsByUsuarioIdAsync(int usuarioId);
    }
}
