# Versioning in SubSonicMedia

This project follows [Semantic Versioning](https://semver.org/) principles and uses both GitVersion and commit message conventions to automatically determine version bumps.

## GitVersion Integration

The project now uses [GitVersion](https://gitversion.net/) to automatically generate version numbers based on Git history. This eliminates the need to manually update version numbers in multiple places.

### How GitVersion Works

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

### GitVersion

With GitVersion integrated into the build process, you can:

1. **Install GitVersion**:
   ```
   dotnet tool install --global GitVersion.Tool
   ```

2. **Trigger Version Updates**:
   - Create a new Git tag: `git tag v1.2.3`
   - Use commit message conventions:
     - `+semver: major` - Bump major version
     - `+semver: minor` - Bump minor version
     - `+semver: patch` - Bump patch version

3. **For CI/CD**:
   GitVersion is already integrated into the CI/CD pipeline:
   - The build workflow automatically runs GitVersion during builds
   - The GitVersion workflow can be manually triggered for version updates

### GitHub Workflow

The GitVersion workflow can be triggered manually from the GitHub Actions tab:

1. Go to the "Actions" tab in your GitHub repository
2. Select the "GitVersion Update" workflow
3. Click "Run workflow"
4. Options:
   - **Create and push Git tag**: When set to `true`, the workflow will create and push a Git tag based on the GitVersion-generated version

### Manual Version Management

If you prefer to manage versions manually:

1. Edit the version in `SubSonicMedia/Directory.Build.props`
2. Commit and push the change
3. Create and push a tag: `git tag v1.2.3 && git push origin v1.2.3`

## Complete Release Process

1. Make your changes and commit them using the conventional commit format
2. Push your changes to GitHub
3. Use one of the version management options above to:
   - Analyze commits since the last tag
   - Determine the appropriate version bump
   - Update the version in Directory.Build.props
   - Create and push a new version tag
4. The tag push will trigger the "Build and Publish NuGet Package" workflow
5. If all checks pass, the package will be published to NuGet.org

## API Version

The Subsonic API version supported by this library is specified separately in `Directory.Build.props` as `<SubsonicApiVersion>`.

## Benefits

- **Single Source of Truth**: Version information is derived from Git
- **Automatic Updates**: No need to manually update version numbers
- **Proper NuGet Versioning**: Correctly handles prerelease suffixes
- **Assembly Version Compliance**: Ensures assembly versions follow .NET requirements
- **Flexible Options**: Multiple ways to manage versions depending on your workflow
- **CI/CD Integration**: Seamless integration with your build pipeline
