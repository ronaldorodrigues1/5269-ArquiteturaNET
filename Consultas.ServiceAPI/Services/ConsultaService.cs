
using Consultas.ServiceAPI.Domain;
using Consultas.ServiceAPI.Dto;
using Consultas.ServiceAPI.Exceptions;
using Consultas.ServiceAPI.Interfaces;
using MassTransit;
using VollMed.Messages;


namespace Consultas.ServiceAPI.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IMedicoRepository _medicoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private const int PageSize = 5;

        public ConsultaService(IConsultaRepository consultaRepository, IMedicoRepository medicoRepository, IPacienteRepository pacienteRepository, IPublishEndpoint publishEndpoint)
        {
            _consultaRepository = consultaRepository;
            _medicoRepository = medicoRepository;
            _pacienteRepository = pacienteRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<PaginatedList<ConsultaDto>> ListarAsync(int? page)
        {
            var consultas = _consultaRepository.GetAllOrderedByData();
            IQueryable<ConsultaDto> dtos = consultas.Select(m => new ConsultaDto(m));
            return await PaginatedList<ConsultaDto>.CreateAsync(dtos, page ?? 1, PageSize);
        }

        public async Task CadastrarAsync(ConsultaDto dados)
        {
            //Aqui o Microsserviço de Medico está diretamente ligado ao microsserviço de consulta
            //Isso causa uma dependência forte entre os microsserviços (APIs), se uma cai a outra para de funcionar
            var medicoConsulta = await _medicoRepository.FindByIdAsync(dados.MedicoId);
            if (medicoConsulta == null)
            {
                throw new RegraDeNegocioException("Erro ao obter dados do médico.");
            }
            else
            {
                dados.MedicoId = medicoConsulta.Id;
                dados.MedicoNome = medicoConsulta.Nome;
            }

            var pacienteConsulta = await _pacienteRepository.FindByIdAsync(dados.PacienteId);
            if (pacienteConsulta == null)
            {
                throw new RegraDeNegocioException("Erro ao obter dados do paciente.");
            }
            else
            {
                dados.PacienteId = pacienteConsulta.Id;
                dados.PacienteNome = pacienteConsulta.Nome;
            }


            if (dados.Id == 0)
            {
                var consulta = new Consulta(dados);
                await _consultaRepository.SaveAsync(consulta);
            }
            else
            {
                var consulta = await _consultaRepository.FindByIdAsync(dados.Id);
                if (consulta == null) throw new RegraDeNegocioException("Consulta não encontrada.");

                consulta.ModificarDados(dados);
                await _consultaRepository.UpdateAsync(consulta);
            }
        }

        public async Task<ConsultaDto> CarregarPorIdAsync(long id)
        {
            var consulta = await _consultaRepository.FindByIdAsync(id);
            if (consulta == null) throw new RegraDeNegocioException("Consulta não encontrada.");

            //Aqui o Microsserviço de Medico está diretamente ligado ao microsserviço de consulta
            //Isso causa uma dependência forte entre os microsserviços (APIs), se uma cai a outra para de funcionar
            var medicoConsulta = await _medicoRepository.FindByIdAsync(consulta.MedicoId);
            
            return new ConsultaDto(consulta.Id, consulta.MedicoId, consulta.MedicoNome, consulta.PacienteId, consulta.PacienteNome, consulta.PacienteCpf, consulta.Data, (Especialidade)medicoConsulta!.Especialidade);
        }

        public async Task ExcluirAsync(long id)
        {
            await _consultaRepository.DeleteByIdAsync(id);
        }

        public async Task<PaginatedList<ReceitaDto>> ListarReceitasByReceitaIdAsync(long consultaId, int? page)
        {
            var receitas = _consultaRepository.GetAllReceitaByConsultaId(consultaId);
            IQueryable<ReceitaDto> dtos = receitas.Select(c => new ReceitaDto(c));
            return await PaginatedList<ReceitaDto>.CreateAsync(dtos, page ?? 1, PageSize);
        }

        public async Task<ReceitaResultadoOperacaoDto> GerarReceitaAsync(ReceitaDto dados)
        {
            if (dados.Id == 0)
            {
                // 1️⃣ Cria e salva a receita
                var receita = new Receita(dados);
                await _consultaRepository.SaveReceitaAsync(receita);

                // 2️⃣ Tenta publicar o evento de forma segura
                try
                {
                    await PublicarEventoReceitaAsync(dados);
                    return new ReceitaResultadoOperacaoDto
                    {
                        Sucesso = true,
                        Mensagem = "Receita salva e evento publicado com sucesso."
                    };
                }
                catch (Exception ex)
                {
                    // Loga o erro mas não interrompe o retorno
                    Console.WriteLine($"[RabbitMQ] Falha ao publicar evento: {ex.Message}");
                    return new ReceitaResultadoOperacaoDto
                    {
                        Sucesso = true,
                        Mensagem = "Receita salva, mas houve falha ao publicar no RabbitMQ."
                    };
                }
            }

            return new ReceitaResultadoOperacaoDto
            {
                Sucesso = false,
                Mensagem = "Receita já existente, nenhuma ação executada."
            };
        }

        public async Task<ReceitaResultadoOperacaoDto> PublicarMessageRabbitTeste(ReceitaDto dados)
        {
            try
            {
                await PublicarEventoReceitaAsync(dados);

            }
            catch (Exception ex)
            {
                // Loga o erro mas não interrompe o retorno
                Console.WriteLine($"[RabbitMQ] Falha ao publicar evento: {ex.Message}");
                return new ReceitaResultadoOperacaoDto
                {
                    Sucesso = true,
                    Mensagem = "Receita salva, mas houve falha ao publicar no RabbitMQ."
                };
            }
            return new ReceitaResultadoOperacaoDto
            {
                Sucesso = false,
                Mensagem = "Receita já existente, nenhuma ação executada."
            };
        }

        private async Task PublicarEventoReceitaAsync(ReceitaDto dados)
        {
            var evento = new ReceitaGeradaEvent(
                Guid.NewGuid(),
                dados.MedicoNome,
                dados.PacienteNome,
                dados.PacienteEmail,
                dados.DataReceita.ToShortDateString(),
                dados.Descricao
            );

            await _publishEndpoint.Publish(evento);

            Console.WriteLine($"[RabbitMQ] Evento publicado para o paciente {dados.PacienteNome}");
        }



    }
}


