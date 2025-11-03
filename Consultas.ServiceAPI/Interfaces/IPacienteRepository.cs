using Consultas.ServiceAPI.Dto;

namespace Consultas.ServiceAPI.Interfaces
{
    public interface IPacienteRepository
    {
        Task<IEnumerable<PacienteDto>> GetAll();
        Task<PacienteDto> FindByIdAsync(long Id);

    }
}
