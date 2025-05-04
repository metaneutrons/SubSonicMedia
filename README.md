# SubSonicMedia

<p align="center">
  <img src="assets/logo.svg" alt="SubSonicMedia Logo" width="64"/>
</p>

[![Build and Test](https://github.com/metaneutrons/SubSonicMedia/actions/workflows/build.yml/badge.svg)](https://github.com/metaneutrons/SubSonicMedia/actions/workflows/build.yml)
[![Status: Beta](https://img.shields.io/badge/Status-Beta-yellow)](https://github.com/metaneutrons/SubSonicMedia/releases)
[![License: GPL-3.0](https://img.shields.io/badge/License-GPL--3.0-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/download)
[![NuGet](https://img.shields.io/nuget/v/SubSonicMedia.svg)](https://www.nuget.org/packages/SubSonicMedia/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/SubSonicMedia.svg)](https://www.nuget.org/packages/SubSonicMedia/)
[![GitHub Copilot](https://img.shields.io/badge/GitHub-Copilot-blue?logo=github)](https://github.com/features/copilot)
[![Built with Claude](https://img.shields.io/badge/Built_with-Claude-8A2BE2)](https://claude.ai)

SubSonicMedia is a comprehensive .NET client library for the Subsonic API, supporting API version 1.16.1.

## Features

- Full implementation of Subsonic API v1.16.1
- Strongly-typed response models
- Interface-based architecture for easy testing and extensibility
- Async support throughout
- Comprehensive documentation

## Getting Started

### Installation

```powershell
Install-Package SubSonicMedia
```

```bash
dotnet add package SubSonicMedia
```

```xml
<PackageReference Include="SubSonicMedia" Version="x.y.z" />
```

### TestKit Usage

The **TestKit** is a console application that validates the SubSonicMedia library against a real Subsonic-compatible server.

1. Navigate to the TestKit directory and copy the example environment file:

    ```bash
    cd SubSonicMedia.TestKit
    cp .env.example .env
    ```

2. Open `.env` and set your server connection info:

    ```env
    SUBSONIC_SERVER_URL=https://your-subsonic-server.com
    SUBSONIC_USERNAME=your-username
    SUBSONIC_PASSWORD=your-password
    API_VERSION=1.16.1
    RECORD_TEST_RESULTS=true
    OUTPUT_DIRECTORY=./TestResults
    JUNIT_XML_OUTPUT=true
    ```

3. Run the TestKit application:

    ```bash
    dotnet run
    ```

- Use `dotnet run -- --junit-xml` for JUnit XML output.
- Use `dotnet run list` to list available tests.
- Use `dotnet run help` for full CLI options.

## Testing

The project includes two complementary testing approaches:

### Unit Tests (SubSonicMedia.Tests)

Standard xUnit tests for the library with mocked API responses:

- Fast execution with no external dependencies
- WireMock.NET for API response simulation
- FluentAssertions for readable test assertions
- Uses recorded responses from the TestKit
- Ideal for CI/CD pipelines and rapid local testing

    ```bash
    # Run the unit tests
    dotnet test SubSonicMedia.Tests
    ```

### Integration Tests (SubSonicMedia.TestKit)

A comprehensive TestKit for validating against a real Subsonic server:

- Structured API tests with clear pass/fail results
- JSON output for test results
- JUnit XML output for integration with CI/CD systems
- Record server responses for future mocking
- Command-line interface for automation
- npm-style output with UTF-8 icons
- Detailed logging and error reporting

### Configuration

The TestKit uses a `.env` file for configuration. Copy the `.env.example` file to `.env` in the TestKit directory and update the following settings:

    ```env
    # Server information
    SUBSONIC_SERVER_URL=https://your-server-url
    SUBSONIC_USERNAME=your-username
    SUBSONIC_PASSWORD=your-password

    # Test configuration
    RECORD_TEST_RESULTS=true
    OUTPUT_DIRECTORY=./TestResults
    JUNIT_XML_OUTPUT=true
    ```

### Usage

```bash
# Navigate to the TestKit directory
cd SubSonicMedia.TestKit

# Run all tests
dotnet run

# Run a specific test
dotnet run test "Connection Test"

# Run with JUnit XML output
dotnet run -- --junit-xml

# List available tests
dotnet run list

# Get help
dotnet run help
```

## Documentation

Explore the full documentation on [SubSonicMedia Documentation](https://metaneutrons.github.io/SubSonicMedia/).

## Development

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022, VS Code, or JetBrains Rider
- PowerShell 7.0 or later

### AI Development Tools

We use [Claude Code](https://claude.ai/code) (Anthropic's AI assistant, see [CLAUDE.md](CLAUDE.md) and [GitHub Copilot](https://github.com/features/copilot) to accelerate development tasks, code reviews and documentation. This helps us maintain consistent code quality and speed up development workflows.

### Building

```bash
# Clone the repository
git clone https://github.com/metaneutrons/SubSonicMedia.git
cd SubSonicMedia

# Build the solution
dotnet build
```

### Versioning

This project follows semantic versioning with automated version bumping based on conventional commit messages.

We provide tools to automatically analyze commit messages and determine the appropriate version bump (major, minor, patch).

See [docs/VERSIONING.md](docs/VERSIONING.md) for details on:

- Commit message conventions
- Local version bump PowerShell script
- GitHub workflow for automated versioning
- Complete release process

### VS Code Setup

The repository includes a VS Code setup with:

- Build tasks for the library and TestKit
- Debug configurations for running and debugging
- Recommended extensions and settings

See [docs/VSCODE.md](docs/VSCODE.md) for details.

## License

This project is licensed under the [GNU General Public License v3.0](LICENSE) or later.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request
