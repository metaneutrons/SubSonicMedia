# Versioning in SubSonicMedia

This project follows [Semantic Versioning](https://semver.org/) principles and uses GitVersion to automatically determine version bumps based on Git history and conventional commits.

## GitVersion Integration

The project uses [GitVersion](https://gitversion.net/) to automate versioning, which eliminates the need to manually update version numbers in multiple places.

### How GitVersion Works

GitVersion analyzes your branch structure and commit messages to determine the appropriate version number for your project. The configuration is set to `ManualDeployment` mode, which gives you full control over when version numbers change.

1. **GitVersion** analyzes your Git repository to determine the current version based on:
   - Git tags
   - Branch names
   - Commit messages
   - Merge history

2. **Version Numbers** are automatically generated and follow SemVer 2.0:
   - `1.2.3` - Release version
   - `1.2.3-alpha.1` - Alpha prerelease
   - `1.2.3-beta.2` - Beta prerelease
   - `1.2.3-rc.3` - Release candidate

3. **Integration with Build Process**:
   - The `GitVersionTarget.targets` file runs GitVersion during build
   - Version information is stored in `GitVersion.props`
   - `Directory.Build.props` imports and uses these properties

### GitVersion Configuration

The GitVersion configuration is in `GitVersion.yml`:

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
    is-release-branch: true

  develop:
    regex: ^develop$
    label: beta
    increment: Minor
    is-release-branch: false
```

This configuration uses:
- `ContinuousDeployment` mode: Version is calculated from Git history
- Branch-specific configurations:
  - `main` branch: Release branch with no label, increment Patch version
  - `develop` branch: Development branch with beta label, increment Minor version

### Branch Naming Conventions

GitVersion uses branch names to determine version suffixes:

- `main` or `master` - Release branch (no suffix)
- `develop` - Development branch (beta suffix)
- `feature/xyz` - Feature branch (alpha suffix)
- `release/x.y.z` - Release preparation (rc suffix)
- `hotfix/xyz` - Hotfix branch (beta suffix)

## Commit Message Convention

We use a simplified version of the [Conventional Commits](https://www.conventionalcommits.org/) specification to trigger version bumps:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### Types that affect versioning

| Type | Description | Version Impact |
|------|-------------|------------|
| `feat!:` | A feature that introduces a breaking change | MAJOR |
| `fix!:` | A bug fix that introduces a breaking change | MAJOR |
| `BREAKING CHANGE:` | Any commit with this in the footer | MAJOR |
| `feat:` | A new feature | MINOR |
| `fix:` | A bug fix | PATCH |
| `docs:` | Documentation only changes | PATCH |
| `style:` | Changes that do not affect the meaning of the code | PATCH |
| `refactor:` | A code change that neither fixes a bug nor adds a feature | PATCH |
| `perf:` | A code change that improves performance | PATCH |
| `test:` | Adding missing tests or correcting existing tests | PATCH |
| `chore:` | Changes to the build process or auxiliary tools | PATCH |

### Examples

```
feat: add new browsing API method

This adds a new method to browse by genre.
```

```
fix!: change parameter order in streaming API

BREAKING CHANGE: The order of parameters in the streaming API has changed.
```

```
docs: update README with new examples
```

### GitVersion Commit Message Format

In addition to conventional commits, you can use GitVersion's commit message format to trigger version bumps:

```
+semver: major|minor|patch|none
```

For example:
```
Add new feature
+semver: minor
```

## Version Management and Release Process

### Local Development

1. **Install GitVersion**:
   ```
   dotnet tool install --global GitVersion.Tool
   ```

2. **View Version Information**:
   ```
   dotnet gitversion /nocache
   ```

### Branch Strategy

Our strategy uses two primary branches:

1. **Develop Branch**:
   - All development happens here
   - Versioned with beta suffix (e.g., `1.0.3-beta.5`) 
   - Tags can be created for beta releases
   - Until the first stable release, main branch follows develop

2. **Main Branch**:
   - Stable release branch
   - Updated automatically from tagged versions
   - After first stable release, only updated from stable (non-beta) tags
   - Stable releases have no suffix (e.g., `1.0.3`)

### Release Process

#### Beta Releases (from develop branch)

1. Develop and commit changes to the develop branch
2. When ready for a beta release:
   ```bash
   # Create a beta tag (from develop branch)
   git tag v1.0.3-beta.5
   git push origin v1.0.3-beta.5
   ```
3. This triggers:
   - `tag-release.yml` workflow: Creates GitHub release with NuGet packages
   - `update-main.yml` workflow: Updates main branch if no stable release exists yet

#### Stable Releases

1. When ready for a stable release:
   ```bash
   # Create a stable tag (from develop branch)
   git tag v1.0.3
   git push origin v1.0.3
   ```
2. This triggers:
   - `tag-release.yml` workflow: Creates GitHub release with NuGet packages
   - `update-main.yml` workflow: Always updates main branch to this stable version

### Publishing to NuGet.org

We publish to NuGet.org manually with approval:

1. Go to the "Actions" tab in your GitHub repository
2. Select the "Publish to NuGet.org" workflow
3. Click "Run workflow"
4. Enter the version tag to publish (e.g., `v1.0.3-beta.5` or `v1.0.3`)
5. The workflow will:
   - Create a confirmation issue requiring admin approval
   - Wait for a repository admin to comment `/publish` on the issue
   - Publish the package to NuGet.org after approval

This two-step process ensures packages are only published after explicit review and approval by a repository administrator.

## Tag Naming Convention

Tags should follow the format `v{version}` (e.g., `v1.2.3` or `v1.2.3-beta.1`).

## Integration with MSBuild

GitVersion is integrated with the build system via:

1. `GitVersion.props`: Contains version properties used by MSBuild
2. `GitVersionTarget.targets`: MSBuild target that runs GitVersion
3. `Directory.Build.props`: Imports GitVersion properties

This ensures consistent versioning across local and CI builds.

## API Version

The Subsonic API version supported by this library is specified separately in `Directory.Build.props` as `<SubsonicApiVersion>`.

## Benefits

- **Automated Semantic Versioning**: Version numbers are derived from Git history
- **Manual Control**: ManualDeployment mode ensures you control when versions change
- **Single Source of Truth**: Version information is derived from Git
- **Assembly Version Compliance**: Ensures assembly versions follow .NET requirements
- **Proper NuGet Versioning**: Correctly handles prerelease suffixes
- **CI/CD Integration**: Properly integrated with GitHub Actions workflows
- **Flexible Options**: Multiple ways to manage versions depending on your workflow
