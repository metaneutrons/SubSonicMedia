# SubSonicMedia Development Guide

## Build Commands

- Build solution: `dotnet build`
- Build in Release: `dotnet build -c Release`
- Format code: `dotnet csharpier .`
- Run StyleCop check: `dotnet build /p:TreatWarningsAsErrors=true`
- Fix StyleCop issues: `./scripts/Fix-StyleCopIssues.ps1`
- Run all tests: `cd SubSonicMedia.TestKit && dotnet run`
- Run specific test: `cd SubSonicMedia.TestKit && dotnet run run "Connection Test"`
- List available tests: `cd SubSonicMedia.TestKit && dotnet run list`
- Display test help: `cd SubSonicMedia.TestKit && dotnet run help`
- Bump version: `./scripts/Bump-Version.ps1 -Apply`
- Check for shell scripts: `./scripts/Check-ShellScripts.ps1`

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

## Configuration

- .env file required for testing with required keys: SUBSONIC_SERVER_URL, SUBSONIC_USERNAME, SUBSONIC_PASSWORD
- Optional config: SUBSONIC_API_VERSION, SUBSONIC_RESPONSE_FORMAT, RECORD_TEST_RESULTS, OUTPUT_DIRECTORY, FAIL_FAST, JUNIT_XML_OUTPUT

## Target Framework

- net8.0 for both library and test kit
- Modern C# features via LangVersion=latest
