namespace Consultas.ServiceAPI.Dto
{
    [Serializable]
    public class FormularioConsultaDto
    {
        public ConsultaDto Consulta { get; set; }

        public IEnumerable<MedicoDto> Medicos { get; set; }
    }
}