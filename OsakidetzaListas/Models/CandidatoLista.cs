using System.Text.Json.Serialization;

namespace OsakidetzaListas.Models;

// Raíz del JSON
public class OsakidetzaResponse
{
    public CandidatoInfo? Candidato { get; set; }
}

public class CandidatoInfo
{
    public string Zdni { get; set; } = "";
    public string Vorna { get; set; } = "";
    public string Zapell1 { get; set; } = "";
    public string Zapell2 { get; set; } = "";
    public ListasActivas? ListasActivas { get; set; }

    public string NombreCompleto => $"{Zapell1} {Zapell2}, {Vorna}";
}

public class ListasActivas
{
    public List<CandidatoLista>? Item { get; set; }
}

public class CandidatoLista
{
    [JsonIgnore]
    public string Dni { get; set; } = "";

    [JsonIgnore]
    public string NombreCompleto { get; set; } = "";

    public string? Zconv { get; set; }

    /// <summary>Tipo de contrato: I = Interinidad, S = Sustitución/Lista Corta</summary>
    public string Ztconc { get; set; } = "";

    public string? Zprio { get; set; }
    public string Zcodcat { get; set; } = "";

    /// <summary>Descripción categoría (castellano)</summary>
    public string Zdesca { get; set; } = "";

    /// <summary>Descripción categoría (euskera)</summary>
    public string Zdeseusca { get; set; } = "";

    public string? Ztipopla { get; set; }
    public string? Zcodpla { get; set; }

    /// <summary>Descripción plaza/centro (castellano)</summary>
    public string Zdespl { get; set; } = "";

    /// <summary>Descripción plaza/centro (euskera)</summary>
    public string Zdeseuspl { get; set; } = "";

    /// <summary>Puntuación examen/oposición</summary>
    public decimal Zcalexa { get; set; }

    /// <summary>Puntuación experiencia</summary>
    public decimal Zcalexp { get; set; }

    /// <summary>Puntuación euskera</summary>
    public decimal Zptoeus { get; set; }

    /// <summary>Puntuación académica</summary>
    public decimal Zptoaca { get; set; }

    /// <summary>Posición absoluta</summary>
    public int Posicabs { get; set; }

    /// <summary>Posición relativa — 0 = persona ocupada/no contratable</summary>
    public int Posicrel { get; set; }

    /// <summary>Posición relativa con perfil lingüístico — 0 = ocupada en ese ranking</summary>
    public int Posicrelpl { get; set; }

    public string Fecultact { get; set; } = "";

    // Calculados
    public decimal PuntuacionTotal => Zcalexa + Zcalexp + Zptoeus;
    public bool EstaOcupado => Posicrel == 0;
    public string EstadoTexto => EstaOcupado ? "🔴 Ocupado" : "🟢 Disponible";

    public string TipoLista => Ztconc; // alias para compatibilidad con el resto del código

    public string TipoListaDescripcion => Ztconc switch
    {
        "I" => "Interinidad",
        "S" => "Lista Corta",
        _ => Ztconc
    };

    public string TipoListaBadge => Ztconc switch
    {
        "I" => "bg-success",
        "S" => "bg-warning text-dark",
        _ => "bg-secondary"
    };
}
