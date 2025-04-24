using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class ClinicaRepository : IClinicaRepository
    {
        private readonly ClinicaDbContext _context;

        public ClinicaRepository(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Clinica>> GetAllAsync() =>
            await _context.Clinicas.ToListAsync();

        public async Task<Clinica?> GetByIdAsync(int id) =>
            await _context.Clinicas.FindAsync(id);

        public async Task<Clinica> AddAsync(Clinica clinica)
        {
            _context.Clinicas.Add(clinica);
            await _context.SaveChangesAsync();
            return clinica;
        }

        public async Task UpdateAsync(Clinica clinica)
        {
            _context.Clinicas.Update(clinica);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var clinica = await _context.Clinicas.FindAsync(id);
            if (clinica != null)
            {
                _context.Clinicas.Remove(clinica);
                await _context.SaveChangesAsync();
            }
        }
    }
}