using Microsoft.EntityFrameworkCore;
using OsakidetzaListas.Components;
using OsakidetzaListas.Data;
using OsakidetzaListas.Repositories;
using OsakidetzaListas.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ── BBDD: cambia solo esta línea para migrar a otro motor ──────────────────
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=osakidetza_historico.db"));
// PostgreSQL: opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
// SQL Server: opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
// ──────────────────────────────────────────────────────────────────────────

builder.Services.AddScoped<IHistoricoRepository, EfHistoricoRepository>();
builder.Services.AddScoped<ExportService>();

// IWebHostEnvironment se inyecta automáticamente en OsakidetzaService
builder.Services.AddHttpClient<OsakidetzaService>()
    .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(30));
builder.Services.AddSingleton(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var logger = sp.GetRequiredService<ILogger<OsakidetzaService>>();
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    var http = factory.CreateClient(nameof(OsakidetzaService));
    return new OsakidetzaService(http, logger, env);
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
    });

var app = builder.Build();

// Crear BBDD al arrancar
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    ctx.Database.EnsureCreated();
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
