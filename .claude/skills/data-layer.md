# Data Layer Skill

Use this skill when changing service, repository, or database behavior.

## Principles

- Keep data access centralized.
- Avoid duplicating snapshot transformation logic.
- Treat historical records as append-only unless the user asks otherwise.

## Common Targets

- `Data/AppDbContext.cs`
- `Repositories/IHistoricoRepository.cs`
- `Repositories/EfHistoricoRepository.cs`
- `Services/OsakidetzaService.cs`

## Validation

- Confirm the app still loads the latest snapshot.
- Verify history writes do not break existing reads.
- Check for accidental schema drift.

