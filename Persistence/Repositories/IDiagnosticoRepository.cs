using Domain.Entities;

namespace Persistence.Repositories
{
    public interface IDiagnosticoRepository
    {
        Task<IEnumerable<Diagnostico>> GetAllAsync();
        Task<Diagnostico?> GetByIdAsync(int id);
        Task<Diagnostico> AddAsync(Diagnostico diagnostico);
        Task UpdateAsync(Diagnostico diagnostico);
        Task DeleteAsync(int id);
    }
}