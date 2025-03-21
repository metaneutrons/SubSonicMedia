# Using GitVersion for Versioning and Releases

This project uses [GitVersion](https://gitversion.net/) for versioning, which automates semantic versioning based on Git history and conventional commits.

## How It Works

GitVersion analyzes your branch structure and commit messages to determine the appropriate version number for your project. The configuration is set to `ManualDeployment` mode, which gives you full control over when version numbers change.

## Local Development

1. Install GitVersion tool:
   ```
   dotnet tool install --global GitVersion.Tool
   ```

2. Run GitVersion to see the current version:
   ```
   dotnet gitversion
   ```

3. To update build files with GitVersion info:
   ```
   dotnet gitversion /updateprojectfiles
   ```

## Manual Release Process

We use a manual release process to ensure control over when and how versions are incremented:

1. Make your changes and commit them using [conventional commits](https://www.conventionalcommits.org/):
   ```
   feat(api): add new endpoint
   fix(auth): resolve token validation issue
   ```

2. When ready to update the version:
   - Go to GitHub Actions
   - Run the "GitVersion Update" workflow
   - Choose whether to create a tag

3. If you created a tag, the "Build and Publish NuGet Package" workflow will run automatically.

4. Alternatively, trigger the "Build and Publish NuGet Package" workflow manually to publish without creating a new tag.

## GitVersion Configuration

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

## Tag Naming Convention

Tags should follow the format `v{version}` (e.g., `v1.2.3` or `v1.2.3-beta.1`).

## Integration with MSBuild

GitVersion is integrated with the build system via:

1. `GitVersion.props`: Contains version properties used by MSBuild
2. `GitVersionTarget.targets`: MSBuild target that runs GitVersion
3. `Directory.Build.props`: Imports GitVersion properties

This ensures consistent versioning across local and CI builds.