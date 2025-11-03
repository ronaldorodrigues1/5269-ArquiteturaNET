using Consultas.ServiceAPI.Dto;
using Consultas.ServiceAPI.Interfaces;

namespace Consultas.ServiceAPI.Repositories
{
    public class PacienteRepositoryHttp : IPacienteRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PacienteRepositoryHttp(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<PacienteDto>> GetAll()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                var url = $"{_configuration["Services:Pacientes"]}/api/Paciente/listar";

                var ret = await _httpClient.GetFromJsonAsync<IEnumerable<PacienteDto>>(url);
                return ret ?? new List<PacienteDto>();
            }
            catch (Exception ex)
            {
                // Aqui você pode logar o erro, se quiser:
                Console.WriteLine($"Erro ao obter médicos: {ex.Message}");
                return new List<PacienteDto>();
            }
        }


        public async Task<PacienteDto?> FindByIdAsync(long id)
        {
            try
            {

                var url = $"{_configuration["Services:Pacientes"]}/api/Paciente/formulario/{id}";
                var ret = await _httpClient.GetFromJsonAsync<PacienteDto>(url);
                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter médico:{ex.Message}");
                return null;
            }
        }


    }
}
