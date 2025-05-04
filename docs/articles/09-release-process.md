---
title: Release Process
---

This document outlines the GitFlow-based release process using GitVersion for SemVer automation.

## Branch Strategy

1. **Develop** (`develop`)
   - Main development branch
   - Merges of features/fixes produce beta packages (`1.2.0-beta.n`)

2. **Release** (`release/{x.y.z}`)
   - Created from `develop` when preparing a release:

     ```bash
     git checkout develop
     git pull
     git checkout -b release/1.2.0
     ```

   - CI builds RC packages (`1.2.0-rc.n`)

3. **Main** (`main`)
   - Stable branch, updated on tag push
   - Merges of release branches:

     ```bash
     git checkout main
     git merge --no-ff release/1.2.0
     git tag v1.2.0
     git push origin main --tags
     ```

4. **Back-merge to Develop**:

   ```bash
   git checkout develop
   git merge --no-ff main
   git push
   ```

## CI Workflows

- **Develop builds**: on `develop` push, runs GitVersion, build, test, pack prerelease artifacts.
- **Tag builds**: on `v*` tag push, triggers `tag-release.yml`.
- **Publish to NuGet**: manual via `nuget-publish.yml`.

## Manual Release Steps

1. Create `release/x.y.z` branch and finalize changes.
2. Merge into `main`, tag, and push.
3. Run **Publish NuGet Packages from Release** workflow with the version tag.
