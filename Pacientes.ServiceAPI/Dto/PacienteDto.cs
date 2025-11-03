using Pacientes.ServiceAPI.Domain;
using System.ComponentModel.DataAnnotations;

namespace Pacientes.ServiceAPI.Dto
{
    [Serializable]
    public class PacienteDto
    {
        public PacienteDto()
        {
        }

        public PacienteDto(Paciente paciente)
        {
            Id = paciente.Id;
            Nome = paciente.Nome;
            Email = paciente.Email;
            Telefone = paciente.Telefone;
            CPF = paciente.CPF.ToString();
        }

        public long Id { get; set; }
        public string _method { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório"), MinLength(5, ErrorMessage = "Campo deve ter no mínimo 5 caracteres")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório"), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 dígitos numéricos")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CPF deve conter apenas números")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Campo obrigatório"), RegularExpression(@"^(?:\d{8}|\d{9}|\d{4}-\d{4}|\d{5}-\d{4}|\(\d{2}\)\s*\d{4}-\d{4}|\(\d{2}\)\s*\d{5}-\d{4}|\(\d{2}\)\s*\d{9})$",
                  ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; }
    }
}
