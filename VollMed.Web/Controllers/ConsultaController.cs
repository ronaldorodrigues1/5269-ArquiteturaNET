using VollMed.Web.Dtos;
using VollMed.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VollMed.Web.Controllers
{
    [Route("consultas")]
    public class ConsultaController : BaseController
    {
        private const string PaginaListagem = "Listagem";
        private const string PaginaCadastro = "Formulario";

        private readonly IVollMedApiService _vollMedApiService;

        public ConsultaController(IVollMedApiService vollMedApiService)
        : base()
        {
            _vollMedApiService = vollMedApiService;
        }

        [HttpGet]
        [Route("{page?}")]
        public async Task<IActionResult> ListarAsync([FromQuery] int page = 1)
        {
            PaginatedList<ConsultaDto> consultas = await _vollMedApiService.ListarConsultas(page);
            
            ViewBag.Consultas = consultas;
            ViewData["Url"] = "Consultas";
            return View(PaginaListagem, consultas);
        }

        [HttpGet]
        [Route("formulario/{id?}")]
        public async Task<IActionResult> ObterFormularioAsync(long id = 0)
        {
            FormularioConsultaDto formularioConsulta = await _vollMedApiService.ObterFormularioConsulta(id);
            ViewData["Medicos"] = formularioConsulta.Medicos;
            return View(PaginaCadastro, formularioConsulta.Consulta);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SalvarAsync([FromForm] ConsultaDto dados)
        {
            if (dados._method == "delete")
            {
                await _vollMedApiService.ExcluirConsulta(dados.Id);
                return RedirectToAction("index");
            }

            if (!ModelState.IsValid)
            {
                PaginatedList<MedicoDto> medicos = await _vollMedApiService.ListarMedicos(1);
                ViewData["Medicos"] = medicos.Items;
                return View(PaginaCadastro, dados);
            }

            try
            {
                await _vollMedApiService.SalvarConsulta(dados);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                ViewBag.Dados = dados;
                return View(PaginaCadastro);
            }
        }

        [HttpGet]
        [Route("formularioatendimento/{id?}")]
        public async Task<IActionResult> FormularioAtendimento(long id)
        {

            var formularioConsulta = await _vollMedApiService.ObterFormularioConsulta(id);
            var receita = new ReceitaDto(formularioConsulta.Consulta);

            return View(receita);
        }

        [HttpPost]
        [Route("salvarreceita")]
        public async Task<IActionResult> SalvarReceita([FromForm] ReceitaDto dados)
        {

            if (!ModelState.IsValid)
            {

                return View("formularioatendimento", dados);
            }

            try
            {
                await _vollMedApiService.GerarReceita(dados);
                return RedirectToAction("/listarconsultas");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                ViewBag.Dados = dados;
                return RedirectToAction("formularioatendimento", dados.Id);
            }
        }

    }
}
