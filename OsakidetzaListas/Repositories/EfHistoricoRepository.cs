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

    public async Task<List<ContratacionCambio>> GetCambiosContratacionAsync(string categoria)
    {
        var historial = await db.Historico
            .Where(x => x.Zdesca == categoria)
            .OrderBy(x => x.Dni)
            .ThenBy(x => x.Zdespl)
            .ThenBy(x => x.Ztconc)
            .ThenBy(x => x.FechaConsulta)
            .ToListAsync();

        var cambios = new List<ContratacionCambio>();

        foreach (var grupo in historial.GroupBy(x => new { x.Dni, x.Zdespl, x.Ztconc }))
        {
            var registros = grupo.OrderBy(x => x.FechaConsulta).ToList();

            for (var i = 1; i < registros.Count; i++)
            {
                var prev = registros[i - 1];
                var curr = registros[i];

                if (prev.EstaOcupado == curr.EstaOcupado)
                {
                    continue;
                }

                cambios.Add(new ContratacionCambio
                {
                    FechaConsulta = curr.FechaConsulta,
                    Dni = curr.Dni,
                    NombreCompleto = curr.NombreCompleto,
                    Zdesca = curr.Zdesca,
                    Zdespl = curr.Zdespl,
                    Ztconc = curr.Ztconc,
                    EstadoAnteriorOcupado = prev.EstaOcupado,
                    EstadoActualOcupado = curr.EstaOcupado
                });
            }
        }

        return cambios
            .GroupBy(x => new
            {
                x.Dni,
                x.FechaConsulta,
                x.EstadoAnteriorOcupado,
                x.EstadoActualOcupado
            })
            .Select(g =>
            {
                var primero = g.First();
                return new ContratacionCambio
                {
                    FechaConsulta = primero.FechaConsulta,
                    Dni = primero.Dni,
                    NombreCompleto = primero.NombreCompleto,
                    Zdesca = primero.Zdesca,
                    Zdespl = string.Join(" · ", g.Select(x => x.Zdespl).Distinct().Take(3)),
                    Ztconc = primero.Ztconc,
                    EstadoAnteriorOcupado = primero.EstadoAnteriorOcupado,
                    EstadoActualOcupado = primero.EstadoActualOcupado,
                    CentrosAfectados = g.Select(x => x.Zdespl).Distinct().Count()
                };
            })
            .OrderByDescending(x => x.FechaConsulta)
            .ThenBy(x => x.NombreCompleto)
            .ThenBy(x => x.Dni)
            .ToList();
    }

    public async Task<int> GetTotalSnapshotsAsync() =>
     await db.Historico.CountAsync();
}
