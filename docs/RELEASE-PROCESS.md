# Release Process

This document outlines the release process for SubSonicMedia packages.

## Branch Strategy

Our release process uses two primary branches:

1. **Develop Branch** (`develop`):
   - This is where all development happens
   - Features, fixes, and improvements are committed here
   - Beta releases are tagged from this branch
   - Version numbers include a beta suffix (e.g., `1.0.3-beta.5`)

2. **Main Branch** (`main`):
   - This is the stable release branch
   - Updated automatically from tagged versions 
   - Contains only production-ready code
   - Has special update rules (detailed below)
   - Version numbers have no suffix (e.g., `1.0.3`)

## GitVersion Integration

We use GitVersion to automatically calculate version numbers based on Git history:

- Version numbers follow Semantic Versioning (SemVer)
- The format is `{major}.{minor}.{patch}[-{prerelease}]`
- Branch names influence the prerelease label:
  - `develop` branch uses `-beta.{n}` suffix
  - `main` branch has no suffix

## Release Workflows

### 1. Automated Build and Validation

Every commit to `develop` or `main` triggers:
- Code building and validation
- Style checking
- Test runs
- Package creation (but not publishing)

### 2. Creating Beta Releases

When ready for a beta release:

1. Ensure you're on the `develop` branch
2. Create and push a tag with beta suffix:
   ```bash
   git tag v1.0.3-beta.5
   git push origin v1.0.3-beta.5
   ```
3. This triggers:
   - GitHub release creation with "Beta" label
   - Main branch update (if no stable release exists yet)
   - Package creation (but not publishing to NuGet.org)

### 3. Creating Stable Releases

When ready for a stable release:

1. Ensure you're on the `develop` branch with all changes tested
2. Create and push a tag without a suffix:
   ```bash
   git tag v1.0.3
   git push origin v1.0.3
   ```
3. This triggers:
   - GitHub release creation with "Release" label
   - Main branch update (always updates for stable releases)
   - Package creation (but not publishing to NuGet.org)

### 4. Main Branch Update Rules

The `update-main.yml` workflow determines when to update the main branch:

1. **Before First Stable Release**:
   - Every tag (beta or stable) updates the main branch
   - This keeps main in sync with develop until the first stable release

2. **After First Stable Release**:
   - Only stable tags (without a suffix) update the main branch
   - Beta tags no longer affect the main branch
   - This ensures main only contains stable versions

### 5. Publishing to NuGet.org

Publishing to NuGet.org is a separate manual step with approval:

1. Go to the GitHub Actions tab
2. Run the "Publish to NuGet.org" workflow
3. Enter the version tag to publish (e.g., `v1.0.3-beta.5` or `v1.0.3`)
4. The workflow creates an issue requiring admin approval
5. A repository admin must comment `/publish` on the issue
6. Only after approval will the package be published to NuGet.org

This two-step process ensures packages are only published after explicit review and approval by a repository administrator.

## Github Releases

GitHub releases are created automatically when tags are pushed:

- **Beta Releases**:
  - Tagged with a version containing a hyphen (e.g., `v1.0.3-beta.5`)
  - Labeled as "Beta" with prerelease flag
  - Include NuGet packages as assets

- **Stable Releases**:
  - Tagged with a version without a hyphen (e.g., `v1.0.3`)
  - Labeled as "Release" without prerelease flag
  - Include NuGet packages as assets

## Special Requirements

1. **Personal Access Token (PAT)**:
   - Required for the `update-main.yml` workflow
   - Needs `contents` and `workflows` permissions
   - Must be added as a `PAT_TOKEN` repository secret

2. **NuGet API Key**:
   - Required for publishing to NuGet.org
   - Must be added as a `NUGET_API_KEY` repository secret

## Example Workflow

1. Develop and commit changes to the `develop` branch
2. Create a beta release with `git tag v1.0.3-beta.1 && git push origin v1.0.3-beta.1`
3. Test the beta release 
4. Make additional improvements with more beta releases as needed
5. When ready for stable: `git tag v1.0.3 && git push origin v1.0.3`
6. Run the "Publish to NuGet.org" workflow and approve as needed
7. Continue development for the next version on the `develop` branch