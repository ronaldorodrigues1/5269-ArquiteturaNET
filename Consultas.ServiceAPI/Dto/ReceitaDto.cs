using Consultas.ServiceAPI.Domain;

namespace Consultas.ServiceAPI.Dto
{
    public class ReceitaDto
    {
        public long Id { get; set; }
        public string _method { get; set; }

        public long ConsultaId { get; set; }
        //public Consulta Consulta { get; set; }

        public long PacienteId { get; set; }
        public string? PacienteNome { get; set; }
        public string? PacienteEmail { get; set; }
        public long MedicoId { get; set; }
        public string? MedicoNome { get; set; }
        public string? CRM { get; set; }
        public DateTime DataReceita { get; set; }
        public string? Descricao { get; set; }

        public ReceitaDto() { }
        public ReceitaDto(Receita dados)
        {
            this.Id = dados.Id;

            this.ConsultaId = dados.ConsultaId;
            //this.Consulta = dados.Consulta;
            this.PacienteId = dados.PacienteId;
            this.PacienteNome = dados.PacienteNome;
            this.MedicoId = dados.MedicoId;
            this.MedicoNome = dados.MedicoNome;
            this.CRM = dados.CRM;
            this.DataReceita = dados.DataReceita;
            this.Descricao = dados.Descricao;
        }
    }

    public class ReceitaResultadoOperacaoDto
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
    }
}
