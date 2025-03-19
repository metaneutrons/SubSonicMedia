# Release Process

This document outlines the automated release process for SubSonicMedia packages.

## Automated Release Process

The project uses GitHub Actions to automate the release process. Here's how it works:

### 1. Normal Development

- Regular commits to `main` branch trigger the `build.yml` workflow
- This workflow builds and tests the code but does not create releases or publish packages

### 2. Creating a Release

To create a new release and publish packages to NuGet:

1. Create and push a version tag:
   ```bash
   git tag v1.0.0-beta   # For prerelease
   # OR
   git tag v1.0.0        # For stable release

   git push origin v1.0.0-beta  # Push the tag
   ```

2. The `publish.yml` workflow automatically:
   - Builds the projects
   - Creates NuGet packages
   - Creates a GitHub release
   - Publishes packages to NuGet.org
   - Marks the release as prerelease if the version contains a hyphen (e.g., `-beta`)

### 3. Manual Triggering

You can also manually trigger the publish workflow from the GitHub Actions tab if needed.

## Version Handling

- Version information is stored in `SubSonicMedia/Directory.Build.props`
- When pushing a tag, the version is extracted from the tag name (removing the 'v' prefix)
- For prerelease versions (e.g., `v1.0.0-beta`), the workflow sets `<VersionSuffix>beta</VersionSuffix>`
- For stable versions (e.g., `v1.0.0`), the workflow removes any version suffix

## Requirements

- GitHub repository secrets:
  - `NUGET_API_KEY`: Required for publishing to NuGet.org

## Versioning Guidelines

Follow [Semantic Versioning](https://semver.org/) principles:

- **MAJOR** version for incompatible API changes
- **MINOR** version for backward-compatible functionality additions
- **PATCH** version for backward-compatible bug fixes
- **Prerelease** identifiers (e.g., `-beta`) for prerelease versions

## Example Release Flow

1. Development on `main` branch with version set to `1.0.0-beta`
2. Ready for beta release: `git tag v1.0.0-beta && git push origin v1.0.0-beta`
3. Testing and refinement
4. Ready for stable release: `git tag v1.0.0 && git push origin v1.0.0`
5. Start next development cycle with version `1.1.0-beta`
