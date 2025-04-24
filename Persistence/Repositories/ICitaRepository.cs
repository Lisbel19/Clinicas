using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface ICitaRepository
    {
        Task<IEnumerable<Cita>> GetAllAsync();
        Task<Cita?> GetByIdAsync(int id);
        Task<Cita> AddAsync(Cita cita);
        Task UpdateAsync(Cita cita);
        Task DeleteAsync(int id);
        Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId);
        Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<Cita>> GetByPacienteEmailAsync(string email);
        Task<IEnumerable<Cita>> GetByMedicoEmailAsync(string email);
    }
}