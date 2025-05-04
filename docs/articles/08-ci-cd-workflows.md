---
title: CI/CD Workflows
---

## build.yml (CI Build)

Runs on pushes to `develop` branch:

1. Checkout (full history) + GitVersion
2. `dotnet restore`, `dotnet build`, `dotnet test`
3. `dotnet pack` outputs prerelease packages to `./artifacts`

## tag-release.yml

Triggers on Git tags matching `v*`:

1. Checkout full history + GitVersion
2. `dotnet build --configuration Release`
3. `dotnet pack -o ./artifacts`
4. Create GitHub Release (draft: false, prerelease if beta suffix)
5. Upload artifacts (`.nupkg`/.snupkg)

## nuget-publish.yml

Manual dispatch workflow:

1. Input `version_tag` (e.g., `v1.2.0`)
2. Download artifacts from corresponding GitHub release
3. Push `.nupkg` packages to NuGet.org using `NUGET_API_KEY`

## docfx.yml

Manual dispatch or pushes to `main`:

1. `docfx metadata && docfx build`
2. Publish to `gh-pages` branch (or custom hosting)
