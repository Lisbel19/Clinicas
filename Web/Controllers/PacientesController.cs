using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;
        private readonly IUsuarioService _usuarioService;

        public PacientesController(
        IPacienteService pacienteService,
        IUsuarioService usuarioService)
        {
            _pacienteService = pacienteService;
            _usuarioService = usuarioService; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetAll()
        {
            var pacientes = await _pacienteService.GetAllAsync();
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Paciente>> GetById(int id)
        {
            var paciente = await _pacienteService.GetByIdAsync(id);
            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpGet("usuarios-pacientes")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosPacientes()
        {
            var usuarios = await _usuarioService.GetUsuariosPacientesAsync();
            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<ActionResult<Paciente>> Create([FromBody] Paciente paciente, [FromQuery] string correoUsuario)
        {
            try
            {
                var created = await _pacienteService.CreateAsync(paciente, correoUsuario);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear paciente: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Paciente pacienteActualizado)
        {
            try
            {
                if (id != pacienteActualizado.Id)
                {
                    return BadRequest("ID en la URL no coincide con ID del paciente");
                }

                var pacienteExistente = await _pacienteService.GetByIdAsync(id);
                if (pacienteExistente == null)
                {
                    return NotFound();
                }

                // Preservar datos que no deben cambiar
                pacienteActualizado.UsuarioId = pacienteExistente.UsuarioId;
                pacienteActualizado.Correo = pacienteExistente.Correo;

                await _pacienteService.UpdateAsync(pacienteActualizado);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar paciente: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pacienteService.DeleteAsync(id);
            return NoContent();
        }
    }
}
