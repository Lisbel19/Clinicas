using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicosController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medico>>> GetAll()
        {
            var medicos = await _medicoService.GetAllAsync();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medico>> GetById(int id)
        {
            var medico = await _medicoService.GetByIdAsync(id);
            if (medico == null)
                return NotFound();

            return Ok(medico);
        }

        [HttpGet("por-usuario/{usuarioId}")]
        public async Task<ActionResult<Medico>> GetByUsuarioId(int usuarioId)
        {
            var medico = await _medicoService.GetByUsuarioIdAsync(usuarioId);
            if (medico == null)
                return NotFound();

            return Ok(medico);
        }

        [HttpGet("por-especialidad/{especialidadId}")]
        public async Task<ActionResult<IEnumerable<Medico>>> GetByEspecialidad(int especialidadId)
        {
            var medicos = await _medicoService.GetByEspecialidadAsync(especialidadId);
            return Ok(medicos);
        }

        [HttpGet("usuarios-medicos")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosMedicos()
        {
            var usuariosMedicos = await _medicoService.GetUsuariosMedicosAsync();
            return Ok(usuariosMedicos);
        }
        

        [HttpPost]
        public async Task<ActionResult<Medico>> Create([FromBody] Medico medico, [FromQuery] string correoUsuario)
        {
            try
            {
                var created = await _medicoService.CreateAsync(medico, correoUsuario);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Medico medico)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                medico.Id = id; // Asegurar que el ID coincida con la ruta
                await _medicoService.UpdateAsync(medico);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _medicoService.DeleteAsync(id);
            return NoContent();
        }
    }
}