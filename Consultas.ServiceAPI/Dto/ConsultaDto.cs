using Consultas.ServiceAPI.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Consultas.ServiceAPI.Dto
{

    [Serializable]
    public class ConsultaDto
    {
        public ConsultaDto()
        {
        }

        public ConsultaDto(
            long Id,
            long MedicoId,
            string MedicoNome,

            long PacienteId,
            string PacienteNome,
            string PacienteCpf,
            
            DateTime Data,
            Especialidade Especialidade
        )
        {
            this.Id = Id;
            this.MedicoId = MedicoId;
            this.MedicoNome = MedicoNome;

            this.PacienteId = PacienteId;
            this.PacienteNome = PacienteNome;
            this.PacienteCpf = PacienteCpf!;
            this.Data = Data;
            this.Especialidade = Especialidade;
        }

        public ConsultaDto(Consulta consulta)
        {
            Id = consulta.Id;
            MedicoId = consulta.MedicoId;
            MedicoNome = consulta.MedicoNome;
            PacienteId = consulta.PacienteId;
            PacienteNome = consulta.PacienteNome;
            PacienteCpf = consulta.PacienteCpf;
            Data = consulta.Data;
        }

        public long Id { get; set; }
        public string _method { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")] 
        public long MedicoId { get; set; }

        [ValidateNever]
        public string MedicoNome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public long PacienteId { get; set; }

        [ValidateNever]
        public string PacienteNome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório"), StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 digitos")]
        public string PacienteCpf { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório"), DataType(DataType.DateTime)] 
        public DateTime Data { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório")] 
        public Especialidade Especialidade { get; set; }
    }

    public enum Especialidade
    {
        [Display(Name = "Cardiologia")]
        Cardiologia = 1,

        [Display(Name = "Neurocirurgia")]
        Neurocirurgia = 2,

        [Display(Name = "Cirurgia Geral")]
        CirurgiaGeral = 3,

        [Display(Name = "Pediatria")]
        Pediatria = 4,

        [Display(Name = "Oncologia")]
        Oncologia = 5,

        [Display(Name = "Diagnóstico")]
        Diagnostico = 6
    }
}


