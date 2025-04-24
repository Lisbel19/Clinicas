using Domain.Entities;

namespace Persistence.Repositories
{
    public interface IClinicaRepository
    {
        Task<IEnumerable<Clinica>> GetAllAsync();
        Task<Clinica?> GetByIdAsync(int id);
        Task<Clinica> AddAsync(Clinica clinica);
        Task UpdateAsync(Clinica clinica);
        Task DeleteAsync(int id);
    }
}