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
- Bump version: `./scripts/bump-version.sh` or `./scripts/Bump-Version.ps1`

## Code Style Guidelines
- **Formatting**: 4-space indentation, no tabs, newline at end of file
- **Naming**: PascalCase for types/methods, camelCase for params/variables, prefix private fields with underscore
- **Imports**: System imports first, then other namespaces with blank line between groups
- **Organization**: Interfaces in Interfaces/, responses in Responses/, implementations in Clients/
- **Documentation**: XML docs for public members with <summary> and parameter docs
- **Error Handling**: Use SubsonicApiException variants, wrap unexpected exceptions
- **Nullability**: Enable nullable reference types, validate inputs with guard clauses
- **API Pattern**: Follow Subsonic API spec exactly, maintain backward compatibility
- **License**: All code files must have GPL-3.0 license headers

## Git Workflow
- Follow conventional commits: `<type>(scope): message` (e.g., `feat(api): add bookmark support`)
- Types: feat, fix, docs, style, refactor, test, chore, build, ci, perf, revert
- Pre-commit hooks format code and check for GPL-3.0 headers
- Pre-push hooks run full build with warnings as errors

## Configuration
- .env file required for testing with required keys: SUBSONIC_SERVER_URL, SUBSONIC_USERNAME, SUBSONIC_PASSWORD
- Optional config: SUBSONIC_API_VERSION, SUBSONIC_RESPONSE_FORMAT, RECORD_TEST_RESULTS, OUTPUT_DIRECTORY

## Target Framework
- net8.0 for both library and test kit
- Modern C# features via LangVersion=latest