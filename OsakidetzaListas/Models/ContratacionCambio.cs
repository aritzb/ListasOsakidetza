namespace OsakidetzaListas.Models;

public class ContratacionCambio
{
    public DateTime FechaConsulta { get; set; }
    public string Dni { get; set; } = "";
    public string NombreCompleto { get; set; } = "";
    public string Zdesca { get; set; } = "";
    public string Zdespl { get; set; } = "";
    public string Ztconc { get; set; } = "";
    public bool EstadoAnteriorOcupado { get; set; }
    public bool EstadoActualOcupado { get; set; }
    public int CentrosAfectados { get; set; }

    public string EstadoAnteriorTexto => EstadoAnteriorOcupado ? "🔴 Ocupado" : "🟢 Libre";
    public string EstadoActualTexto => EstadoActualOcupado ? "🔴 Ocupado" : "🟢 Libre";
    public string CambioTexto => EstadoAnteriorOcupado == EstadoActualOcupado
        ? "Sin cambio"
        : (EstadoAnteriorOcupado ? "Ocupado → Libre" : "Libre → Ocupado");
    public string CentrosTexto => CentrosAfectados switch
    {
        0 => "Sin centros",
        1 => "1 centro",
        _ => $"{CentrosAfectados} centros"
    };
}
