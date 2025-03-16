# Semantic Versioning in SubSonicMedia

This project follows [Semantic Versioning](https://semver.org/) principles and uses commit message conventions to automatically determine version bumps.

## Commit Message Convention

We use a simplified version of the [Conventional Commits](https://www.conventionalcommits.org/) specification:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### Types that affect versioning

| Type | Description | Version Impact |
|------|-------------|--------------|
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

## Automatic Version Bumping

The project includes multiple ways to analyze commit messages and determine the appropriate version bump:

1. **Major version bump (1.0.0 → 2.0.0)** when:
   - A commit message contains `BREAKING CHANGE:`
   - A commit type has the `!` suffix (e.g., `feat!:`, `fix!:`)

2. **Minor version bump (1.0.0 → 1.1.0)** when:
   - A commit message starts with `feat:`

3. **Patch version bump (1.0.0 → 1.0.1)** when:
   - Any other type of commit is detected

## Version Bump Tools

You have three options for managing version bumps:

### 1. Local Scripts (Recommended for Development)

#### Unix/macOS (Bash)

```bash
# Analyze commits and suggest a version bump
./scripts/bump-version.sh

# Apply the suggested version bump
./scripts/bump-version.sh --apply

# Force a specific bump type
./scripts/bump-version.sh --force-bump minor --apply
```

#### Windows (PowerShell)

```powershell
# Analyze commits and suggest a version bump
.\scripts\Bump-Version.ps1

# Apply the suggested version bump
.\scripts\Bump-Version.ps1 -Apply

# Force a specific bump type
.\scripts\Bump-Version.ps1 -ForceBump minor -Apply
```

After running with the apply option, you'll need to push the changes:
```bash
git push && git push --tags
```

### 2. GitHub Workflow (Recommended for CI/CD)

The workflow can be triggered manually from the GitHub Actions tab:

1. Go to the "Actions" tab in your GitHub repository
2. Select the "Semantic Version Bump" workflow
3. Click "Run workflow"
4. Options:
   - **Apply version bump and create tag**: When set to `true`, the workflow will:
     - Update the version in Directory.Build.props
     - Commit the change
     - Create a new version tag
     - Push both to the repository
   - **Force bump type**: Override the automatic detection with a specific bump type (auto, patch, minor, major)

### 3. Manual Version Management

If you prefer to manage versions manually:

1. Edit the version in `SubSonicMedia/Directory.Build.props`
2. Commit and push the change
3. Create and push a tag: `git tag v1.2.3 && git push origin v1.2.3`

## Complete Release Process

1. Make your changes and commit them using the conventional commit format
2. Push your changes to GitHub
3. Use one of the version bump methods above to:
   - Analyze commits since the last tag
   - Determine the appropriate version bump
   - Update the version in Directory.Build.props
   - Create and push a new version tag
4. The tag push will trigger the "Build and Publish NuGet Package" workflow
5. If all checks pass, the package will be published to NuGet.org

## Integration with CI/CD

The complete flow integrates with your CI/CD pipeline:

1. Developers make commits with conventional commit messages
2. When ready for release, the version is bumped using one of the tools
3. The tag creation triggers the publish workflow
4. The package is built, tested, and published to NuGet

This automated process ensures consistent versioning and reduces manual work while maintaining control over when releases happen.
