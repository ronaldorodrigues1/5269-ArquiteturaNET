using VollMed.Web.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VollMed.Web.Domain
{

    public class Medico
    {

        public long Id { get; private set; }

        public string Nome { get; private set; }

        public string Email { get; private set; }

        public string Telefone { get; private set; }

        public string Crm { get; private set; }

        public Especialidade Especialidade { get; private set; }

        public virtual ICollection<Consulta>? Consultas { get; set; }

    }
}


