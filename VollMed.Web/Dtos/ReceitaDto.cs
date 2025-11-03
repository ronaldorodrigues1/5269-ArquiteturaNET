using VollMed.Web.Models;

namespace VollMed.Web.Dtos
{
    public class ReceitaDto
    {
        public long Id { get; set; }
        public string _method { get; set; }

        public long ConsultaId { get; set; }

        public long PacienteId { get; set; }
        public string? PacienteNome { get; set; }
        public string PacienteCpf { get; set; }
        public string PacienteEmail { get; set; }
        public long MedicoId { get; set; }
        public string? MedicoNome { get; set; }
        public string? CRM { get; set; }
        public DateTime DataReceita { get; set; }
        public string? Descricao { get; set; }

        public ReceitaDto() { }
        public ReceitaDto(ConsultaDto dados)
        {
            //this.Id = dados.Id;

            this.ConsultaId = dados.Id;

            this.PacienteId = dados.PacienteId;
            this.PacienteNome = dados.PacienteNome;
            this.PacienteCpf = dados.PacienteCpf;
            this.MedicoId = dados.MedicoId;
            this.MedicoNome = dados.MedicoNome;
            this.CRM = string.Empty; // dados.CRM;
            this.DataReceita = dados.Data;
            this.Descricao = string.Empty;
        }
    }

    public class ReceitaResultadoOperacaoDto
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
    }
}
