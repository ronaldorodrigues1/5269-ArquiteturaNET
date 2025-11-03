
using Pacientes.ServiceAPI.Interfaces;
using Pacientes.ServiceAPI.Dto;
using Pacientes.ServiceAPI.Domain;
using Pacientes.ServiceAPI.Exceptions;

namespace pacientes.ServiceAPI.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;
        private const int PageSize = 5;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<PacienteDto>> ListarAsync(int? page)
        {
            var pacientes = _repository.GetAll();
            IQueryable<PacienteDto> dtos = pacientes.Select(m => new PacienteDto(m));
            return await PaginatedList<PacienteDto>.CreateAsync(dtos, page ?? 1, PageSize);
        }

        public IEnumerable<PacienteDto> ListarTodos()
        {
            var pacientes = _repository.GetAll();
            return pacientes.Select(m => new PacienteDto(m)).AsEnumerable();
        }

        public async Task<PacienteDto> CadastrarAsync(PacienteDto dados)
        {
            if (await _repository.IsJaCadastradoAsync(dados.Email, dados.CPF, dados.Id))
                throw new RegraDeNegocioException("E-mail ou CPF já cadastrado para outro paciente!");

            Paciente paciente;

            if (dados.Id == 0)
            {
                paciente = new Paciente(dados);
                await _repository.InsertAsync(paciente);
            }
            else
            {
                paciente = await _repository.FindByIdAsync(dados.Id);
                if (paciente == null)
                    throw new RegraDeNegocioException("Paciente não encontrado.");

                paciente.AtualizarDados(dados);
                await _repository.UpdateAsync(paciente);
            }

            // Caso o repositório não atualize automaticamente o estado do objeto:
            paciente = await _repository.FindByIdAsync(paciente.Id);

            return new PacienteDto(paciente);
        }

        public async Task<PacienteDto> CarregarPorIdAsync(long id)
        {
            var paciente = await _repository.FindByIdAsync(id);
            if (paciente == null) throw new RegraDeNegocioException("Paciente não encontrado.");

            return new PacienteDto(paciente);
        }

        public async Task ExcluirAsync(long id)
        {
            await _repository.DeleteByIdAsync(id);
        }

        public async Task<PacienteDto> CarregarPorCpfAsync(string cpf)
        {
            var paciente = await _repository.FindByCpfAsync(cpf);
            if (paciente == null) throw new RegraDeNegocioException("Paciente não encontrado.");

            return new PacienteDto(paciente);
        }
    }
}
