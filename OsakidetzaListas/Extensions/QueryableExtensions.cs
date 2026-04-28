using OsakidetzaListas.Models;

namespace OsakidetzaListas.Extensions;

public static class QueryableExtensions
{
    public static IEnumerable<CandidatoLista> OrderByDynamic(
        this IEnumerable<CandidatoLista> source, string property, bool ascending) =>
        property switch
        {
            "Dni" => ascending ? source.OrderBy(x => x.Dni) : source.OrderByDescending(x => x.Dni),
            "NombreCompleto" => ascending ? source.OrderBy(x => x.NombreCompleto) : source.OrderByDescending(x => x.NombreCompleto),
            "Zdesca" => ascending ? source.OrderBy(x => x.Zdesca) : source.OrderByDescending(x => x.Zdesca),
            "Zdespl" => ascending ? source.OrderBy(x => x.Zdespl) : source.OrderByDescending(x => x.Zdespl),
            "TipoLista" => ascending ? source.OrderBy(x => x.Ztconc) : source.OrderByDescending(x => x.Ztconc),
            "Zcalexa" => ascending ? source.OrderBy(x => x.Zcalexa) : source.OrderByDescending(x => x.Zcalexa),
            "Zcalexp" => ascending ? source.OrderBy(x => x.Zcalexp) : source.OrderByDescending(x => x.Zcalexp),
            "Zptoeus" => ascending ? source.OrderBy(x => x.Zptoeus) : source.OrderByDescending(x => x.Zptoeus),
            "Zptoaca" => ascending ? source.OrderBy(x => x.Zptoaca) : source.OrderByDescending(x => x.Zptoaca),
            "Posicabs" => ascending ? source.OrderBy(x => x.Posicabs) : source.OrderByDescending(x => x.Posicabs),
            "Posicrel" => ascending ? source.OrderBy(x => x.Posicrel) : source.OrderByDescending(x => x.Posicrel),
            "EstaOcupado" => ascending ? source.OrderBy(x => x.EstaOcupado) : source.OrderByDescending(x => x.EstaOcupado),
            _ => ascending ? source.OrderBy(x => x.PuntuacionTotal) : source.OrderByDescending(x => x.PuntuacionTotal),
        };
}
