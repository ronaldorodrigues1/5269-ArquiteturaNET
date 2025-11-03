using VollMed.Web.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VollMed.Web.Domain
{

    public class Consulta
    {

        public long Id { get; private set; }

        public string Paciente { get; private set; }

        public long MedicoId { get; private set; }

        public DateTime Data { get; private set; }




    }
}


