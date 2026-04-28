using Microsoft.EntityFrameworkCore;
using OsakidetzaListas.Data;
using OsakidetzaListas.Models;

namespace OsakidetzaListas.Repositories;

public class EfHistoricoRepository(AppDbContext db) : IHistoricoRepository
{
    public async Task GuardarSnapshotAsync(IEnumerable<CandidatoLista> candidatos)
    {
        var lista = candidatos.ToList();
        if (!lista.Any()) return;

        var ahora = DateTime.UtcNow;
        var registrosNuevos = new List<HistoricoLista>();

        foreach (var c in lista)
        {
            // Buscar el último registro para este DNI + centro + tipo
            var ultimo = await db.Historico
                .Where(x => x.Dni == c.Dni && x.Zdespl == c.Zdespl && x.Ztconc == c.Ztconc)
                .OrderByDescending(x => x.FechaConsulta)
                .FirstOrDefaultAsync();

            // Solo guardar si no existe registro previo o si algo ha cambiado
            bool hayNovedad = ultimo == null
                || ultimo.Posicabs != c.Posicabs
                || ultimo.Posicrel != c.Posicrel
                || ultimo.Posicrelpl != c.Posicrelpl
                || ultimo.EstaOcupado != c.EstaOcupado
                || ultimo.Zcalexa != c.Zcalexa
                || ultimo.Zcalexp != c.Zcalexp
                || ultimo.Zptoeus != c.Zptoeus;

            if (hayNovedad)
            {
                registrosNuevos.Add(new HistoricoLista
                {
                    FechaConsulta = ahora,
                    Dni = c.Dni,
                    NombreCompleto = c.NombreCompleto,
                    Zdesca = c.Zdesca,
                    Zdespl = c.Zdespl,
                    Ztconc = c.Ztconc,
                    Zcalexa = c.Zcalexa,
                    Zcalexp = c.Zcalexp,
                    Zptoeus = c.Zptoeus,
                    Zptoaca = c.Zptoaca,
                    PuntuacionTotal = c.PuntuacionTotal,
                    Posicabs = c.Posicabs,
                    Posicrel = c.Posicrel,
                    Posicrelpl = c.Posicrelpl
                });
            }
        }

        if (registrosNuevos.Any())
        {
            db.Historico.AddRange(registrosNuevos);
            await db.SaveChangesAsync();
        }
    }



    public async Task<List<HistoricoLista>> GetHistoricoPorDniAsync(string dni) =>
    await db.Historico
        .Where(x => x.Dni == dni)
        .OrderBy(x => x.FechaConsulta)
        .ToListAsync();

    public async Task<List<HistoricoLista>> GetUltimoSnapshotAsync()
    {
        // Para cada combinación única DNI+Zdespl+Ztconc, coger el registro más reciente
        var ultimoEstado = await db.Historico
            .GroupBy(x => new { x.Dni, x.Zdespl, x.Ztconc })
            .Select(g => g.OrderByDescending(x => x.FechaConsulta).First())
            .ToListAsync();

        return ultimoEstado;
    }


    public async Task<List<HistoricoLista>> GetCambiosPosicionAsync(string dni)
    {
        var historial = await db.Historico
            .Where(x => x.Dni == dni)
            .OrderBy(x => x.FechaConsulta)
            .ToListAsync();

        var cambios = new List<HistoricoLista>();
        for (int i = 1; i < historial.Count; i++)
        {
            var prev = historial[i - 1];
            var curr = historial[i];
            if (curr.Posicabs != prev.Posicabs ||
                curr.Posicrel != prev.Posicrel ||
                curr.EstaOcupado != prev.EstaOcupado)
                cambios.Add(curr);
        }
        return cambios;
    }

    public async Task<int> GetTotalSnapshotsAsync() =>
     await db.Historico.CountAsync();
}
