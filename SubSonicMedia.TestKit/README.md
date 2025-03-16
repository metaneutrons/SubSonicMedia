# SubSonicMedia TestKit

A comprehensive testing tool for the SubSonicMedia API. This tool provides structured output in npm-style format with UTF-8 icons and can be integrated into your release process.

## Features

- Structured API tests with clear pass/fail results
- JSON output for test results
- Record server responses for future mocking
- Command-line interface for automation
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

- ✓ - Test passed
- ✗ - Test failed
- ℹ - Information
- ⚠ - Warning

If `RECORD_TEST_RESULTS` is set to `true`, API responses will be saved as JSON files in the `OUTPUT_DIRECTORY` for future reference and mocking.

## Integration with Release Process

You can include this TestKit in your CI/CD pipeline by running the tests and checking the exit code. The application returns:

- 0 - All tests passed
- 1 - One or more tests failed

Example:

```bash
dotnet run
if [ $? -ne 0 ]; then
    echo "Tests failed, aborting release"
    exit 1
fi
```

## License

SubSonicMedia is licensed under the GNU General Public License v3.0.
