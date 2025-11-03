using Consultas.ServiceAPI.Dto;
using Consultas.ServiceAPI.Interfaces;
using System.Net.Http.Json;

namespace Consultas.ServiceAPI.Services
{
    public class MedicoService : IMedicoService
    {

        private readonly IMedicoRepository _medicoRepository;
        private const int PageSize = 5;

        public MedicoService(IMedicoRepository medicoRepository)
        {
            
            _medicoRepository = medicoRepository;
        }
        public async Task<IEnumerable<MedicoDto>> ListarTodosAsync()
        {
            return await _medicoRepository.GetAll();
        }

        public async Task<MedicoDto?> ObterPorIdAsync(long id)
        {
            return await _medicoRepository.FindByIdAsync(id);
        }
    }
}
