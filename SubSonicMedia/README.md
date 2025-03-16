# SubSonicMedia

A comprehensive .NET client library for the Subsonic API, supporting API version 1.16.1.

## Features

- Full implementation of Subsonic API v1.16.1
- Strongly-typed response models
- Interface-based architecture for easy testing and extensibility
- Async support throughout
- Comprehensive documentation

## Getting Started

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

## License

This project is licensed under the GNU General Public License v3.0 or later.
