using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class EspecialidadRepository : IEspecialidadRepository
    {
        private readonly ClinicaDbContext _context;

        public EspecialidadRepository(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Especialidad>> GetAllAsync() =>
            await _context.Especialidades.ToListAsync();

        public async Task<Especialidad?> GetByIdAsync(int id) =>
            await _context.Especialidades.FindAsync(id);

        public async Task<Especialidad> AddAsync(Especialidad especialidad)
        {
            _context.Especialidades.Add(especialidad);
            await _context.SaveChangesAsync();
            return especialidad;
        }

        public async Task UpdateAsync(Especialidad especialidad)
        {
            _context.Especialidades.Update(especialidad);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var especialidad = await _context.Especialidades.FindAsync(id);
            if (especialidad != null)
            {
                _context.Especialidades.Remove(especialidad);
                await _context.SaveChangesAsync();
            }
        }
    }
}