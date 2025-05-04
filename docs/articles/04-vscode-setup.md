---
title: VS Code Setup
---

SubSonicMedia provides recommended settings and extensions for Visual Studio Code.

## Recommended Extensions

Defined in `.vscode/extensions.json`:

- ms-dotnettools.csharp
- ms-dotnettools.csdevkit
- formulahendry.dotnet-test-explorer
- streetsidesoftware.code-spell-checker
- editorconfig.editorconfig
- github.vscode-github-actions

## Settings

Key settings in `.vscode/settings.json`:

```json
{
  "editor.formatOnSave": true,
  "editor.tabSize": 4,
  "editor.insertSpaces": true,
  "files.insertFinalNewline": true,
  "omnisharp.enableRoslynAnalyzers": true
}
```

## Launch Configurations

Debug TestKit and test projects via `.vscode/launch.json`.

## Tasks

Use `.vscode/tasks.json` for build, test, pack, and lint tasks.
