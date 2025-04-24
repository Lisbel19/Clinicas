using Domain.Entities;

namespace Application.Interfaces
{
    public interface IClinicaService
    {
        Task<IEnumerable<Clinica>> GetAllAsync();
        Task<Clinica?> GetByIdAsync(int id);
        Task<Clinica> CreateAsync(Clinica clinica);
        Task UpdateAsync(Clinica clinica);
        Task DeleteAsync(int id);
    }
}
