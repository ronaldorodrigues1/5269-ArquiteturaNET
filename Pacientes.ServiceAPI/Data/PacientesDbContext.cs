using Microsoft.EntityFrameworkCore;
using Pacientes.ServiceAPI.Domain;
using Pacientes.ServiceAPI.Dto;

namespace Pacientes.ServiceAPI.Data
{
    public class PacientesDbContext(DbContextOptions<PacientesDbContext> options) : DbContext(options)
    {
        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
