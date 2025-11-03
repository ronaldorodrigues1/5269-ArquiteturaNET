namespace VollMed.Messages
{
    public record ReceitaGeradaEvent
    (
        Guid ConsultaId,
        string MedicoNome,
        string PacienteNome,
        string PacienteEmail,
        string Data,
        string DescricaoReceita
    );
    
}
