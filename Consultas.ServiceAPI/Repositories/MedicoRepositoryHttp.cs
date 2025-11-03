using Consultas.ServiceAPI.Dto;
using Consultas.ServiceAPI.Interfaces;

namespace Consultas.ServiceAPI.Repositories
{
    public class MedicoRepositoryHttp : IMedicoRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public MedicoRepositoryHttp(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<MedicoDto>> GetAll()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                var url = $"{_configuration["Services:Medicos"]}/api/medico/listar";

                var ret = await _httpClient.GetFromJsonAsync<IEnumerable<MedicoDto>>(url);
                return ret ?? new List<MedicoDto>();
            }
            catch (Exception ex)
            {
                // Aqui você pode logar o erro, se quiser:
                Console.WriteLine($"Erro ao obter médicos: {ex.Message}");
                return new List<MedicoDto>();
            }
        }


        public async Task<MedicoDto?> FindByIdAsync(long id)
        {
            try
            {

                var url = $"{_configuration["Services:Medicos"]}/api/medico/formulario/{id}";
                var ret = await _httpClient.GetFromJsonAsync<MedicoDto>(url);
                return ret;
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao obter médico:{ ex.Message}");
                return null;
            }
        }
    }
}
