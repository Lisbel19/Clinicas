using Application.Interfaces;
using Domain.Entities;
using Persistence.Repositories;

namespace Application.Services
{
    public class EspecialidadService : IEspecialidadService
    {
        private readonly IEspecialidadRepository _especialidadRepository;

        public EspecialidadService(IEspecialidadRepository especialidadRepository)
        {
            _especialidadRepository = especialidadRepository;
        }

        public async Task<IEnumerable<Especialidad>> GetAllAsync()
        {
            return await _especialidadRepository.GetAllAsync();
        }

        public async Task<Especialidad?> GetByIdAsync(int id)
        {
            return await _especialidadRepository.GetByIdAsync(id);
        }

        public async Task<Especialidad> CreateAsync(Especialidad especialidad)
        {
            return await _especialidadRepository.AddAsync(especialidad);
        }

        public async Task UpdateAsync(Especialidad especialidad)
        {
            await _especialidadRepository.UpdateAsync(especialidad);
        }

        public async Task DeleteAsync(int id)
        {
            await _especialidadRepository.DeleteAsync(id);
        }
    }
}
