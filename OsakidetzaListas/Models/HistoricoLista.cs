namespace OsakidetzaListas.Models;

public class HistoricoLista
{
    public int Id { get; set; }
    public DateTime FechaConsulta { get; set; } = DateTime.UtcNow;
    public string Dni { get; set; } = "";
    public string NombreCompleto { get; set; } = "";
    public string Zdesca { get; set; } = "";      // Categoría
    public string Zdespl { get; set; } = "";      // Centro/Plaza
    public string Ztconc { get; set; } = "";      // I / S
    public decimal Zcalexa { get; set; }
    public decimal Zcalexp { get; set; }
    public decimal Zptoeus { get; set; }
    public decimal Zptoaca { get; set; }
    public decimal PuntuacionTotal { get; set; }
    public int Posicabs { get; set; }
    public int Posicrel { get; set; }
    public int Posicrelpl { get; set; }

    public bool EstaOcupado => Posicrel == 0;
    public string EstadoTexto => EstaOcupado ? "🔴 Ocupado" : "🟢 Disponible";
    public string TipoListaBadge => Ztconc switch
    {
        "I" => "bg-success",
        "S" => "bg-warning text-dark",
        _ => "bg-secondary"
    };
}
