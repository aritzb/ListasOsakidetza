# Testing Skill

Use this skill when validating a change before handing it back.

## Minimum Checks

- Build the solution.
- Open the affected page flows.
- Confirm no obvious console or runtime errors.
- Check that the change behaves correctly with empty data and with populated data.

## Useful Commands

- `dotnet build OsakidetzaListas.sln`
- `dotnet run --project OsakidetzaListas`

## Notes

- If a change touches layout or filters, test both desktop and mobile widths.
- If a change touches history or export, verify the data shape and formatting.

