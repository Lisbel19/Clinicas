using Domain.Entities;

namespace Persistence.Repositories
{
    public interface IPacienteRepository
    {
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task<Paciente?> GetByIdAsync(int id);
        Task<Paciente?> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Paciente>> GetByClinicaIdAsync(int clinicaId);
        Task<Paciente> AddAsync(Paciente paciente);
        Task UpdateAsync(Paciente paciente);
        Task DeleteAsync(int id);
    }
}