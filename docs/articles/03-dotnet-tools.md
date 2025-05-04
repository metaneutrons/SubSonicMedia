---
title: .NET Global Tools
---

SubSonicMedia uses .NET global tools defined in `.config/dotnet-tools.json`:

```json
{
  "tools": {
    "husky": { "version": "0.7.2", "commands": ["husky"] },
    "docfx": { "version": "2.78.3", "commands": ["docfx"] },
    "gitversion.tool": { "version": "6.3.0", "commands": ["dotnet-gitversion"] }
  }
}
```

- **husky**: Git hooks manager (pre-commit, pre-push)
- **docfx**: Documentation generator
- **gitversion.tool**: Semantic versioning via Git history

Install all tools with:

```bash
dotnet tool restore
```
