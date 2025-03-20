# Release Process

This document outlines the automated release process for SubSonicMedia packages.

## Automated Release Process

The project uses GitHub Actions to automate the release process. Here's how it works:

### 1. Normal Development

- Regular commits to `main` branch trigger the `build.yml` workflow
- This workflow builds and tests the code but does not create releases or publish packages

### 2. Version Bumping

The project includes helper scripts to manage versioning:

1. Use the `Bump-Version.ps1` script to analyze commits and bump the version:
   ```powershell
   # Analyze commits and suggest a version bump
   ./scripts/Bump-Version.ps1
   
   # Apply the suggested version bump
   ./scripts/Bump-Version.ps1 -Apply
   
   # Force a specific bump type
   ./scripts/Bump-Version.ps1 -ForceBump minor -Apply
   
   # Change version stage (alpha, beta, release)
   ./scripts/Bump-Version.ps1 -Stage beta -Apply
   ```

2. Commit the version changes:
   ```bash
   git add SubSonicMedia/Directory.Build.props
   git commit -m "chore: bump version to 1.0.0-beta [skip ci]"
   ```

### 3. Creating a Release

To create a new release and publish packages to NuGet:

1. Use the `Create-Tag.ps1` script to create a version tag (requires a clean working directory):
   ```powershell
   ./scripts/Create-Tag.ps1
   ```
   
   This will:
   - Extract the version from Directory.Build.props
   - Create a tag like `v1.0.0-beta` or `v1.0.0`
   - Push the tag to origin

2. Alternatively, create and push a tag manually:
   ```bash
   git tag v1.0.0-beta   # For prerelease
   # OR
   git tag v1.0.0        # For stable release

   git push origin v1.0.0-beta  # Push the tag
   ```

3. The `publish.yml` workflow automatically:
   - Builds the projects
   - Creates NuGet packages
   - Creates a GitHub release
   - Publishes packages to NuGet.org
   - Marks the release as prerelease if the version contains a hyphen (e.g., `-beta`)

### 3. Manual Triggering

You can also manually trigger the publish workflow from the GitHub Actions tab if needed.

## Version Handling

- Version information is stored in `SubSonicMedia/Directory.Build.props`
- The version consists of two parts:
  - `<VersionPrefix>` - The numeric version (e.g., `1.0.0`)
  - `<VersionSuffix>` - Optional prerelease identifier (e.g., `beta`)
- When using the helper scripts:
  - `Bump-Version.ps1` updates the version in Directory.Build.props
  - `Create-Tag.ps1` creates a tag based on the current version
- When pushing a tag manually, the CI workflow extracts the version from the tag name (removing the 'v' prefix)
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

1. Development on `main` branch with version set to `1.0.0-alpha`

2. Ready for beta release:
   ```powershell
   # Transition to beta stage
   ./scripts/Bump-Version.ps1 -Stage beta -Apply
   
   # Commit the changes
   git add SubSonicMedia/Directory.Build.props
   git commit -m "chore: transition to beta [skip ci]"
   
   # Create and push the tag
   ./scripts/Create-Tag.ps1
   ```

3. Testing and refinement with patch updates:
   ```powershell
   # Bump patch version for fixes
   ./scripts/Bump-Version.ps1 -Apply
   
   # Commit the changes
   git add SubSonicMedia/Directory.Build.props
   git commit -m "chore: bump version [skip ci]"
   
   # Create and push the tag
   ./scripts/Create-Tag.ps1
   ```

4. Ready for stable release:
   ```powershell
   # Transition to release (removes suffix)
   ./scripts/Bump-Version.ps1 -Stage release -Apply
   
   # Commit the changes
   git add SubSonicMedia/Directory.Build.props
   git commit -m "chore: prepare stable release [skip ci]"
   
   # Create and push the tag
   ./scripts/Create-Tag.ps1
   ```

5. Start next development cycle:
   ```powershell
   # Bump minor version and set to alpha
   ./scripts/Bump-Version.ps1 -ForceBump minor -Stage alpha -Apply
   
   # Commit the changes
   git add SubSonicMedia/Directory.Build.props
   git commit -m "chore: start next development cycle [skip ci]"
   ```
