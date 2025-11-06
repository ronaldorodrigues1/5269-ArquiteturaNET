using VollMed.Web.Dtos;
using VollMed.Web.Interfaces;
using VollMed.Web.Domain;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace VollMed.Web.Services
{
    public class VollMedApiService : IVollMedApiService
    {
        private readonly HttpClient _httpClientGateway;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string Scope => throw new NotImplementedException();

        public VollMedApiService(HttpClient httpGateway, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientGateway = httpGateway;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task AddBearerTokenAsync()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClientGateway.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        // -------------------------------------------------------------------
        // CONSULTA
        // -------------------------------------------------------------------
        #region Consulta

        public async Task<PaginatedList<ConsultaDto>> ListarConsultas(int? page)
        {
            await AddBearerTokenAsync();
            var url = $"{_httpClientGateway.BaseAddress}/consulta/listar?page={page ?? 1}";
            Console.WriteLine($"[ListarConsultas] Chamando: {url}");

            var list = await _httpClientGateway.GetFromJsonAsync<PaginatedList<ConsultaDto>>(url)
                       ?? new PaginatedList<ConsultaDto>(new List<ConsultaDto>(), 0, page ?? 1, 10);

            return list;
        }

        public async Task<FormularioConsultaDto> ObterFormularioConsulta(long? consultaId)
        {
            await AddBearerTokenAsync();
            var url = $"{_httpClientGateway.BaseAddress}/consulta/formulario/{consultaId}";
            Console.WriteLine($"[ObterFormularioConsulta] {url}");

            return await _httpClientGateway.GetFromJsonAsync<FormularioConsultaDto>(url)
                   ?? new FormularioConsultaDto();
        }

        public async Task<ConsultaDto> SalvarConsulta(ConsultaDto input)
        {
            if (string.IsNullOrEmpty(input.PacienteCpf))
                throw new Exception("CPF do paciente é obrigatório!");

            // Verifica se o paciente existe
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

            await AddBearerTokenAsync();
            var url = $"{_httpClientGateway.BaseAddress}/consulta/salvar";
            Console.WriteLine($"[SalvarConsulta] POST {url}");

            var response = await _httpClientGateway.PostAsJsonAsync(url, input);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ConsultaDto>();
        }

        public async Task<ReceitaResultadoOperacaoDto> GerarReceita(ReceitaDto input)
        {
            await AddBearerTokenAsync();
            var url = $"{_httpClientGateway.BaseAddress}/consulta/gerarreceita";
            Console.WriteLine($"[GerarReceita] POST {url}");

            var response = await _httpClientGateway.PostAsJsonAsync(url, input);

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

        // -------------------------------------------------------------------
        // MÉDICO
        // -------------------------------------------------------------------
        #region Medico

        public async Task<MedicoDto> ObterFormularioMedico(long? medicoId)
        {
            await AddBearerTokenAsync();
            var url = $"{_httpClientGateway.BaseAddress}/medico/formulario/{medicoId}";
            Console.WriteLine($"[ObterFormularioMedico] GET {url}");

            return await _httpClientGateway.GetFromJsonAsync<MedicoDto>(url)
                   ?? new MedicoDto();
        }

        public async Task<IEnumerable<MedicoDto>> ListarMedicosPorEspecialidade(Especialidade especEnum)
        {
            await AddBearerTokenAsync();
            var url = $"{_httpClientGateway.BaseAddress}/medico/especialidade/{(int)especEnum}";
            Console.WriteLine($"[ListarMedicosPorEspecialidade] GET {url}");

            return await _httpClientGateway.GetFromJsonAsync<IEnumerable<MedicoDto>>(url)
                   ?? Enumerable.Empty<MedicoDto>();
        }

        public async Task<PaginatedList<MedicoDto>> ListarMedicos(int? page)
        {
            try
            {
                await AddBearerTokenAsync();
                var url = $"{_httpClientGateway.BaseAddress}/medico/listar?page={page ?? 1}";
                Console.WriteLine($"[ListarMedicos] GET {url}");

                var lista = await _httpClientGateway.GetFromJsonAsync<PaginatedList<MedicoDto>>(url);
                return lista ?? new PaginatedList<MedicoDto>(new List<MedicoDto>(), 0, page ?? 1, 10);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ListarMedicos] ERRO: {ex.Message}");
                return new PaginatedList<MedicoDto>();
            }
        }

        public async Task<MedicoDto> SalvarMedico(MedicoDto input)
        {
            await AddBearerTokenAsync();
            var url = $"{_httpClientGateway.BaseAddress}/medico/salvar";
            Console.WriteLine($"[SalvarMedico] POST {url}");

            var response = await _httpClientGateway.PostAsJsonAsync(url, input);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<MedicoDto>();
        }

        public Task ExcluirMedico(long medicoId)
        {
            throw new NotImplementedException();
        }

        #endregion

        // -------------------------------------------------------------------
        // PACIENTE
        // -------------------------------------------------------------------
        #region Paciente

        public async Task<PacienteDto> ObterPacientePorCpf(string pacienteCpf)
        {
            await AddBearerTokenAsync();
            var url = $"{_httpClientGateway.BaseAddress}/paciente/formularioporcpf/{pacienteCpf}";
            Console.WriteLine($"[ObterPacientePorCpf] GET {url}");

            var response = await _httpClientGateway.GetAsync(url);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<PacienteDto>();

            return null;
        }

        public async Task<PacienteDto> SalvarPaciente(PacienteDto input)
        {
            await AddBearerTokenAsync();
            var url = $"{_httpClientGateway.BaseAddress}/paciente/salvar";

            var json = JsonSerializer.Serialize(input);
            Console.WriteLine($"[SalvarPaciente] POST {url}");
            Console.WriteLine($"[SalvarPaciente] Payload:\n{json}");

            var response = await _httpClientGateway.PostAsJsonAsync(url, input);
            var respText = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"[SalvarPaciente] StatusCode: {response.StatusCode}");
            Console.WriteLine($"[SalvarPaciente] Response: {respText}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao salvar paciente. Status: {response.StatusCode}. Conteúdo: {respText}");

            return await response.Content.ReadFromJsonAsync<PacienteDto>();
        }

        public Task ExcluirPaciente(long pacienteId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
