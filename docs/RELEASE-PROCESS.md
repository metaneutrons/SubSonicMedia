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

The workflow is optimized to prevent duplicate builds:
- The Build and Test workflow runs on branch pushes but has special logic:
  - Initial job checks if the commit has a version tag
  - Skips the build if a version tag is found
  - This prevents duplicate builds when commits and tags are pushed together
- Tag pushes (v*) are handled by the Tag Release workflow
- This intelligent skipping prevents the same code from being built twice

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

1. **Branch Origin Verification**:
   - Only tags created from the `develop` branch are considered
   - Tags created from other branches do not update main
   - This ensures consistent release workflow

2. **Before First Stable Release**:
   - Every tag (beta or stable) from develop updates the main branch
   - This keeps main in sync with develop until the first stable release

3. **After First Stable Release**:
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
6. After approval, the workflow automatically publishes the package to NuGet.org
7. A confirmation comment is added to the issue with publish details

This approval-based workflow ensures:
- Packages are only published after explicit review by a repository admin
- There's a complete audit trail of who approved each publish
- The issue contains a record of the published version and timestamp

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

## Helper Scripts

### Create-Tag.ps1

The `Create-Tag.ps1` script automates the tag creation and push process:

```pwsh
./scripts/Create-Tag.ps1
```

Features:
- Automatic version detection using GitVersion
- Detection of uncommitted changes
- Automatic detection of unpushed commits
- Combined pushing of commits and tags in a single Git operation
- Support for developing on the develop branch (no confirmation required)
- Comprehensive verification of both tag and commits on remote after pushing
- Clean command preview without step numbers when simple operations
- Prompts for confirmation at critical points
- Prevents duplicate GitHub workflow runs through optimized push strategy

## Example Workflow

1. Develop and commit changes to the `develop` branch
2. Run `./scripts/Create-Tag.ps1` to create and push a beta tag
   - The script will detect and push any unpushed commits together with the tag
   - It will create a tag based on the current version from GitVersion
   - The build workflow will automatically skip duplicate builds
3. Test the beta release 
4. Make additional improvements with more beta releases as needed
5. When ready for stable: run the script again or manually create a stable tag without beta suffix
   ```bash
   # Either using the script:
   ./scripts/Create-Tag.ps1
   
   # Or manually:
   git tag v1.0.3 && git push origin v1.0.3
   ```
6. Run the "Publish to NuGet.org" workflow and approve as needed
7. Continue development for the next version on the `develop` branch