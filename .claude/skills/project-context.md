# Project Context Skill

Use this skill when you need a quick mental model of the repo.

## Context

- App name: `OsakidetzaListas`
- Framework: Blazor Web App on .NET 8
- Domain: Osakidetza list rankings and historical snapshots

## Key Areas

- `Components/Pages/` contains the main user flows.
- `Services/` handles API and data orchestration.
- `Repositories/` persists and retrieves history.
- `Data/` contains the database context.
- `wwwroot/app.css` holds the core visual language.

## Practical Notes

- The home page is the main dashboard.
- Search and opposition pages reuse the same snapshot data.
- `osakidetza_historico.db` is the persistent source for history.

