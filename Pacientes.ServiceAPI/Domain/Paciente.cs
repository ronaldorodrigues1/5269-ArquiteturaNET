using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pacientes.ServiceAPI.Dto;

namespace Pacientes.ServiceAPI.Domain
{
    [Table("pacientes")]
    public class Paciente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, StringLength(150)]
        public string Nome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CPF deve conter apenas números")]
        public string Telefone { get; set; }

        [Required, StringLength(20)]
        public string CPF { get; set; }

        public Paciente() { }
        public Paciente(PacienteDto dados)
        {
            AtualizarDados(dados);
        }

        public void AtualizarDados(PacienteDto dados)
        {
            Nome = dados.Nome;
            Email = dados.Email;
            Telefone = dados.Telefone;
            CPF = dados.CPF;
        }


    }
}
