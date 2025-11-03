using Consultas.ServiceAPI.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consultas.ServiceAPI.Domain
{
    public class Receita
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ConsultaId { get; set; }
        public Consulta Consulta { get; set; }

        public long PacienteId { get; set; }
        public string? PacienteNome { get; set; }
        public long MedicoId { get; set; }
        public string? MedicoNome { get; set; }
        public string? CRM { get; set; }
        public DateTime DataReceita { get; set; }
        public string? Descricao { get; set; }

        public Receita() { }
        public Receita(ReceitaDto dados)
        {
            ModificarDados(dados);
        }

        private void ModificarDados(ReceitaDto dados)
        {
            ConsultaId = dados.ConsultaId;

            PacienteId = dados.PacienteId;
            PacienteNome = dados.PacienteNome;
            MedicoId = dados.MedicoId;
            MedicoNome = dados.MedicoNome;
            CRM = dados.CRM;

            DataReceita = dados.DataReceita;
            Descricao = dados.Descricao;
        }
    }
}


