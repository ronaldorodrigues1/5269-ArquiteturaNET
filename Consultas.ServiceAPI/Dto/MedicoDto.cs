using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace Consultas.ServiceAPI.Dto
{
    public class MedicoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Crm { get; set; }
        public string Telefone { get; set; }
        public int Especialidade { get; set; }

    }
}


//{
//    "id": 1,
//  "_method": null,
//  "nome": "Dr. João Silva",
//  "email": "jsilva@email.com",
//  "crm": "12345-SP",
//  "telefone": "(11)12345-6781",
//  "especialidade": 1
//}