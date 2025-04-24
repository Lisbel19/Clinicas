using Domain.Entities;

namespace Persistence.Repositories
{
    public interface IEspecialidadRepository
    {
        Task<IEnumerable<Especialidad>> GetAllAsync();
        Task<Especialidad?> GetByIdAsync(int id);
        Task<Especialidad> AddAsync(Especialidad especialidad);
        Task UpdateAsync(Especialidad especialidad);
        Task DeleteAsync(int id);
    }
}