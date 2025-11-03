using Pacientes.ServiceAPI.Dto;

namespace Pacientes.ServiceAPI.Interfaces
{
    public interface IPacienteService
    {
        Task<PacienteDto> CadastrarAsync(PacienteDto dados);
        Task<PacienteDto> CarregarPorIdAsync(long id);
        Task<PacienteDto> CarregarPorCpfAsync(string cpf);
        Task ExcluirAsync(long id);
        Task<PaginatedList<PacienteDto>> ListarAsync(int? page);
        IEnumerable<PacienteDto> ListarTodos();

    }
}
