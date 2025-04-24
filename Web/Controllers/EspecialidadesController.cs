using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IEspecialidadService _especialidadService;

        public EspecialidadesController(IEspecialidadService especialidadService)
        {
            _especialidadService = especialidadService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Especialidad>>> GetAll()
        {
            var especialidades = await _especialidadService.GetAllAsync();
            return Ok(especialidades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Especialidad>> GetById(int id)
        {
            var especialidad = await _especialidadService.GetByIdAsync(id);
            if (especialidad == null)
                return NotFound();

            return Ok(especialidad);
        }

        [HttpPost]
        public async Task<ActionResult<Especialidad>> Create(Especialidad especialidad)
        {
            var created = await _especialidadService.CreateAsync(especialidad);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Especialidad especialidad)
        {
            if (id != especialidad.Id)
                return BadRequest();

            await _especialidadService.UpdateAsync(especialidad);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _especialidadService.DeleteAsync(id);
            return NoContent();
        }
    }
}
