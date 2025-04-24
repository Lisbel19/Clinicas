using Domain.Entities;

namespace Application.Interfaces
{
    public interface IEspecialidadService
    {
        Task<IEnumerable<Especialidad>> GetAllAsync();
        Task<Especialidad?> GetByIdAsync(int id);
        Task<Especialidad> CreateAsync(Especialidad especialidad);
        Task UpdateAsync(Especialidad especialidad);
        Task DeleteAsync(int id);
    }
}
