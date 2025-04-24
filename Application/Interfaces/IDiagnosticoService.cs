using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDiagnosticoService
    {
        Task<IEnumerable<Diagnostico>> GetAllAsync();
        Task<Diagnostico?> GetByIdAsync(int id);
        Task<Diagnostico> CreateAsync(Diagnostico diagnostico);
        Task UpdateAsync(Diagnostico diagnostico);
        Task DeleteAsync(int id);
    }
}
