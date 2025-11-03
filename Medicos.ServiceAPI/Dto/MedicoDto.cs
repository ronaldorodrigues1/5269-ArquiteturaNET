using Medicos.ServiceAPI.Domain;
using System.ComponentModel.DataAnnotations;

namespace Medicos.ServiceAPI.Dto
{
    public class MedicoDto
    {

        public long Id { get; set; }
        public string _method { get; set; }
        [Required(ErrorMessage = "Campo obrigatório"), MinLength(1)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo obrigatório"), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [RegularExpression(@"^[0-9]{4,8}(-[A-Za-z]{1,3})?$",
            ErrorMessage = "CRM deve ter entre 4 e 8 dígitos numéricos, podendo conter sufixo de letras (ex: 12345-SP)")]
        public string Crm { get; set; }
        [Required(ErrorMessage = "Campo obrigatório"), RegularExpression(@"^(?:\d{8}|\d{9}|\d{4}-\d{4}|\d{5}-\d{4}|\(\d{2}\)\s*\d{4}-\d{4}|\(\d{2}\)\s*\d{5}-\d{4}|\(\d{2}\)\s*\d{9})$",
            ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public Especialidade Especialidade { get; set; }

        public MedicoDto()
        {
        }

        public MedicoDto(Medico medico)
        {
            Id = medico.Id;
            Nome = medico.Nome;
            Email = medico.Email;
            Telefone = medico.Telefone;
            Crm = medico.Crm;
            Especialidade = medico.Especialidade;
        }
    }
}
