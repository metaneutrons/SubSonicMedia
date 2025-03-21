# SubSonicMedia Library Documentation

## Overview

SubSonicMedia is a C# library that provides easy integration with the Subsonic API. The library offers a set of client classes that cover various aspects of the Subsonic API, as well as response classes that represent the data returned by the API.

## Table of Contents

- [Basic Usage](#basic-usage)
- [Authentication](#authentication)
- [Clients](#clients)
  - [SubsonicClient](#subsonicclient)
  - [BrowsingClient](#browsingclient)
  - [SystemClient](#systemclient)
  - [PlaylistClient](#playlistclient)
  - [UserClient](#userclient)
  - [SearchClient](#searchclient)
  - [MediaClient](#mediaclient)
  - [RadioClient](#radioclient)
  - [JukeboxClient](#jukeboxclient)
  - [PodcastClient](#podcastclient)
  - [BookmarkClient](#bookmarkclient)
  - [ChatClient](#chatclient)
  - [VideoClient](#videoclient)
  - [UserManagementClient](#usermanagementclient)
- [Response Classes](#response-classes)
- [Models](#models)
- [Utilities](#utilities)
- [Error Handling](#error-handling)
- [Examples](#examples)

## Basic Usage

```csharp
// Creating a SubsonicClient
var connectionInfo = new SubsonicConnectionInfo("http://yourserver", "username", "password");
var client = new SubsonicClient(connectionInfo);

// Using a specific client
var browsingClient = client.Browsing;
var genres = await browsingClient.GetGenresAsync();

// Extracting data
foreach (var genre in genres.Genres.Genre)
{
    Console.WriteLine($"Genre: {genre.Name}, Song Count: {genre.SongCount}");
}
```

## Authentication

SubSonicMedia supports multiple authentication methods offered by the Subsonic API:

- Legacy Authentication (Plaintext Password)
- Token Authentication (Salt + MD5 Hashing)

The library automatically uses the most secure available method, but you can explicitly set it.

```csharp
var connectionInfo = new SubsonicConnectionInfo(
    "http://yourserver",
    "username",
    "password",
    SubsonicAuthenticationMethod.Legacy
);
```

## Clients

### SubsonicClient

The `SubsonicClient` is the main class that provides access to all API functionalities. It manages the connection and provides specialized clients for different API areas.

```csharp
public class SubsonicClient : IDisposable
{
    // Constructors
    public SubsonicClient(SubsonicConnectionInfo connectionInfo);
    public SubsonicClient(SubsonicConnectionInfo connectionInfo, HttpClient httpClient);
    public SubsonicClient(SubsonicConnectionInfo connectionInfo, IAuthenticationProvider authProvider);
    public SubsonicClient(SubsonicConnectionInfo connectionInfo, HttpClient httpClient, IAuthenticationProvider authProvider);

    // Properties for specific clients
    public IBrowsingClient Browsing { get; }
    public ISystemClient System { get; }
    public IPlaylistClient Playlists { get; }
    public IUserClient User { get; }
    public ISearchClient Search { get; }
    public IMediaClient Media { get; }
    public IRadioClient Radio { get; }
    public IJukeboxClient Jukebox { get; }
    public IPodcastClient Podcasts { get; }
    public IBookmarkClient Bookmarks { get; }
    public IChatClient Chat { get; }
    public IVideoClient Video { get; }
    public IUserManagementClient UserManagement { get; }

    // API methods
    public Task<T> ExecuteRequestAsync<T>(string endpoint, Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default) where T : SubsonicResponse, new();
    public Task<Stream> GetBinaryAsync(string endpoint, Dictionary<string, string> parameters, CancellationToken cancellationToken = default);

    // Helper and management methods
    public void Dispose();
}
```

### BrowsingClient

The `BrowsingClient` is used to browse the music library on the Subsonic server.

```csharp
public interface IBrowsingClient
{
    Task<MusicFoldersResponse> GetMusicFoldersAsync(CancellationToken cancellationToken = default);

    Task<IndexesResponse> GetIndexesAsync(
        string? musicFolderId = null,
        DateTime? ifModifiedSince = null,
        CancellationToken cancellationToken = default
    );

    Task<MusicDirectoryResponse> GetMusicDirectoryAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<GenresResponse> GetGenresAsync(CancellationToken cancellationToken = default);

    Task<ArtistsResponse> GetArtistsAsync(
        string? musicFolderId = null,
        CancellationToken cancellationToken = default
    );

    Task<ArtistResponse> GetArtistAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<AlbumResponse> GetAlbumAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<SongResponse> GetSongAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<VideosResponse> GetVideosAsync(CancellationToken cancellationToken = default);

    Task<VideoInfoResponse> GetVideoInfoAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<AlbumList2Response> GetAlbumListAsync(
        AlbumListType type,
        int? size = null,
        int? offset = null,
        int? fromYear = null,
        int? toYear = null,
        string? genre = null,
        string? musicFolderId = null,
        CancellationToken cancellationToken = default
    );

    Task<RandomSongsResponse> GetRandomSongsAsync(
        int? size = null,
        string? genre = null,
        int? fromYear = null,
        int? toYear = null,
        string? musicFolderId = null,
        CancellationToken cancellationToken = default
    );

    Task<SongsByGenreResponse> GetSongsByGenreAsync(
        string genre,
        int? count = null,
        int? offset = null,
        string? musicFolderId = null,
        CancellationToken cancellationToken = default
    );

    Task<StarredResponse> GetStarredAsync(
        string? musicFolderId = null,
        CancellationToken cancellationToken = default
    );

    Task<Starred2Response> GetStarred2Async(
        string? musicFolderId = null,
        CancellationToken cancellationToken = default
    );

    Task<ArtistInfoResponse> GetArtistInfoAsync(
        string id,
        int? count = null,
        bool? includeNotPresent = null,
        CancellationToken cancellationToken = default
    );

    Task<ArtistInfo2Response> GetArtistInfo2Async(
        string id,
        int? count = null,
        bool? includeNotPresent = null,
        CancellationToken cancellationToken = default
    );

    Task<AlbumInfoResponse> GetAlbumInfoAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<AlbumInfo2Response> GetAlbumInfo2Async(
        string id,
        CancellationToken cancellationToken = default
    );
}
```

### SystemClient

The `SystemClient` is used for system operations of the Subsonic server.

```csharp
public interface ISystemClient
{
    Task<BaseResponse> PingAsync(CancellationToken cancellationToken = default);

    Task<LicenseResponse> GetLicenseAsync(CancellationToken cancellationToken = default);

    Task<ScanStatusResponse> GetScanStatusAsync(CancellationToken cancellationToken = default);

    Task<ScanStatusResponse> StartScanAsync(CancellationToken cancellationToken = default);
}
```

### PlaylistClient

The `PlaylistClient` is used for managing playlists.

```csharp
public interface IPlaylistClient
{
    Task<PlaylistsResponse> GetPlaylistsAsync(CancellationToken cancellationToken = default);

    Task<PlaylistResponse> GetPlaylistAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> CreatePlaylistAsync(
        string? name = null,
        string? playlistId = null,
        List<string>? songIds = null,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> UpdatePlaylistAsync(
        string playlistId,
        string? name = null,
        string? comment = null,
        bool? isPublic = null,
        List<string>? songIdsToAdd = null,
        List<int>? songIndicesToRemove = null,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> DeletePlaylistAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<PlayQueueResponse> GetPlayQueueAsync(CancellationToken cancellationToken = default);

    Task<SubsonicResponse> SavePlayQueueAsync(
        List<string> ids,
        string? current = null,
        long? position = null,
        CancellationToken cancellationToken = default
    );
}
```

### UserClient

The `UserClient` provides functions for user management.

```csharp
public interface IUserClient
{
    Task<Responses.User.UserResponse> GetUserAsync(
        string username,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> ChangePasswordAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default
    );
}
```

### SearchClient

The `SearchClient` offers methods for searching the music library.

```csharp
public interface ISearchClient
{
    Task<Search3Response> Search3Async(
        string query,
        int? artistCount = null,
        int? artistOffset = null,
        int? albumCount = null,
        int? albumOffset = null,
        int? songCount = null,
        int? songOffset = null,
        string? musicFolderId = null,
        CancellationToken cancellationToken = default
    );
}
```

### MediaClient

The `MediaClient` is used for streaming and downloading media.

```csharp
public interface IMediaClient
{
    Task<Stream> StreamAsync(
        string id,
        int? maxBitRate = null,
        string? format = null,
        int? timeOffset = null,
        StreamPlaybackFormat? playbackFormat = null,
        bool? estimateContentLength = null,
        CancellationToken cancellationToken = default
    );

    Task<Stream> DownloadAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<Stream> GetCoverArtAsync(
        string id,
        int? size = null,
        CancellationToken cancellationToken = default
    );

    Task<LyricsResponse> GetLyricsAsync(
        string? artist = null,
        string? title = null,
        CancellationToken cancellationToken = default
    );

    Task<Stream> GetAvatarAsync(
        string username,
        CancellationToken cancellationToken = default
    );
}
```

### RadioClient

The `RadioClient` is used for internet radio functionalities.

```csharp
public interface IRadioClient
{
    Task<InternetRadioStationsResponse> GetInternetRadioStationsAsync(
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> CreateInternetRadioStationAsync(
        string streamUrl,
        string name,
        string? homepageUrl = null,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> UpdateInternetRadioStationAsync(
        string id,
        string streamUrl,
        string name,
        string? homepageUrl = null,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> DeleteInternetRadioStationAsync(
        string id,
        CancellationToken cancellationToken = default
    );
}
```

### JukeboxClient

The `JukeboxClient` allows control of the jukebox mode of the Subsonic server.

```csharp
public interface IJukeboxClient
{
    Task<JukeboxStatusResponse> JukeboxControlAsync(
        JukeboxAction action,
        int? index = null,
        int? offset = null,
        string? id = null,
        float? gain = null,
        CancellationToken cancellationToken = default
    );
}
```

### PodcastClient

The `PodcastClient` is used for managing podcasts.

```csharp
public interface IPodcastClient
{
    Task<PodcastsResponse> GetPodcastsAsync(
        bool? includeEpisodes = null,
        string? id = null,
        CancellationToken cancellationToken = default
    );

    Task<NewestPodcastsResponse> GetNewestPodcastsAsync(
        int? count = null,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> RefreshPodcastsAsync(
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> CreatePodcastChannelAsync(
        string url,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> DeletePodcastChannelAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> DeletePodcastEpisodeAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> DownloadPodcastEpisodeAsync(
        string id,
        CancellationToken cancellationToken = default
    );
}
```

### BookmarkClient

The `BookmarkClient` is responsible for managing bookmarks.

```csharp
public interface IBookmarkClient
{
    Task<BookmarksResponse> GetBookmarksAsync(
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> CreateBookmarkAsync(
        string id,
        long position,
        string? comment = null,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> DeleteBookmarkAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> GetPlayQueueAsync(
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> SavePlayQueueAsync(
        List<string> ids,
        string? current = null,
        long? position = null,
        CancellationToken cancellationToken = default
    );
}
```

### ChatClient

The `ChatClient` is used for chat functionalities.

```csharp
public interface IChatClient
{
    Task<ChatMessagesResponse> GetChatMessagesAsync(
        long? since = null,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> AddChatMessageAsync(
        string message,
        CancellationToken cancellationToken = default
    );
}
```

### VideoClient

The `VideoClient` provides methods for video functionalities.

```csharp
public interface IVideoClient
{
    Task<VideosResponse> GetVideosAsync(
        CancellationToken cancellationToken = default
    );

    Task<VideoInfoResponse> GetVideoInfoAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    Task<Stream> GetCaptions(
        string id,
        CancellationToken cancellationToken = default
    );
}
```

### UserManagementClient

The `UserManagementClient` allows for managing users at an admin level.

```csharp
public interface IUserManagementClient
{
    Task<Responses.UserManagement.UsersResponse> GetUsersAsync(
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> CreateUserAsync(
        string username,
        string password,
        string email,
        bool? ldapAuthenticated = null,
        bool? adminRole = null,
        bool? settingsRole = null,
        bool? streamRole = null,
        bool? jukeboxRole = null,
        bool? downloadRole = null,
        bool? uploadRole = null,
        bool? playlistRole = null,
        bool? coverArtRole = null,
        bool? commentRole = null,
        bool? podcastRole = null,
        bool? shareRole = null,
        bool? videoConversionRole = null,
        int? maxBitRate = null,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> UpdateUserAsync(
        string username,
        string? password = null,
        string? email = null,
        bool? ldapAuthenticated = null,
        bool? adminRole = null,
        bool? settingsRole = null,
        bool? streamRole = null,
        bool? jukeboxRole = null,
        bool? downloadRole = null,
        bool? uploadRole = null,
        bool? playlistRole = null,
        bool? coverArtRole = null,
        bool? commentRole = null,
        bool? podcastRole = null,
        bool? shareRole = null,
        bool? videoConversionRole = null,
        int? maxBitRate = null,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> DeleteUserAsync(
        string username,
        CancellationToken cancellationToken = default
    );

    Task<SubsonicResponse> ChangePasswordAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default
    );
}
```

## Response Classes

The library includes a variety of response classes that represent the API responses. All response classes inherit from `SubsonicResponse`.

```csharp
public class SubsonicResponse
{
    public string? Status { get; set; }

    public string? Version { get; set; }

    public string? Type { get; set; }

    public string? ServerVersion { get; set; }

    public bool OpenSubsonic { get; set; }

    public SubsonicError? Error { get; set; }

    public bool IsSuccess => Status == "ok";
}
```

Important response classes include:

- Browsing.MusicFoldersResponse
- Browsing.IndexesResponse
- Browsing.MusicDirectoryResponse
- Browsing.GenresResponse
- Browsing.ArtistsResponse
- Browsing.ArtistResponse
- Browsing.AlbumResponse
- Browsing.SongResponse
- Search.Search3Response
- Playlists.PlaylistsResponse
- Playlists.PlaylistResponse
- Radio.InternetRadioStationsResponse
- Podcasts.PodcastsResponse
- System.LicenseResponse
- System.ScanStatusResponse

## Models

The model classes represent the data structures returned by the Subsonic server. Important models include:

- Browsing.Models.Directory
- Browsing.Models.Child
- Browsing.Models.Genre
- Browsing.Models.Artist
- Browsing.Models.AlbumSummary
- Search.Models.Song
- Search.Models.Album
- Search.Models.Artist
- Playlists.Models.PlaylistSummary
- Radio.Models.InternetRadioStation
- Podcasts.Models.PodcastChannel
- Podcasts.Models.PodcastEpisode

## Utilities

### RequestBuilder

The `RequestBuilder` class helps in constructing API request URLs with parameters.

```csharp
internal class RequestBuilder
{
    public RequestBuilder(string endpoint);

    public RequestBuilder AddParameter(string name, string value);

    public RequestBuilder AddParameter<T>(string name, T? value) where T : struct;

    public RequestBuilder AddParameter(string name, bool? value);

    public RequestBuilder AddParameter(string name, DateTime? value);

    public RequestBuilder AddParameters(string name, IEnumerable<string> values);

    public RequestBuilder AddParameters(string name, IEnumerable<int> values);

    public string BuildRequestUrl();
}
```

### JsonParser

The `JsonParser` class parses JSON responses from the Subsonic API.

```csharp
internal static class JsonParser
{
    public static T Parse<T>(string json) where T : SubsonicResponse, new();

    public static T Parse<T>(Stream stream) where T : SubsonicResponse, new();
}
```

## Error Handling

The library uses `SubsonicApiException` for API-specific errors:

```csharp
public class SubsonicApiException : Exception
{
    public int Code { get; }

    public SubsonicApiException(int code, string message);

    public SubsonicApiException(int code, string message, Exception innerException);
}
```

All API responses also include a status field and optionally an error object:

```csharp
public class SubsonicError
{
    public int Code { get; set; }

    public string? Message { get; set; }
}
```

## Examples

### Connecting and Browsing Music

```csharp
// Connecting
var connectionInfo = new SubsonicConnectionInfo("http://yourserver", "username", "password");
var client = new SubsonicClient(connectionInfo);

// Retrieving music folders
var musicFolders = await client.Browsing.GetMusicFoldersAsync();
foreach (var folder in musicFolders.MusicFolders)
{
    Console.WriteLine($"Folder: {folder.Name} (ID: {folder.Id})");
}

// Retrieving artists
var artists = await client.Browsing.GetArtistsAsync();
foreach (var index in artists.Artists.Index)
{
    foreach (var artist in index.Artist)
    {
        Console.WriteLine($"Artist: {artist.Name}");

        // Retrieving albums of the artist
        var artistDetails = await client.Browsing.GetArtistAsync(artist.Id);
        foreach (var album in artistDetails.Artist.Album)
        {
            Console.WriteLine($"  Album: {album.Name} ({album.Year})");

            // Retrieving songs of the album
            var albumDetails = await client.Browsing.GetAlbumAsync(album.Id);
            foreach (var song in albumDetails.Album.Song)
            {
                Console.WriteLine($"    Song: {song.Title} ({song.Duration} seconds)");
            }
        }
    }
}
```

### Managing Playlists

```csharp
// Retrieving playlists
var playlists = await client.Playlists.GetPlaylistsAsync();
foreach (var playlist in playlists.Playlists.Playlist)
{
    Console.WriteLine($"Playlist: {playlist.Name} by {playlist.Owner}");

    // Retrieving playlist details
    var playlistDetails = await client.Playlists.GetPlaylistAsync(playlist.Id);
    foreach (var song in playlistDetails.Playlist.Entry)
    {
        Console.WriteLine($"  Song: {song.Title} by {song.Artist}");
    }
}

// Creating a new playlist
await client.Playlists.CreatePlaylistAsync(
    name: "My New Playlist",
    songIds: new List<string> { "song1Id", "song2Id" }
);

// Updating a playlist
await client.Playlists.UpdatePlaylistAsync(
    playlistId: "playlistId",
    name: "Updated Playlist Name",
    comment: "My updated playlist",
    isPublic: true,
    songIdsToAdd: new List<string> { "song3Id" },
    songIndicesToRemove: new List<int> { 0 } // Removes the first song
);

// Deleting a playlist
await client.Playlists.DeletePlaylistAsync("playlistId");
```

### Streaming Media

```csharp
// Streaming a song
using (var stream = await client.Media.StreamAsync("songId"))
using (var fileStream = new FileStream("song.mp3", FileMode.Create))
{
    await stream.CopyToAsync(fileStream);
}

// Retrieving cover art
using (var stream = await client.Media.GetCoverArtAsync("albumId"))
using (var fileStream = new FileStream("cover.jpg", FileMode.Create))
{
    await stream.CopyToAsync(fileStream);
}
```

### Searching

```csharp
// Searching for "Beatles"
var searchResults = await client.Search.Search3Async(
    query: "Beatles",
    artistCount: 10,
    albumCount: 20,
    songCount: 30
);

// Outputting the results
foreach (var artist in searchResults.SearchResult.Artists)
{
    Console.WriteLine($"Artist: {artist.Name}");
}

foreach (var album in searchResults.SearchResult.Albums)
{
    Console.WriteLine($"Album: {album.Name} by {album.Artist}");
}

foreach (var song in searchResults.SearchResult.Songs)
{
    Console.WriteLine($"Song: {song.Title} by {song.Artist} on {song.Album}");
}
```

### System Operations

```csharp
// Server Ping
var pingResponse = await client.System.PingAsync();
Console.WriteLine($"Server is {(pingResponse.IsSuccess ? "online" : "offline")}");

// Retrieving license information
var license = await client.System.GetLicenseAsync();
Console.WriteLine($"License is {(license.License.Valid ? "valid" : "invalid")}");

// Retrieving scan status
var scanStatus = await client.System.GetScanStatusAsync();
Console.WriteLine($"Scan status: {scanStatus.ScanStatus.Scanning}");

// Starting a scan
await client.System.StartScanAsync();
```
