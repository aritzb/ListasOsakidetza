# AGENTS.md

Repository guidance for AI coding agents working on `OsakidetzaListas`.

## Project Snapshot

- Stack: ASP.NET Core Blazor Web App (.NET 8)
- Main project: `OsakidetzaListas/`
- UI style: Bootstrap 5 plus custom CSS in `OsakidetzaListas/wwwroot/app.css`
- Data flow: service + repository + SQLite snapshot history

## Working Rules

- Prefer small, focused changes over broad refactors.
- Preserve the current Blazor page structure unless the user asks for a redesign.
- Use `apply_patch` for edits.
- Do not delete or rewrite user changes unless explicitly requested.
- Keep new files ASCII unless there is a strong reason not to.

## Important Files

- `OsakidetzaListas/Components/Pages/Index.razor`
- `OsakidetzaListas/Components/Pages/BuscarPersona.razor`
- `OsakidetzaListas/Components/Pages/Oposicion.razor`
- `OsakidetzaListas/wwwroot/app.css`
- `OsakidetzaListas/Data/AppDbContext.cs`
- `OsakidetzaListas/Repositories/`
- `OsakidetzaListas/Services/`

## Validation

- Build with `dotnet build OsakidetzaListas.sln`
- Run with `dotnet run --project OsakidetzaListas`

## Domain Notes

- `osakidetza_historico.db` is the local snapshot store.
- `wwwroot/dnisfiltrados.json` is part of the app data and should be treated carefully.
- The home page, search page, and opposition simulator all read from the same latest snapshot.

