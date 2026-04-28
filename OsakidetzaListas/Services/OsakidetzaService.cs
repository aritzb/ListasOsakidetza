using System.Text.Json;
using OsakidetzaListas.Models;

namespace OsakidetzaListas.Services;

public class OsakidetzaService(HttpClient http, ILogger<OsakidetzaService> logger, IWebHostEnvironment env)
{
    private const string BaseUrl = "https://lcontratacion.osakidetza.eus/obtenerLista/";
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public List<string> Dnis { get; private set; } = [];

    public void SetDnis(List<string> dnis) => Dnis = dnis;

    public List<CandidatoLista> UltimosResultados { get; set; } = [];


    public async Task CargarDnisDesdeJsonAsync()
    {
        try
        {
            var path = Path.Combine(env.WebRootPath, "dnisfiltrados.json");
            if (!File.Exists(path))
            {
                logger.LogWarning("No se encontró dnisfiltrados.json en wwwroot");
                return;
            }
            var json = await File.ReadAllTextAsync(path);
            var lista = JsonSerializer.Deserialize<List<string>>(json);
            if (lista != null && lista.Count > 0)
            {
                Dnis = lista;
                logger.LogInformation("Cargados {Count} DNIs desde dnisfiltrados.json", Dnis.Count);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error cargando dnisfiltrados.json");
        }
    }

    public async Task<List<CandidatoLista>> GetListasCandidatoAsync(string dni)
    {
        try
        {
            var response = await http.GetAsync($"{BaseUrl}{dni.ToUpperInvariant()}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json) || !json.TrimStart().StartsWith('{'))
            {
                logger.LogWarning("DNI {Dni} no encontrado o respuesta vacía", dni);
                return [];
            }

            var root = JsonSerializer.Deserialize<OsakidetzaResponse>(json, JsonOpts);
            var items = root?.Candidato?.ListasActivas?.Item;
            if (items != null)
            {
                var nombre = root!.Candidato!.NombreCompleto;
                foreach (var item in items)
                {
                    item.Dni = dni.ToUpperInvariant();
                    item.NombreCompleto = nombre;
                }
                return items;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error obteniendo lista para DNI {Dni}", dni);
        }
        return [];
    }

    public async Task<List<CandidatoLista>> GetTodasLasListasAsync(Func<int, int, string?, Task>? progress = null)
    {
        var resultados = new List<CandidatoLista>();
        var total = Dnis.Count;

        for (var i = 0; i < Dnis.Count; i++)
        {
            var dni = Dnis[i];
            var items = await GetListasCandidatoAsync(dni);
            resultados.AddRange(items);

            if (progress != null)
            {
                await progress(i + 1, total, dni);
            }
        }

        UltimosResultados = resultados;
        return UltimosResultados;
    }



}
