using OsakidetzaListas.Models;

namespace OsakidetzaListas.Repositories;

public interface IHistoricoRepository
{
    Task GuardarSnapshotAsync(IEnumerable<CandidatoLista> candidatos);
    Task<List<HistoricoLista>> GetHistoricoPorDniAsync(string dni);
    Task<List<HistoricoLista>> GetUltimoSnapshotAsync();
    Task<List<HistoricoLista>> GetCambiosPosicionAsync(string dni);
    Task<List<ContratacionCambio>> GetCambiosContratacionAsync(string categoria);
    Task<int> GetTotalSnapshotsAsync();
}
