namespace Pacientes.ServiceAPI.Exceptions
{
    public class RegraDeNegocioException : ApplicationException
    {
        public RegraDeNegocioException(string? message) : base(message)
        {
        }
    }
}
