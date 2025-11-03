using Consultas.ServiceAPI.Dto;

namespace Consultas.ServiceAPI.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<PacienteDto>> ListarTodosAsync();
        Task<PacienteDto?> ObterPorIdAsync(long id);
    }
}
