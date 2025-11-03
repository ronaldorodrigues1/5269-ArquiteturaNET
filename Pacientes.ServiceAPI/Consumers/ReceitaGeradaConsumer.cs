using MassTransit;
using VollMed.Messages;

namespace Pacientes.ServiceAPI.Consumers
{
    public class ReceitaGeradaConsumer : IConsumer<ReceitaGeradaEvent>
    {
        public async Task Consume(ConsumeContext<ReceitaGeradaEvent> context)
        {
            var evento = context.Message;

            Console.WriteLine($"[Pacientes.ServiceAPI] Receita recebida:");
            Console.WriteLine($"..Enviando e-mail ao paciente:");
            Console.WriteLine($"....Paciente: {evento.PacienteNome}");
            Console.WriteLine($"....E-mail: {evento.PacienteEmail}");
            Console.WriteLine($"....Descrição da Receita: {evento.DescricaoReceita}");
            Console.WriteLine($"....Data: {evento.Data}");
            Console.WriteLine($"....Médico: {evento.MedicoNome}");

            // Aqui você pode enviar o e-mail real, mas por enquanto só simule
            await Task.CompletedTask;
        }
    }
}
