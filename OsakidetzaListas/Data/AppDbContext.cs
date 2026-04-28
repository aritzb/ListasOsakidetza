using Microsoft.EntityFrameworkCore;
using OsakidetzaListas.Models;

namespace OsakidetzaListas.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<HistoricoLista> Historico => Set<HistoricoLista>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HistoricoLista>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.Dni, x.FechaConsulta });
            e.HasIndex(x => x.FechaConsulta);
            e.Property(x => x.PuntuacionTotal).HasPrecision(10, 5);
            e.Property(x => x.Zcalexa).HasPrecision(10, 5);
            e.Property(x => x.Zcalexp).HasPrecision(10, 5);
            e.Property(x => x.Zptoeus).HasPrecision(10, 5);
        });
    }
}
