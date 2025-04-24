using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicasController : ControllerBase
    {
        private readonly IClinicaService _clinicaService;

        public ClinicasController(IClinicaService clinicaService)
        {
            _clinicaService = clinicaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clinica>>> GetAll()
        {
            var clinicas = await _clinicaService.GetAllAsync();
            return Ok(clinicas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Clinica>> GetById(int id)
        {
            var clinica = await _clinicaService.GetByIdAsync(id);
            if (clinica == null)
                return NotFound();

            return Ok(clinica);
        }

        [HttpPost]
        public async Task<ActionResult<Clinica>> Create(Clinica clinica)
        {
            var createdClinica = await _clinicaService.CreateAsync(clinica);
            return CreatedAtAction(nameof(GetById), new { id = createdClinica.Id }, createdClinica);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Clinica clinica)
        {
            if (id != clinica.Id)
                return BadRequest();

            await _clinicaService.UpdateAsync(clinica);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clinicaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
