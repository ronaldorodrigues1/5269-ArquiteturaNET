using Consultas.ServiceAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace Consultas.ServiceAPI.Data
{
    public class ConsultasDbContext(DbContextOptions<ConsultasDbContext> options) : DbContext(options)
    {
        public DbSet<Consulta> Consultas { get; set; }

        public DbSet<Receita> Receitas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Chamada do seed
            //SeedConsultas.Seed(modelBuilder);
        }
    }
}
