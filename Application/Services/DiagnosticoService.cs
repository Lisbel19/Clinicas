using Application.Interfaces;
using Domain.Entities;
using Persistence.Repositories;

namespace Application.Services
{
    public class DiagnosticoService : IDiagnosticoService
    {
        private readonly IDiagnosticoRepository _diagnosticoRepository;

        public DiagnosticoService(IDiagnosticoRepository diagnosticoRepository)
        {
            _diagnosticoRepository = diagnosticoRepository;
        }

        public async Task<IEnumerable<Diagnostico>> GetAllAsync()
        {
            return await _diagnosticoRepository.GetAllAsync();
        }

        public async Task<Diagnostico?> GetByIdAsync(int id)
        {
            return await _diagnosticoRepository.GetByIdAsync(id);
        }

        public async Task<Diagnostico> CreateAsync(Diagnostico diagnostico)
        {
            return await _diagnosticoRepository.AddAsync(diagnostico);
        }

        public async Task UpdateAsync(Diagnostico diagnostico)
        {
            await _diagnosticoRepository.UpdateAsync(diagnostico);
        }

        public async Task DeleteAsync(int id)
        {
            await _diagnosticoRepository.DeleteAsync(id);
        }
    }
}
