using Application.Interfaces;
using Domain.Entities;
using Persistence.Repositories;

namespace Application.Services
{
    public class ClinicaService : IClinicaService
    {
        private readonly IClinicaRepository _clinicaRepository;

        public ClinicaService(IClinicaRepository clinicaRepository)
        {
            _clinicaRepository = clinicaRepository;
        }

        public async Task<IEnumerable<Clinica>> GetAllAsync()
        {
            return await _clinicaRepository.GetAllAsync();
        }

        public async Task<Clinica?> GetByIdAsync(int id)
        {
            return await _clinicaRepository.GetByIdAsync(id);
        }

        public async Task<Clinica> CreateAsync(Clinica clinica)
        {
            return await _clinicaRepository.AddAsync(clinica);
        }

        public async Task UpdateAsync(Clinica clinica)
        {
            await _clinicaRepository.UpdateAsync(clinica);
        }

        public async Task DeleteAsync(int id)
        {
            await _clinicaRepository.DeleteAsync(id);
        }
    }
}
