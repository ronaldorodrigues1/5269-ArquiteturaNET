using Microsoft.EntityFrameworkCore;
using Pacientes.ServiceAPI.Data;
using Pacientes.ServiceAPI.Domain;
using Pacientes.ServiceAPI.Interfaces;

namespace Pacientes.ServiceAPI.Repository
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly PacientesDbContext _context;

        public PacienteRepository(PacientesDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsJaCadastradoAsync(string email, string cpf, long? id)
        {
            return await _context.Pacientes
                .AnyAsync(m => (m.Email.Equals(email) || m.CPF.Equals(cpf)) && (!id.HasValue || m.Id != id));
        }

        public async Task InsertAsync(Paciente paciente)
        {
            _context.Add(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Paciente paciente)
        {
            _context.Update(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task<Paciente?> FindByIdAsync(long id)
        {
            return await _context.Pacientes.FindAsync(id);
        }

        public async Task<Paciente?> FindByCpfAsync(string cpf)
        {
            return await _context.Pacientes.SingleOrDefaultAsync(c => c.CPF == cpf);
        }

        public async Task DeleteByIdAsync(long id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<Paciente> GetAll()
        {
            return _context.Pacientes.AsQueryable();
        }
    }
}
