using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cita>>> GetAll()
        {
            var citas = await _citaService.GetAllAsync();
            return Ok(citas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetById(int id)
        {
            var cita = await _citaService.GetByIdAsync(id);
            if (cita == null)
                return NotFound();

            return Ok(cita);
        }

        [HttpGet("pacientes")]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientesParaCitas()
        {
            var pacientes = await _citaService.GetPacientesParaCitasAsync();
            return Ok(pacientes);
        }

        [HttpGet("medicos")]
        public async Task<ActionResult<IEnumerable<Medico>>> GetMedicosParaCitas()
        {
            var medicos = await _citaService.GetMedicosParaCitasAsync();
            return Ok(medicos);
        }

        [HttpGet("clinicas")]
        public async Task<ActionResult<IEnumerable<Clinica>>> GetClinicasParaCitas()
        {
            var clinicas = await _citaService.GetClinicasParaCitasAsync();
            return Ok(clinicas);
        }

        [HttpGet("por-paciente/{pacienteId}")]
        public async Task<ActionResult<IEnumerable<Cita>>> GetByPacienteId(int pacienteId)
        {
            var citas = await _citaService.GetByPacienteIdAsync(pacienteId);
            return Ok(citas);
        }

        [HttpGet("por-medico/{medicoId}")]
        public async Task<ActionResult<IEnumerable<Cita>>> GetByMedicoId(int medicoId)
        {
            var citas = await _citaService.GetByMedicoIdAsync(medicoId);
            return Ok(citas);
        }

        [HttpGet("pacientes-por-clinica/{clinicaId}")]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientesByClinica(int clinicaId)
        {
            var pacientes = await _citaService.GetPacientesByClinicaAsync(clinicaId);
            return Ok(pacientes);
        }

        [HttpGet("especialidades-por-clinica/{clinicaId}")]
        public async Task<ActionResult<IEnumerable<Especialidad>>> GetEspecialidadesByClinica(int clinicaId)
        {
            var especialidades = await _citaService.GetEspecialidadesByClinicaAsync(clinicaId);
            return Ok(especialidades);
        }

        [HttpGet("medicos-por-clinica-especialidad/{clinicaId}/{especialidadId}")]
        public async Task<ActionResult<IEnumerable<Medico>>> GetMedicosByClinicaAndEspecialidad(
            int clinicaId, int especialidadId)
        {
            var medicos = await _citaService.GetMedicosByClinicaAndEspecialidadAsync(clinicaId, especialidadId);
            return Ok(medicos);
        }

        [HttpGet("por-correo-paciente/{email}")]
        public async Task<ActionResult<dynamic>> GetByPacienteEmail(string email)
        {
            try
            {
                var citas = await _citaService.GetByPacienteEmailAsync(email);
                
                var primeraCita = citas.FirstOrDefault();
                var nombrePaciente = primeraCita != null ? 
                    $"{primeraCita.Paciente.Nombre} {primeraCita.Paciente.Apellido}" : 
                    "Paciente";

                return Ok(new {
                    citas = citas,
                    nombrePaciente = nombrePaciente
                    // La especialidad ya viene incluida en cada cita.Medico.Especialidad
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener citas del paciente: {ex.Message}");
            }
        }

        [HttpGet("por-correo-medico/{email}")]
        public async Task<ActionResult<dynamic>> GetByMedicoEmail(string email)
        {
            try
            {
                var citas = await _citaService.GetByMedicoEmailAsync(email);
                
                var primeraCita = citas.FirstOrDefault();
                var nombreMedico = primeraCita != null ? 
                    $"{primeraCita.Medico.Nombre} {primeraCita.Medico.Apellido}" : 
                    "Médico";

                return Ok(new {
                    citas = citas,
                    nombreMedico = nombreMedico,
                    especialidadPrincipal = primeraCita?.Medico?.Especialidad?.Nombre
                    // La especialidad de cada cita ya viene en cita.Medico.Especialidad
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener citas del médico: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cita>> Create([FromBody] Cita cita)
        {
            try
            {
                var created = await _citaService.CreateAsync(cita);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear cita: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cita cita)
        {
            try
            {
                if (id != cita.Id)
                    return BadRequest("ID en la URL no coincide con ID de la cita");

                await _citaService.UpdateAsync(cita);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar cita: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _citaService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar cita: {ex.Message}");
            }
        }

        [HttpDelete("medico/{id}")]
        public async Task<IActionResult> DeleteByMedico(int id)
        {
            try
            {
                // Primero verificamos que la cita exista
                var cita = await _citaService.GetByIdAsync(id);
                if (cita == null)
                    return NotFound("Cita no encontrada");

                // Aquí podrías agregar lógica adicional para verificar que el médico
                // que está cancelando es el mismo que tiene asignada la cita
                // Esto requeriría pasar el email del médico en el request y verificarlo

                await _citaService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al cancelar cita: {ex.Message}");
            }
        }
    }
}