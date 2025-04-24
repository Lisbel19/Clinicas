using Application.Interfaces;
using Domain.Entities;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMedicoRepository _medicoRepository;
        private readonly IClinicaRepository _clinicaRepository;

        public CitaService(
            ICitaRepository citaRepository,
            IPacienteRepository pacienteRepository,
            IMedicoRepository medicoRepository,
            IClinicaRepository clinicaRepository)
        {
            _citaRepository = citaRepository;
            _pacienteRepository = pacienteRepository;
            _medicoRepository = medicoRepository;
            _clinicaRepository = clinicaRepository;
        }

        public async Task<IEnumerable<Cita>> GetAllAsync()
        {
            return await _citaRepository.GetAllAsync();
        }

        public async Task<Cita?> GetByIdAsync(int id)
        {
            return await _citaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Paciente>> GetPacientesByClinicaAsync(int clinicaId)
        {
            return await _pacienteRepository.GetByClinicaIdAsync(clinicaId);
        }

        public async Task<IEnumerable<Especialidad>> GetEspecialidadesByClinicaAsync(int clinicaId)
        {
            return await _medicoRepository.GetEspecialidadesByClinicaAsync(clinicaId);
        }

        public async Task<IEnumerable<Medico>> GetMedicosByClinicaAndEspecialidadAsync(int clinicaId, int especialidadId)
        {
            return await _medicoRepository.GetByClinicaAndEspecialidadAsync(clinicaId, especialidadId);
        }

        public async Task<IEnumerable<Cita>> GetByPacienteEmailAsync(string email)
        {
            return await _citaRepository.GetByPacienteEmailAsync(email);
        }

        public async Task<IEnumerable<Cita>> GetByMedicoEmailAsync(string email)
        {
            return await _citaRepository.GetByMedicoEmailAsync(email);
        }

        public async Task<Cita> CreateAsync(Cita cita)
        {
            // Validar que existan las entidades relacionadas
            var paciente = await _pacienteRepository.GetByIdAsync(cita.PacienteId);
            if (paciente == null)
                throw new ArgumentException("Paciente no encontrado");

            var medico = await _medicoRepository.GetByIdAsync(cita.MedicoId);
            if (medico == null)
                throw new ArgumentException("Médico no encontrado");

            var clinica = await _clinicaRepository.GetByIdAsync(cita.ClinicaId);
            if (clinica == null)
                throw new ArgumentException("Clínica no encontrada");

            // Validar que la fecha no sea pasada
            if (cita.Fecha < DateTime.Now)
                throw new ArgumentException("No se pueden agendar citas en fechas pasadas");

            return await _citaRepository.AddAsync(cita);
        }

        public async Task UpdateAsync(Cita cita)
        {
            var existingCita = await _citaRepository.GetByIdAsync(cita.Id);
            if (existingCita == null)
                throw new KeyNotFoundException("Cita no encontrada");

            // Validar entidades relacionadas
            if (cita.PacienteId != existingCita.PacienteId)
            {
                var paciente = await _pacienteRepository.GetByIdAsync(cita.PacienteId);
                if (paciente == null)
                    throw new ArgumentException("Nuevo paciente no encontrado");
            }

            if (cita.MedicoId != existingCita.MedicoId)
            {
                var medico = await _medicoRepository.GetByIdAsync(cita.MedicoId);
                if (medico == null)
                    throw new ArgumentException("Nuevo médico no encontrado");
            }

            if (cita.ClinicaId != existingCita.ClinicaId)
            {
                var clinica = await _clinicaRepository.GetByIdAsync(cita.ClinicaId);
                if (clinica == null)
                    throw new ArgumentException("Nueva clínica no encontrada");
            }

            await _citaRepository.UpdateAsync(cita);
        }

        public async Task DeleteAsync(int id)
        {
            await _citaRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Paciente>> GetPacientesParaCitasAsync()
        {
            var pacientes = await _pacienteRepository.GetAllAsync();
            return pacientes.Select(p => new Paciente { Id = p.Id, Nombre = p.Nombre, Apellido = p.Apellido });
        }

        public async Task<IEnumerable<Medico>> GetMedicosParaCitasAsync()
        {
            var medicos = await _medicoRepository.GetAllAsync();
            return medicos.Select(m => new Medico { Id = m.Id, Nombre = m.Nombre, Apellido = m.Apellido });
        }

        public async Task<IEnumerable<Clinica>> GetClinicasParaCitasAsync()
        {
            var clinicas = await _clinicaRepository.GetAllAsync();
            return clinicas.Select(c => new Clinica { Id = c.Id, Nombre = c.Nombre });
        }

        public async Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId)
        {
            return await _citaRepository.GetByPacienteIdAsync(pacienteId);
        }

        public async Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId)
        {
            return await _citaRepository.GetByMedicoIdAsync(medicoId);
        }
    }
}