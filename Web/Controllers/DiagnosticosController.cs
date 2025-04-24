using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticosController : ControllerBase
    {
        private readonly IDiagnosticoService _diagnosticoService;

        public DiagnosticosController(IDiagnosticoService diagnosticoService)
        {
            _diagnosticoService = diagnosticoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diagnostico>>> GetAll()
        {
            var diagnosticos = await _diagnosticoService.GetAllAsync();
            return Ok(diagnosticos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Diagnostico>> GetById(int id)
        {
            var diagnostico = await _diagnosticoService.GetByIdAsync(id);
            if (diagnostico == null)
                return NotFound();

            return Ok(diagnostico);
        }

        [HttpPost]
        public async Task<ActionResult<Diagnostico>> Create(Diagnostico diagnostico)
        {
            var created = await _diagnosticoService.CreateAsync(diagnostico);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Diagnostico diagnostico)
        {
            if (id != diagnostico.Id)
                return BadRequest();

            await _diagnosticoService.UpdateAsync(diagnostico);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _diagnosticoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
