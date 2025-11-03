using Consultas.ServiceAPI.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consultas.ServiceAPI.Domain
{
    [Table("consultas")]
    public class Consulta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }

        public long PacienteId { get; private set; }
        public string PacienteNome { get; private set; }
        public string PacienteCpf { get; private set; }

        public long MedicoId { get; private set; }
        public string MedicoNome { get; private set; }

        public DateTime Data { get; private set; }

        public ICollection<Receita> Receitas { get; set; }


        public Consulta() { }

        public Consulta(ConsultaDto dados)
        {
            ModificarDados(dados);
        }

        public void ModificarDados(ConsultaDto dados)
        {
            MedicoId = dados.MedicoId;
            MedicoNome = dados.MedicoNome;

            PacienteId = dados.PacienteId;
            PacienteNome = dados.PacienteNome;
            PacienteCpf = dados.PacienteCpf;
            
            Data = dados.Data;
        }
    }
}


