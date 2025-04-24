using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICitaService
    {
        // Métodos básicos de CRUD
        Task<IEnumerable<Cita>> GetAllAsync();
        Task<Cita?> GetByIdAsync(int id);
        Task<Cita> CreateAsync(Cita cita);
        Task UpdateAsync(Cita cita);
        Task DeleteAsync(int id);
        
        // Métodos para obtener citas específicas
        Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId);
        Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId);
        
        // Métodos para los selects del formulario
        Task<IEnumerable<Clinica>> GetClinicasParaCitasAsync();
        
        // Nuevos métodos para el flujo de selección
        Task<IEnumerable<Paciente>> GetPacientesByClinicaAsync(int clinicaId);
        Task<IEnumerable<Especialidad>> GetEspecialidadesByClinicaAsync(int clinicaId);
        Task<IEnumerable<Medico>> GetMedicosByClinicaAndEspecialidadAsync(int clinicaId, int especialidadId);
        
        // Métodos anteriores (puedes mantenerlos o eliminarlos según necesidad)
        Task<IEnumerable<Paciente>> GetPacientesParaCitasAsync(); // Opcional - ahora usamos GetPacientesByClinicaAsync
        Task<IEnumerable<Medico>> GetMedicosParaCitasAsync(); // Opcional - ahora usamos GetMedicosByClinicaAndEspecialidadAsync

        Task<IEnumerable<Cita>> GetByPacienteEmailAsync(string email);
        Task<IEnumerable<Cita>> GetByMedicoEmailAsync(string email);
    }
}