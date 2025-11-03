using Consultas.ServiceAPI.Dto;

namespace Consultas.ServiceAPI.Interfaces
{
    public interface IMedicoRepository
    {
        Task<IEnumerable<MedicoDto>> GetAll();
        Task<MedicoDto> FindByIdAsync(long Id);
    } 
}
