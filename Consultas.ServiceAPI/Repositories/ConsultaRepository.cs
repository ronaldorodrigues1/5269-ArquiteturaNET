
using Consultas.ServiceAPI.Data;
using Consultas.ServiceAPI.Domain;
using Consultas.ServiceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Consultas.ServiceAPI.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly ConsultasDbContext _context;

        public ConsultaRepository(ConsultasDbContext context)
        {
            _context = context;
        }

        public IQueryable<Consulta> GetAllOrderedByData()
        {
            //return _context.Consultas.Include(c => c.Medico).OrderBy(c => c.Data);
            return _context.Consultas.OrderBy(c => c.Data);
        }

        public async Task SaveAsync(Consulta consulta)
        {
            _context.Add(consulta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Consulta consulta)
        {
            _context.Update(consulta);
            await _context.SaveChangesAsync();
        }

        public async Task<Consulta> FindByIdAsync(long id)
        {
            //return await _context.Consultas.Include(c => c.Medico).SingleAsync(c => c.Id == id);
            return await _context.Consultas.SingleAsync(c => c.Id == id);
        }

        public async Task DeleteByIdAsync(long id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta != null)
            {
                _context.Consultas.Remove(consulta);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveReceitaAsync(Receita receita)
        {
            _context.Add(receita);
            await _context.SaveChangesAsync();
        }

        public async Task<Receita> FindReceitaByIdAsync(long id)
        {
            return await _context.Receitas.SingleAsync(c => c.Id == id);
        }
        public IQueryable<Receita> GetAllReceitaByConsultaId(long consultaId)
        {
            return _context.Receitas.Include(c => c.Consulta).Where(c => c.ConsultaId == consultaId);
        }
    }
}


