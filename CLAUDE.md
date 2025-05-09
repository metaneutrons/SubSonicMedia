# SubSonicMedia Development Guide

## Build Commands

- Build solution: `dotnet build`
- Build in Release: `dotnet build -c Release`
- Format code: `dotnet csharpier .`
- Run StyleCop check: `dotnet build /p:TreatWarningsAsErrors=true`
- Fix StyleCop issues: `./scripts/Fix-StyleCopIssues.ps1`
- Run xUnit tests: `dotnet test SubSonicMedia.Tests`
- Run GitVersion: `dotnet gitversion`
- Create and push tag: `./scripts/Create-Tag.ps1` (combines commit and tag push in a single operation)
- Check for shell scripts: `./scripts/Check-ShellScripts.ps1`

## TestKit Commands

- Run all integration tests: `cd SubSonicMedia.TestKit && dotnet run`
- Run specific integration test: `cd SubSonicMedia.TestKit && dotnet run test "Connection Test"`
- List available integration tests: `cd SubSonicMedia.TestKit && dotnet run list`
- Display TestKit help: `cd SubSonicMedia.TestKit && dotnet run help`

## Code Style Guidelines

- **Formatting**: 4-space indentation, no tabs, newline at end of file (see stylecop.json)
- **Naming**: PascalCase for types/methods, camelCase for params/variables, prefix private fields with underscore
- **Imports**: System imports first, then other namespaces with blank line between groups, placed outside namespace
- **Organization**: Interfaces in Interfaces/, responses in Responses/, implementations in Clients/
- **Documentation**: XML docs for public members with `summary` and parameter docs
- **Error Handling**: Use SubsonicApiException variants, wrap unexpected exceptions
- **Nullability**: Enable nullable reference types, validate inputs with guard clauses
- **API Pattern**: Follow Subsonic API spec exactly, maintain backward compatibility
- **License**: All code files must have GPL-3.0 license headers

## Git Workflow

- Uses Husky.Net for Git hooks (see docs/GIT-HOOKS.md for details)
- Follow conventional commits: `<type>(scope): message` (e.g., `feat(api): add bookmark support`)
- Types: feat, fix, docs, style, refactor, test, chore, build, ci, perf, revert
- Breaking changes: add `!` suffix to type or include `BREAKING CHANGE:` in footer
- Pre-commit hooks format code, check for GPL-3.0 headers, and validate style rules
- Pre-push hooks run full build with warnings as errors
- See docs/VERSIONING.md for semantic versioning details

## CI/CD

- GitHub Actions for automated build, test, and NuGet package publishing
- Automatic version bumps based on conventional commits
- Package publishing triggered by version tags

### GitHub Workflows

- **Build and Test** (.github/workflows/build.yml)
  - Triggered on push to main/develop branches and PRs
  - Intelligently skips builds for commits that have version tags
  - Prevents duplicate builds when commits and tags are pushed together
  - Validates code with StyleCop and CSharpier
  - Builds solution and runs basic tests
  - Creates NuGet packages for verification

- **Tag Release** (.github/workflows/tag-release.yml)
  - Triggered by version tags (v*)
  - Builds and validates code
  - Creates GitHub releases with NuGet packages
  - Labels releases as Beta or Release based on tag format

- **Update Main Branch** (.github/workflows/update-main.yml)
  - Triggered by version tags (v*) created from the develop branch
  - Only proceeds if tag originates from develop branch
  - Updates main branch to match the latest tag
  - Follows specific rules for beta vs. stable releases
  - Requires PAT_TOKEN with workflows permission

- **NuGet Publish** (.github/workflows/nuget-publish.yml)
  - Manually triggered with a specific version tag
  - Creates an approval issue requiring admin confirmation
  - Publishes packages to NuGet.org only after explicit approval
  - Provides full audit trail of who approved the publish

- **Dependabot**
  - Auto-updates NuGet packages (weekly)
  - Auto-updates GitHub Actions (monthly)
  - Uses conventional commit format

### GitHub Templates

- **Issue Templates**
  - Bug report template (.github/ISSUE_TEMPLATE/bug_report.md)
  - Feature request template (.github/ISSUE_TEMPLATE/feature_request.md)

- **Pull Request Template** (.github/pull_request_template.md)
  - Structured checklist for contributors
  - Ensures code quality standards are met

- **CODEOWNERS** (.github/CODEOWNERS)
  - Automatically assigns reviewers based on file paths

## Configuration

- .env file required for testing with required keys: SUBSONIC_SERVER_URL, SUBSONIC_USERNAME, SUBSONIC_PASSWORD
- Optional config: SUBSONIC_API_VERSION, RECORD_TEST_RESULTS, OUTPUT_DIRECTORY, FAIL_FAST, JUNIT_XML_OUTPUT

## Target Framework

- net8.0 for both library and test kit
- Modern C# features via LangVersion=latest
