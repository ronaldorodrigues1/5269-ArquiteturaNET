using VollMed.Web.Dtos;
using VollMed.Web.Interfaces;
using VollMed.Web.Domain;
using System.Net.Http;
using System.Text.Json;

namespace VollMed.Web.Services
{
    public class VollMedApiService : IVollMedApiService
    {
        private readonly HttpClient _httpClientConsultas;
        private readonly HttpClient _httpClientMedicos;
        private readonly HttpClient _httpClientPacientes;
        private readonly IConfiguration _configuration;
        private HttpContext _httpContext;

        public string Scope => throw new NotImplementedException();

        public VollMedApiService(
            IConfiguration configuration,
            HttpClient httpClientConsultas,
            HttpClient httpClientMedicos,
            HttpClient httpClientPacientes
            )
        {
            _configuration = configuration;
            _httpClientConsultas = httpClientConsultas;
            _httpClientMedicos = httpClientMedicos;
            _httpClientPacientes = httpClientPacientes;
        }

        public IVollMedApiService WithContext(HttpContext context)
        {
            _httpContext = context;
            return this;
        }

        #region Consulta
        public async Task<PaginatedList<ConsultaDto>> ListarConsultas(int? page)
        {
            var url = $"/api/consulta/listar?page={page ?? 1}";
            var list = await _httpClientConsultas.GetFromJsonAsync<PaginatedList<ConsultaDto>>(url)
                   ?? new PaginatedList<ConsultaDto>(new List<ConsultaDto>(), 0, page ?? 1, 10);
            return list;
        }

        public async Task<FormularioConsultaDto> ObterFormularioConsulta(long? consultaId)
        {
            var url = $"/api/consulta/formulario/{consultaId}";
            return await _httpClientConsultas.GetFromJsonAsync<FormularioConsultaDto>(url) ?? new FormularioConsultaDto();
        }

        public async Task<ConsultaDto> SalvarConsulta(ConsultaDto input)
        {
            if (string.IsNullOrEmpty(input.PacienteCpf))
                throw new Exception("CPF do paciente é obrigatório!");

            var pacienteExists = await ObterPacientePorCpf(input.PacienteCpf);
            if (pacienteExists == null)
            {
                var newPaciente = new PacienteDto
                {
                    _method = "post",
                    Id = input.PacienteId,
                    Nome = input.PacienteNome,
                    Email = $"{input.PacienteCpf}@email.com",
                    Telefone = "(00)00000-0000",
                    Cpf = input.PacienteCpf
                };


                var paciente = await SalvarPaciente(newPaciente);
                input.PacienteId = paciente.Id;
                input.PacienteNome = paciente.Nome;
                input.PacienteCpf = paciente.Cpf;

            }
                //throw new Exception("Paciente não encontrado na base.");

            var url = "/api/consulta/salvar";
            var response = await _httpClientConsultas.PostAsJsonAsync(url, input);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ConsultaDto>();
        }

        public async Task<ReceitaResultadoOperacaoDto> GerarReceita(ReceitaDto input)
        {
            var url = $"/api/consulta/gerarreceita";

            var response = await _httpClientConsultas.PostAsJsonAsync(url, input);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao salvar e gerar receita: {erro}");
            }

            var resultado = await response.Content.ReadFromJsonAsync<ReceitaResultadoOperacaoDto>();

            return resultado ?? new ReceitaResultadoOperacaoDto
            {
                Sucesso = false,
                Mensagem = "Não foi possível interpretar o retorno da API."
            };
        }

        public Task ExcluirConsulta(long consultaId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Medico
        public async Task<MedicoDto> ObterFormularioMedico(long? medicoId)
        {
            var url = $"/api/medico/formulario/{medicoId}";
            var response = await _httpClientMedicos.GetFromJsonAsync<MedicoDto>(url);
            return await _httpClientMedicos.GetFromJsonAsync<MedicoDto>(url) ?? new MedicoDto();
        }

        public async Task<IEnumerable<MedicoDto>> ListarMedicosPorEspecialidade(Especialidade especEnum)
        {
            var url = $"/api/medico/especialidade/{(int)especEnum}";
            return await _httpClientMedicos.GetFromJsonAsync<IEnumerable<MedicoDto>>(url)
                   ?? Enumerable.Empty<MedicoDto>();
        }

        public async Task<PaginatedList<MedicoDto>> ListarMedicos(int? page)
        {
            try
            {
                var url = $"/api/medico/listar?page={page ?? 1}";
                Console.WriteLine($"Chamando URL: {_httpClientMedicos.BaseAddress}{url}");
                return await _httpClientMedicos.GetFromJsonAsync<PaginatedList<MedicoDto>>(url)
                       ?? new PaginatedList<MedicoDto>(new List<MedicoDto>(), 0, page ?? 1, 10);
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return new PaginatedList<MedicoDto>();
            }
        }

        public async Task<MedicoDto> SalvarMedico(MedicoDto input)
        {
            var url = "/api/medico/salvar";
            var response = await _httpClientMedicos.PostAsJsonAsync(url, input);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MedicoDto>();
        }

        public Task ExcluirMedico(long medicoId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Paciente 
        public async Task<PacienteDto> ObterPacientePorCpf(string pacienteCpf)
        {
            var url = $"/api/Paciente/formularioporcpf/{pacienteCpf}";
            var response = await _httpClientPacientes.GetAsync(url);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<PacienteDto>();
            return null;
        }


        public async Task<PacienteDto> SalvarPaciente(PacienteDto input)
        {
            // Ajuste a URL conforme seu BaseAddress:
            // se BaseAddress = https://localhost:7218/api -> usar "Paciente/salvar"
            // se BaseAddress = https://localhost:7218     -> usar "api/Paciente/salvar"
            var url = $"/api/Paciente/salvar";

            // Log do JSON enviado
            var json = JsonSerializer.Serialize(input);
            Console.WriteLine($"[SalvarPaciente] Enviando para {new Uri(_httpClientPacientes.BaseAddress, url)}:\n{json}");

            var response = await _httpClientPacientes.PostAsJsonAsync(url, input);

            Console.WriteLine($"[SalvarPaciente] StatusCode: {response.StatusCode}");
            var respText = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[SalvarPaciente] Response content: {respText}");

            if (!response.IsSuccessStatusCode)
            {
                // Lança com detalhe para você ver no caller
                throw new Exception($"Erro ao salvar paciente. Status: {response.StatusCode}. Conteúdo: {respText}");
            }

            return await response.Content.ReadFromJsonAsync<PacienteDto>();
        }




        public Task ExcluirPaciente(long pacienteId)
        {
            throw new NotImplementedException();
        }




        #endregion
    }
}
