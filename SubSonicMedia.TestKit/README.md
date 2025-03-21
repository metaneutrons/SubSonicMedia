# SubSonicMedia.TestKit

[![Build and Test](https://github.com/metaneutrons/SubSonicMedia/actions/workflows/build.yml/badge.svg)](https://github.com/metaneutrons/SubSonicMedia/actions/workflows/build.yml)
[![License: GPL-3.0](https://img.shields.io/badge/License-GPL--3.0-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/download)
[![NuGet](https://img.shields.io/nuget/v/SubSonicMedia.TestKit.svg)](https://www.nuget.org/packages/SubSonicMedia.TestKit/)

A comprehensive testing toolkit for validating the SubSonicMedia API against your Subsonic server.

## 🧪 Features

- Structured API tests with clear pass/fail results
- JSON output for test results
- JUnit XML output for integration with CI/CD systems
- Record server responses for future mocking
- Command-line interface for automation
- npm-style output with UTF-8 icons
- Detailed logging and error reporting

## 🚀 Getting Started

### Installation

#### Package Manager Console

```powershell
Install-Package SubSonicMedia.TestKit
```

#### .NET CLI

```bash
dotnet add package SubSonicMedia.TestKit
```

#### PackageReference (in .csproj file)

```xml
<PackageReference Include="SubSonicMedia.TestKit" Version="x.y.z" />
```

You can find the latest version on [NuGet.org](https://www.nuget.org/packages/SubSonicMedia.TestKit/).

### Configuration

The TestKit uses a `.env` file for configuration. Copy the `.env.example` file to `.env` in your project directory and update the following settings:

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

## 📄 License

This project is licensed under the [GNU General Public License v3.0](https://github.com/metaneutrons/SubSonicMedia/blob/main/LICENSE) or later.

## 📚 Documentation

For more detailed information about the SubSonicMedia library and TestKit, please refer to the [main repository documentation](https://github.com/metaneutrons/SubSonicMedia).
