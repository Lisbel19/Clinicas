using Domain.Entities;

namespace Persistence.Repositories
{
    public interface IMedicoRepository
    {
        Task<IEnumerable<Medico>> GetAllAsync();
        Task<Medico?> GetByIdAsync(int id);
        Task<Medico?> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Especialidad>> GetEspecialidadesByClinicaAsync(int clinicaId);
        Task<IEnumerable<Medico>> GetByClinicaAndEspecialidadAsync(int clinicaId, int especialidadId);
        Task<Medico> AddAsync(Medico medico);
        Task UpdateAsync(Medico medico);
        Task DeleteAsync(int id);
    }
}