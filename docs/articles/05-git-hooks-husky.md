---
title: Git Hooks (Husky)
---

SubSonicMedia uses [Husky](https://typicode.github.io/husky/) to manage Git hooks and ensure code quality checks run automatically.

## Installed Hooks

The following hooks are defined in `.husky/`:

- **pre-commit**: runs `dotnet format --verify-no-changes` and `dotnet test --no-build`.
- **pre-push**: runs `dotnet test --no-build --verbosity normal`.
- **commit-msg**: enforces Conventional Commits message format.

## Configuration

Global tool installed: `husky` (v0.7.2), configured in `.config/dotnet-tools.json`.

Hook scripts live under `.husky/`:

```bash
# example: show pre-commit script
cat .husky/pre-commit
```
