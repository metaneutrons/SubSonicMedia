![SubSonicMedia Logo](https://raw.githubusercontent.com/metaneutrons/SubSonicMedia/refs/heads/main/SubSonicMedia/icon.svg)

# SubSonicMedia

[![Build and Test](https://github.com/metaneutrons/SubSonicMedia/actions/workflows/build.yml/badge.svg)](https://github.com/metaneutrons/SubSonicMedia/actions/workflows/build.yml)
[![Publish](https://github.com/metaneutrons/SubSonicMedia/actions/workflows/publish.yml/badge.svg)](https://github.com/metaneutrons/SubSonicMedia/actions/workflows/publish.yml)
[![Status: Beta](https://img.shields.io/badge/Status-Beta-yellow)](https://github.com/metaneutrons/SubSonicMedia/releases)
[![License: GPL-3.0](https://img.shields.io/badge/License-GPL--3.0-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/download)
[![NuGet](https://img.shields.io/nuget/v/SubSonicMedia.svg)](https://www.nuget.org/packages/SubSonicMedia/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/SubSonicMedia.svg)](https://www.nuget.org/packages/SubSonicMedia/)
[![GitHub Copilot](https://img.shields.io/badge/GitHub-Copilot-blue?logo=github)](https://github.com/features/copilot)
[![Built with Claude](https://img.shields.io/badge/Built_with-Claude-8A2BE2)](https://claude.ai)

A comprehensive .NET client library for the Subsonic API, supporting API version 1.16.1.

## 📋 Features

- Full implementation of Subsonic API v1.16.1
- Strongly-typed response models
- Interface-based architecture for easy testing and extensibility
- Async support throughout
- Comprehensive documentation

## 🚀 Getting Started

### Installation

#### Package Manager Console

```powershell
Install-Package SubSonicMedia
```bash

#### .NET CLI

```bash
dotnet add package SubSonicMedia
```bash

#### PackageReference (in .csproj file)

```xml
<PackageReference Include="SubSonicMedia" Version="x.y.z" />
```

You can find the latest version on [NuGet.org](https://www.nuget.org/packages/SubSonicMedia/).

### TestKit Usage

```csharp
// Create a connection to your Subsonic server
var connectionInfo = new SubsonicConnectionInfo
{
    BaseUrl = "https://your-subsonic-server.com",
    Username = "username",
    Password = "password",
    ApiVersion = "1.16.1"
};

// Create the client
var client = new SubsonicClient(connectionInfo);

// Get all artists
var response = await client.Browsing.GetArtists();

// Play a song
var streamUrl = client.Media.GetStreamUrl("songId");
```

## 🧪 TestKit

The project includes a comprehensive TestKit for validating the SubSonicMedia API against your Subsonic server.

### Features

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

# API configuration
SUBSONIC_RESPONSE_FORMAT=json

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

## 🛠️ Development

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

### Detailed Documentation

For detailed development information, please refer to the following documents:

- [Git Hooks Setup](GIT-HOOKS.md) - Information about Git hooks and PowerShell requirements
- [Versioning Process](docs/VERSIONING.md) - Details about our semantic versioning process
- [VS Code Setup](docs/VSCODE.md) - VS Code configuration and recommended extensions

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

## 📄 License

This project is licensed under the [GNU General Public License v3.0](LICENSE) or later.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request
