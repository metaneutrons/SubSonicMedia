# SubSonicMedia

<p align="center">
  <img src="assets/icon.svg" alt="SubSonicMedia Logo" width="64"/>
</p>

[![Build and Test](https://github.com/metaneutrons/SubSonicMedia/actions/workflows/build.yml/badge.svg)](https://github.com/metaneutrons/SubSonicMedia/actions/workflows/build.yml)
[![Status: Beta](https://img.shields.io/badge/Status-Beta-yellow)](https://github.com/metaneutrons/SubSonicMedia/releases)
[![License: GPL-3.0](https://img.shields.io/badge/License-GPL--3.0-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/download)
[![NuGet](https://img.shields.io/nuget/v/SubSonicMedia.svg)](https://www.nuget.org/packages/SubSonicMedia/)
[![GitHub Copilot](https://img.shields.io/badge/GitHub-Copilot-blue?logo=github)](https://github.com/features/copilot)
[![Built with Claude](https://img.shields.io/badge/Built_with-Claude-8A2BE2)](https://claude.ai)

SubSonicMedia is a comprehensive .NET client library for the Subsonic API, supporting API version 1.16.1.

## ✨ Features

- Full implementation of Subsonic API v1.16.1
- Strongly-typed response models
- Interface-based architecture for easy testing and extensibility
- Async support throughout
- Comprehensive documentation

## 🎯 Not Yet Implemented / Roadmap

These Subsonic API methods aren’t exposed yet:

- [ ] scrobble
- [ ] getShares / createShare / updateShare / deleteShare
- [ ] getPodcasts / getNewestPodcasts / refreshPodcasts / createPodcastChannel / deletePodcastChannel / downloadPodcastEpisode
- [ ] jukeboxControl
- [ ] getInternetRadioStations / createInternetRadioStation / updateInternetRadioStation / deleteInternetRadioStation
- [ ] getChatMessages / addChatMessage
- [ ] getUser / getUsers / createUser / updateUser / deleteUser / changePassword
- [ ] getBookmarks / createBookmark / deleteBookmark
- [ ] getPlayQueue / savePlayQueue
- [ ] getScanStatus / startScan

## Getting Started

### Installation

```bash
dotnet add package SubSonicMedia
```

### Example

```csharp
using System;
using SubSonicMedia;

var connection = new SubsonicConnectionInfo(
    serverUrl: "https://your-subsonic-server",
    username: "username",
    password: "password"
);
using var client = new SubsonicClient(connection);

// Ping the server
var pong = await client.PingAsync();
Console.WriteLine($"Server API version: {pong.Version}");
```

### TestKit Integration Test

The **TestKit** is a console application that validates the SubSonicMedia library against a real Subsonic-compatible server:

- Structured API tests with clear pass/fail results
- JSON output for test results
- JUnit XML output for integration with CI/CD systems
- Record server responses for future mocking
- Command-line interface for automation
- npm-style output with UTF-8 icons
- Detailed logging and error reporting

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

## Documentation

Explore the full documentation on [SubSonicMedia Documentation](https://metaneutrons.github.io/SubSonicMedia/).

## License

This project is licensed under the [GNU General Public License v3.0](LICENSE) or later.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request
