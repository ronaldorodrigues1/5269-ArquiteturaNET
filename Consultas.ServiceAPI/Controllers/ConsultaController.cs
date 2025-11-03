using Consultas.ServiceAPI.Dto;
using Consultas.ServiceAPI.Exceptions;
using Consultas.ServiceAPI.Interfaces;
using Consultas.ServiceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Consultas.ServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultaController : ControllerBase
    {
        private readonly IConsultaService _consultaservice;
        private readonly IMedicoService _medicoService;
     
        public ConsultaController(IConsultaService consultaService, IMedicoService medicoService)
        {
            _consultaservice = consultaService;
            _medicoService = medicoService;

        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarAsync([FromQuery] int page = 1)
        {
            PaginatedList<ConsultaDto> consultas = await _consultaservice.ListarAsync(page);
            return Ok(consultas);
        }

        [HttpGet("formulario/{id?}")]
        public async Task<IActionResult> ObterFormularioAsync(long id = 0)
        {
            var dados = id > 0
                ? await _consultaservice.CarregarPorIdAsync(id)
                : new ConsultaDto { Data = DateTime.Now };

            //Aqui o Microsserviço de Medico está diretamente ligado ao microsserviço de consulta
            //Isso causa uma dependência forte entre os microsserviços (APIs), se uma cai a outra para de funcionar
            var medicos = await _medicoService.ListarTodosAsync();

            var formularioConsulta = new FormularioConsultaDto
            {
                Consulta = dados,
                Medicos = medicos
            };
            return Ok(formularioConsulta);
        }

        [HttpPut("Salvar")]
        [HttpPost("Salvar")]
        public async Task<IActionResult> SalvarAsync([FromBody] ConsultaDto dados)
        {
            try
            {
                await _consultaservice.CadastrarAsync(dados);
                return Ok(dados);
            }
            catch (RegraDeNegocioException ex)
            {
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }

        [HttpDelete("Excluir/{id}")]
        public async Task<IActionResult> ExcluirAsync(int id)
        {
            await _consultaservice.ExcluirAsync(id);
            return Ok();
        }

        [HttpGet("listarReceitasporconsulta")]
        public async Task<IActionResult> ListarReceitasByConsultaIdAsync([FromQuery]long consultaId, int page = 1)
        {
            PaginatedList<ReceitaDto> receita = await _consultaservice.ListarReceitasByReceitaIdAsync(consultaId, page);
            return Ok(receita);
        }

        [HttpPost("EnviarMessageRabbitTeste")]
        public async Task<IActionResult> EnviarMessageRabbitAsync([FromBody] ReceitaDto dados)
        {
            var ret = await _consultaservice.PublicarMessageRabbitTeste(dados);
            return Ok(ret);
        }

    }
}
