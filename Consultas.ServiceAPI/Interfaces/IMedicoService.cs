using Consultas.ServiceAPI.Dto;

namespace Consultas.ServiceAPI.Interfaces
{
    public interface IMedicoService
    {
        Task<IEnumerable<MedicoDto>> ListarTodosAsync();
        Task<MedicoDto?> ObterPorIdAsync(long id);
    }
}
