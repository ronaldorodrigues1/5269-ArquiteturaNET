using Consultas.ServiceAPI.Dto;
using Consultas.ServiceAPI.Interfaces;

namespace Consultas.ServiceAPI.Services
{
    public class PacienteService : IPacienteService
    {

        private readonly IPacienteRepository _pacienteRepository;
        private const int PageSize = 5;

        public PacienteService(IPacienteRepository pacienteRepository)
        {

            _pacienteRepository = pacienteRepository;
        }
        public async Task<IEnumerable<PacienteDto>> ListarTodosAsync()
        {
            return await _pacienteRepository.GetAll();
        }

        public async Task<PacienteDto?> ObterPorIdAsync(long id)
        {
            return await _pacienteRepository.FindByIdAsync(id);
        }
    }
}
