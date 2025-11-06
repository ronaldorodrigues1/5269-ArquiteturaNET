using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pacientes.ServiceAPI.Dto;
using Pacientes.ServiceAPI.Exceptions;
using Pacientes.ServiceAPI.Interfaces;

namespace Pacientes.ServiceAPI.Controllers
{
    [Authorize(Policy = "ApiScope")]
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteService _service;

        public PacienteController(IPacienteService service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> ListarAsync([FromQuery] int page = 1)
        {
            var PacientesCadastrados = await _service.ListarAsync(page);

            return Ok(PacientesCadastrados);
        }

        [HttpGet("formulario/{id?}")]
        public async Task<IActionResult> ObterFormularioAsync(long id = 0)
        {
            var dados = id > 0
                ? await _service.CarregarPorIdAsync(id)
                : new PacienteDto();

            return Ok(dados);
        }

        [HttpGet("formularioporcpf/{cpf?}")]
        public async Task<IActionResult> ObterFormularioPorCpfAsync(string? cpf)
        {
            try 
            { 
                var dados = !string.IsNullOrEmpty(cpf)
                    ? await _service.CarregarPorCpfAsync(cpf)
                    : new PacienteDto();

                    return Ok(dados);
            }
            catch (RegraDeNegocioException ex)
            {
                //return StatusCode(500, $"Erro: {ex.Message}");
                return BadRequest("Paciente não encontrado.");
            }
        }

        [HttpPut("Salvar")]
        [HttpPost("Salvar")]
        public async Task<IActionResult> SalvarAsync([FromBody] PacienteDto dados)
        {
            try
            {
                var pacienteSalvo = await _service.CadastrarAsync(dados);
                return Ok(pacienteSalvo);
            }
            catch (RegraDeNegocioException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
            }
        }

        [HttpDelete("Excluir/{id}")]
        public async Task<IActionResult> ExcluirAsync(int id)
        {
            await _service.ExcluirAsync(id);
            return Ok();
        }

        //// GET: api/Paciente
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<PacienteDto>>> GetPacienteDto()
        //{
        //    return await _context.PacienteDto.ToListAsync();
        //}

        //// GET: api/Paciente/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<PacienteDto>> GetPacienteDto(long id)
        //{
        //    var pacienteDto = await _context.PacienteDto.FindAsync(id);

        //    if (pacienteDto == null)
        //    {
        //        return NotFound();
        //    }

        //    return pacienteDto;
        //}

        //// PUT: api/Paciente/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPaciente(long id, PacienteDto pacienteDto)
        //{
        //    if (id != pacienteDto.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(pacienteDto).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PacienteExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Paciente
        //[HttpPost]
        //public async Task<ActionResult<PacienteDto>> PostPaciente(PacienteDto pacienteDto)
        //{
        //    _context.PacienteDto.Add(pacienteDto);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPacienteDto", new { id = pacienteDto.Id }, pacienteDto);
        //}

        //// DELETE: api/Paciente/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePaciente(long id)
        //{
        //    var pacienteDto = await _context.PacienteDto.FindAsync(id);
        //    if (pacienteDto == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.PacienteDto.Remove(pacienteDto);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


    }
}
