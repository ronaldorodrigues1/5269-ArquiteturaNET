using Consultas.ServiceAPI.Domain;

namespace Consultas.ServiceAPI.Interfaces
{
    public interface IConsultaRepository
    {
        IQueryable<Consulta> GetAllOrderedByData();
        Task SaveAsync(Consulta consulta);
        Task<Consulta> FindByIdAsync(long id);
        Task DeleteByIdAsync(long id);
        Task UpdateAsync(Consulta consulta);

        Task SaveReceitaAsync(Receita receita);
        Task<Receita> FindReceitaByIdAsync(long id);
        IQueryable<Receita> GetAllReceitaByConsultaId(long consultaId);
    }
}


