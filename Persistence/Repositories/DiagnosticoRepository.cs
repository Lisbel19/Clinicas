using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class DiagnosticoRepository : IDiagnosticoRepository
    {
        private readonly ClinicaDbContext _context;

        public DiagnosticoRepository(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Diagnostico>> GetAllAsync() =>
            await _context.Diagnosticos.ToListAsync();

        public async Task<Diagnostico?> GetByIdAsync(int id) =>
            await _context.Diagnosticos.FindAsync(id);

        public async Task<Diagnostico> AddAsync(Diagnostico diagnostico)
        {
            _context.Diagnosticos.Add(diagnostico);
            await _context.SaveChangesAsync();
            return diagnostico;
        }

        public async Task UpdateAsync(Diagnostico diagnostico)
        {
            _context.Diagnosticos.Update(diagnostico);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var diagnostico = await _context.Diagnosticos.FindAsync(id);
            if (diagnostico != null)
            {
                _context.Diagnosticos.Remove(diagnostico);
                await _context.SaveChangesAsync();
            }
        }
    }
}