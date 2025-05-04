---
title: Versioning
---

SubSonicMedia follows [Semantic Versioning](https://semver.org/) and uses GitVersion to automate version bumps based on Git history and conventional commits.

## GitVersion Integration

GitVersion analyzes branch names, Git tags, commit messages, and merge history to compute a semantic version. We use `ContinuousDeployment` mode:

```yaml
mode: ContinuousDeployment
assembly-versioning-scheme: MajorMinorPatch
assembly-file-versioning-scheme: MajorMinorPatch
assembly-informational-format: '{InformationalVersion}'
tag-prefix: '[vV]'
branches:
  main:
    regex: ^main$
    label: ''
    increment: Patch
  develop:
    regex: ^develop$
    label: beta
    increment: Minor
```

Version props are emitted to `GitVersion.props` and imported by `Directory.Build.props`. The build target (`GitVersionTarget.targets`) runs during MSBuild.

## Branch Naming Conventions

- `main` (no suffix) – Stable releases
- `develop` (`-beta.n`) – Development prereleases
- `feature/*` (`-alpha.n`) – Feature in progress
- `release/*` (`-rc.n`) – Release candidate
- `hotfix/*` (`-beta.n`) – Hotfix prereleases

## Commit Message Convention

We use [Conventional Commits](https://conventionalcommits.org/):

```plaintext
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### Types and Impacts

| Type               | Impact |
|--------------------|--------|
| `feat!:`           | MAJOR  |
| `fix!:`            | MAJOR  |
| `BREAKING CHANGE`  | MAJOR  |
| `feat:`            | MINOR  |
| `fix:`             | PATCH  |
| `docs:`,`style:`, `refactor:`, `perf:`,`test:`,`chore:` | PATCH  |

## Local Development

1. Install GitVersion:

    ```bash
    dotnet tool install --global GitVersion.Tool
    ```

2. View version info:

    ```bash
    dotnet gitversion /nocache
    ```

3. Update project files:

    ```bash
    dotnet gitversion /updateprojectfiles
    ```
