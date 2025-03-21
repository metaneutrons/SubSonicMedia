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
workflow: GitFlow/v1
mode: ManualDeployment
branches:
  main:
    label: rc
  support:
    label: beta
```

This configuration uses:
- `ManualDeployment` mode: Version is only incremented when you explicitly trigger it
- GitFlow branching: Supports feature, develop, release, and main branches
- Custom labels: Uses appropriate prerelease labels based on branch type

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

## Version Management Options

### Local Development

1. **Install GitVersion**:
   ```
   dotnet tool install --global GitVersion.Tool
   ```

2. **View Version Information**:
   ```
   dotnet gitversion
   ```

3. **Update Build Files**:
   ```
   dotnet gitversion /updateprojectfiles
   ```

4. **Trigger Version Updates**:
   - Create a new Git tag: `git tag v1.2.3`
   - Use commit message conventions:
     - `+semver: major` - Bump major version
     - `+semver: minor` - Bump minor version
     - `+semver: patch` - Bump patch version

### Manual Release Process

We use a manual release process to ensure control over when and how versions are incremented:

1. **Option 1: GitHub Actions Workflow**
   
   The "GitVersion Update" workflow can be triggered manually from the GitHub Actions tab:
   
   1. Go to the "Actions" tab in your GitHub repository
   2. Select the "GitVersion Update" workflow
   3. Click "Run workflow"
   4. Options:
      - **Create and push Git tag**: When set to `true`, the workflow will create and push a Git tag based on the GitVersion-generated version

2. **Option 2: Local Tag Creation**
   
   Use the Create-Tag.ps1 script that now integrates with GitVersion:

   ```
   ./scripts/Create-Tag.ps1
   ```

   This script will:
   - Use GitVersion to determine the current version
   - Perform pre-checks (no uncommitted files)
   - Create and push a Git tag after confirmation

3. **Manual Version Management (Fallback)**

   If you prefer to manage versions manually:

   1. Edit the version in `SubSonicMedia/Directory.Build.props`
   2. Commit and push the change
   3. Create and push a tag: `git tag v1.2.3 && git push origin v1.2.3`

## Complete Release Process

1. Make your changes and commit them using the conventional commit format
2. Push your changes to GitHub
3. Use one of the version management options above to:
   - Use GitVersion to determine the appropriate version
   - Update version information in the build files
   - Create and push a new version tag
4. The tag push will trigger the "Build and Publish NuGet Package" workflow
5. Alternatively, you can trigger the publish workflow manually:
   - Go to the "Actions" tab in your GitHub repository
   - Select the "Build and Publish NuGet Package" workflow
   - Click "Run workflow"
   - Choose whether to publish to NuGet.org
6. If all checks pass, the package will be published to NuGet.org

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
