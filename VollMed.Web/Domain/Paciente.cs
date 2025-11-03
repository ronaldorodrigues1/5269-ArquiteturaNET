using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VollMed.Web.Domain
{
    public class Paciente
    {
        public long Id { get; private set; }

        public string? Nome { get; private set; }
        public string? Cpf { get; private set; }
        public string? Email { get; private set; }

        public string? Telefone { get; private set; }
    }
}
