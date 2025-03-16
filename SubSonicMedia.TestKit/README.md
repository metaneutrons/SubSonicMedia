# SubSonicMedia TestKit

A comprehensive testing tool for the SubSonicMedia API. This tool provides structured output in npm-style format with UTF-8 icons and can be integrated into your release process and CI/CD pipelines.

## Features

- Structured API tests with clear pass/fail/skip results
- JSON output for test results and recording server responses for future mocking
- JUnit XML reports for CI/CD integration
- Command-line interface for automation with exit codes for CI/CD
- Fail-fast option for quicker development feedback
- Smart skipping of tests for unavailable API features
- npm-style output with UTF-8 icons
- Detailed logging and error reporting

## Configuration

The TestKit uses a `.env` file for configuration. Copy the `.env.example` file to `.env` and update the following settings:

```
# Server information
SUBSONIC_SERVER_URL=https://your-server-url
SUBSONIC_USERNAME=your-username
SUBSONIC_PASSWORD=your-password

# API configuration
SUBSONIC_RESPONSE_FORMAT=json

# Test configuration
RECORD_TEST_RESULTS=true
OUTPUT_DIRECTORY=./TestResults
```

## Usage

### Running all tests

```bash
dotnet run
```

### Running a specific test

```bash
dotnet run run "Connection Test"
```

### Listing available tests

```bash
dotnet run list
```

### Help

```bash
dotnet run help
```

## Test Output

The test results are displayed in the console with UTF-8 icons:

- ✓ - Test passed (green)
- ✗ - Test failed (red)
- ⚠ - Test skipped (yellow)
- ℹ - Information
- 🧪 - Test execution indicator

If `RECORD_TEST_RESULTS` is set to `true`, API responses will be saved as JSON files in the `OUTPUT_DIRECTORY` for future reference and mocking.

## Test Results and Exit Codes

The TestKit uses the following test result states:

| State   | Description                                         | Console Indicator | Exit Code |
|---------|-----------------------------------------------------|-------------------|-----------|
| Pass    | Test executed successfully                          | Green ✓           | 0         |
| Skipped | Test skipped due to feature not being available     | Yellow ⚠          | 0         |
| Fail    | Test failed during execution                        | Red ✗             | 1         |

### Exit Codes

- **0**: All tests passed or were skipped (no failures)
- **1**: One or more tests failed
- **2**: Other errors (e.g., configuration errors)

This approach allows CI/CD pipelines to differentiate between actual test failures and tests that were skipped due to feature unavailability.

### When Tests Are Skipped

Tests are automatically skipped when:
- The API returns a "Not Implemented" response
- The API returns a "Gone" response
- The API returns error code 70 (which is the Subsonic API code for "not implemented")

This allows your test suite to work with various Subsonic server implementations that may not support all API features.

## Integration with CI/CD

The TestKit is designed to integrate seamlessly with CI/CD pipelines and git hooks. It provides multiple integration points:

### Exit Codes

The TestKit uses standard exit codes that can be checked in your CI/CD pipeline:

```bash
dotnet run
if [ $? -ne 0 ]; then
    echo "Tests failed, aborting release"
    exit 1
fi
```

### JUnit XML Reports

For better CI/CD integration, you can enable JUnit XML output by setting `JUNIT_XML_OUTPUT=true` in your .env file:

```
JUNIT_XML_OUTPUT=true
```

This generates a JUnit XML report in the configured output directory that can be consumed by most CI/CD platforms including:
- Jenkins
- GitLab CI
- GitHub Actions
- Azure DevOps
- CircleCI

The report includes detailed test information including:
- Test duration in seconds
- Skipped tests with skip reason
- Failed tests with error details
- Test suite summary with pass/fail/skip counts

### Git Hooks Integration

You can integrate the TestKit with git hooks by using the provided scripts that conditionally run tests only if the .env file exists:

#### Windows (PowerShell)

Create a `.git/hooks/pre-commit` file with the following content:

```powershell
#!/usr/bin/env pwsh
# Pre-commit hook to run tests

$ErrorActionPreference = "Stop"
& "$PSScriptRoot/../../scripts/Run-Tests.ps1" -PreCommit
exit $LASTEXITCODE
```

#### Unix/macOS/Linux

Create a `.git/hooks/pre-commit` file with the following content:

```bash
#!/usr/bin/env pwsh
# Pre-commit hook to run tests

& "$(git rev-parse --show-toplevel)/scripts/Run-Tests.ps1" -PreCommit
exit $LASTEXITCODE
```

Make the hook executable:

```bash
chmod +x .git/hooks/pre-commit
```

#### How it Works

The provided scripts in the `scripts/` directory:
- Check if the `.env` file exists in the TestKit directory
- Only run tests if the configuration exists
- Display helpful messages if tests are skipped
- Properly handle exit codes for CI/CD integration
- Allow temporarily disabling tests by renaming `.env` to `.env.disabled`

#### MSBuild Integration

You can also run tests as part of the build process using the custom MSBuild target included in the project file. This target automatically runs TestKit if the `.env` file exists:

```xml
<Target Name="RunTestsConditionally" Condition="Exists('$(MSBuildProjectDirectory)/.env')">
  <Message Importance="high" Text="Running SubSonicMedia TestKit..." />
  <Exec Command="dotnet run" WorkingDirectory="$(MSBuildProjectDirectory)" IgnoreExitCode="true">
    <Output TaskParameter="ExitCode" PropertyName="ErrorCode" />
  </Exec>
  <Message Importance="high" Text="Tests completed successfully!" Condition="'$(ErrorCode)' == '0'" />
  <Error Text="Tests failed with exit code $(ErrorCode)" Condition="'$(ErrorCode)' != '0'" />
</Target>
```

To run tests after every build, add this to your CI/CD build step:

```bash
dotnet msbuild /t:RunTestsConditionally SubSonicMedia.TestKit/SubSonicMedia.TestKit.csproj
```

### Fail Fast Mode

For quicker development feedback, enable "fail fast" mode with `FAIL_FAST=true` in your .env file:

```
FAIL_FAST=true
```

This causes the test runner to exit immediately upon encountering the first failed test, saving time during local development.

## License

SubSonicMedia is licensed under the GNU General Public License v3.0.
