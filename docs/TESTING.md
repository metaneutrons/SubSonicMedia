# SubSonicMedia Testing Strategy

## Overview

This document outlines the comprehensive testing strategy for the SubSonicMedia library. Our goal is to transition from manual testing via TestKit (which requires a live Subsonic server) to automated unit tests with mocked server responses. This approach offers several advantages:

1. Tests can run without a live Subsonic server
2. Tests run faster and are more deterministic
3. Edge cases can be reliably tested
4. CI/CD integration becomes trivial
5. Test coverage can be measured and improved systematically

## Testing Framework and Tools

We will use the following tools and libraries for our testing strategy:

1. **xUnit**: Primary testing framework
2. **WireMock.NET**: For HTTP server mocking
3. **Coverlet**: For measuring code coverage
4. **FluentAssertions**: For more readable assertions
5. **TestKit Output**: Utilizing real-world API response data captured by our TestKit tool

## Project Structure

We'll create a dedicated test project with the following structure:

```
SubSonicMedia.Tests/
├── Clients/                    # Tests for client implementations
│   ├── BrowsingClientTests.cs
│   ├── SearchClientTests.cs
│   ├── MediaClientTests.cs
│   ├── PlaylistClientTests.cs
│   ├── BookmarkClientTests.cs
│   ├── RadioClientTests.cs
│   ├── SystemClientTests.cs
│   └── UserClientTests.cs
├── Core/                       # Tests for core functionality
│   ├── SubsonicClientTests.cs
│   ├── AuthenticationTests.cs
│   └── RequestBuilderTests.cs
├── Helpers/                    # Testing helpers and utilities
│   ├── WireMockServerFactory.cs
│   ├── TestKitResponseLoader.cs
│   └── TestDataFactory.cs
├── Fixtures/                   # Shared test fixtures
│   └── WireMockServerFixture.cs
├── TestResources/              # Copied test resources from TestKit output
│   ├── Browsing/
│   ├── Search/
│   ├── Media/
│   ├── Playlists/
│   ├── Bookmarks/
│   ├── Radio/
│   ├── System/
│   └── User/
└── Utilities/                  # Test utilities
    ├── JsonFileManager.cs
    └── AssertionExtensions.cs
```

## TestKit Output Files

TestKit creates a variety of output files during its manual testing process. These files provide real-world examples of Subsonic API interactions that we can leverage in our automated tests. The files are organized by API feature area and type of data.

### Key TestKit Output Files

| File Path | Description | Example Content |
|-----------|-------------|----------------|
| `/TestKit/Outputs/Browsing/getMusicFolders.json` | JSON response from the getMusicFolders API | List of music folders on the server |
| `/TestKit/Outputs/Browsing/getArtists.json` | JSON response from the getArtists API | List of artists with indexing information |
| `/TestKit/Outputs/Browsing/getAlbumList2.json` | JSON response from the getAlbumList2 API | Collection of albums by criteria |
| `/TestKit/Outputs/Search/search3.json` | JSON response from the search3 API | Search results across artists, albums, and songs |
| `/TestKit/Outputs/Media/stream_metadata.json` | Metadata about a streamed media file | Format, bitrate, and other stream details |
| `/TestKit/Outputs/Media/coverArt_metadata.json` | Metadata about cover art files | Image dimensions, format information |
| `/TestKit/Outputs/Playlists/getPlaylists.json` | JSON response from the getPlaylists API | List of available playlists |
| `/TestKit/Outputs/Playlists/getPlaylist.json` | JSON response from the getPlaylist API | Detailed playlist with songs |
| `/TestKit/Outputs/Bookmarks/getBookmarks.json` | JSON response from the getBookmarks API | List of saved media bookmarks |
| `/TestKit/Outputs/Radio/getInternetRadioStations.json` | JSON response from the getInternetRadioStations API | Available internet radio stations |
| `/TestKit/Outputs/System/ping.json` | JSON response from the ping API | Server status response |
| `/TestKit/Outputs/System/getLicense.json` | JSON response from the getLicense API | License information for the server |
| `/TestKit/Outputs/User/getUser.json` | JSON response from the getUser API | User account details and permissions |
| `/TestKit/Outputs/Binary/sample_coverart.jpg` | Sample cover art binary file | JPEG image data |
| `/TestKit/Outputs/Binary/sample_stream.mp3` | Sample media stream binary file | MP3 audio data |

### File Structure

TestKit output files follow a consistent pattern:

1. **JSON Response Files**: Contain the complete JSON response from the Subsonic API, including all metadata and response data.
2. **Binary Files**: Contain binary data for responses like media streams, cover art images, etc.
3. **Metadata Files**: Contain additional information about binary responses when relevant.

## Mocking Strategy with WireMock.NET

Our primary mocking strategy will use WireMock.NET to create a mock HTTP server that responds with real-world data captured by TestKit. This approach simulates a Subsonic server without requiring an actual server connection.

### WireMock.NET Setup

We'll create a helper class to manage WireMock.NET server instances:

```csharp
public class WireMockServerFactory
{
    public static WireMockServer CreateServer()
    {
        return WireMockServer.Start();
    }

    public static void ConfigureForStandardResponses(WireMockServer server)
    {
        // Set up common endpoints with responses from TestKit output files

        // System endpoints
        server
            .Given(Request.Create().WithPath("/rest/ping.view").WithParam("v", "1.15.0"))
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(File.ReadAllText("TestResources/System/ping.json"))
            );

        // Browsing endpoints
        server
            .Given(Request.Create().WithPath("/rest/getMusicFolders.view"))
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(File.ReadAllText("TestResources/Browsing/getMusicFolders.json"))
            );

        // ...configure other standard endpoints...
    }

    public static void ConfigureForBinaryResponse(
        WireMockServer server,
        string path,
        string binaryFilePath,
        string contentType)
    {
        server
            .Given(Request.Create().WithPath(path))
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", contentType)
                    .WithBody(File.ReadAllBytes(binaryFilePath))
            );
    }
}
```

### TestKit Response Loader

We'll also create a helper to load TestKit response files:

```csharp
public static class TestKitResponseLoader
{
    private static readonly string TestResourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestResources");

    public static string LoadJsonResponse(string category, string endpoint)
    {
        string path = Path.Combine(TestResourcesPath, category, $"{endpoint}.json");
        return File.ReadAllText(path);
    }

    public static byte[] LoadBinaryResponse(string category, string filename)
    {
        string path = Path.Combine(TestResourcesPath, category, filename);
        return File.ReadAllBytes(path);
    }

    // Copies TestKit output files to the test resources directory
    public static void SetupTestResources(string testKitOutputPath = null)
    {
        // Default path if not specified
        string sourcePath = testKitOutputPath ?? Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "TestKit", "Outputs");

        // Ensure target directory exists
        Directory.CreateDirectory(TestResourcesPath);

        // Copy all TestKit output files to test resources
        CopyDirectory(sourcePath, TestResourcesPath, true);
    }

    private static void CopyDirectory(string sourceDir, string targetDir, bool overwrite)
    {
        // Implementation of directory copying...
    }
}
```

### WireMock Server Fixture

For shared test server instances, we'll use a test fixture:

```csharp
public class WireMockServerFixture : IDisposable
{
    public WireMockServer Server { get; }
    public HttpClient Client { get; }
    public SubsonicConnectionInfo ConnectionInfo { get; }

    public WireMockServerFixture()
    {
        // Set up test resources from TestKit output
        TestKitResponseLoader.SetupTestResources();

        // Start the mock server
        Server = WireMockServerFactory.CreateServer();

        // Configure standard responses
        WireMockServerFactory.ConfigureForStandardResponses(Server);

        // Create a client that connects to the mock server
        Client = new HttpClient { BaseAddress = new Uri(Server.Urls[0]) };

        // Create connection info for the SubsonicClient
        ConnectionInfo = new SubsonicConnectionInfo(
            Server.Urls[0],
            "testuser",
            "testpassword"
        );
    }

    public void Dispose()
    {
        Client?.Dispose();
        Server?.Dispose();
    }
}
```

## Test Implementation Plan

### 1. Connection Test

Tests will verify that the SubsonicClient correctly establishes connections and handles authentication.

```csharp
public class ConnectionTests : IClassFixture<WireMockServerFixture>
{
    private readonly WireMockServerFixture _fixture;

    public ConnectionTests(WireMockServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Constructor_WithValidConnectionInfo_CreatesClient()
    {
        // Arrange & Act
        var client = new SubsonicClient(_fixture.ConnectionInfo, _fixture.Client);

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public async Task ExecuteRequest_WithTokenAuthentication_AddsCorrectAuthParameters()
    {
        // Arrange
        // Configure server to verify auth parameters
        _fixture.Server
            .Given(Request.Create().WithPath("/rest/ping.view")
                .WithParam("u", "testuser")
                .WithParam("t", WireMock.Matchers.AnythingPattern.Instance)  // Token can vary
                .WithParam("s", WireMock.Matchers.AnythingPattern.Instance)  // Salt can vary
                .WithParam("v", "1.15.0")
                .WithParam("c", "SubSonicMedia")
                .WithParam("f", "json"))
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(TestKitResponseLoader.LoadJsonResponse("System", "ping"))
            );

        var client = new SubsonicClient(_fixture.ConnectionInfo, _fixture.Client);

        // Act
        var response = await client.System.PingAsync();

        // Assert
        response.Should().NotBeNull();
        response.Status.Should().Be("ok");

        // Verify the request was made with the expected parameters
        _fixture.Server.LogEntries.Should().Contain(entry =>
            entry.RequestMessage.Path.Contains("/rest/ping.view") &&
            entry.RequestMessage.Query.Contains("u=testuser"));
    }
}
```

### 2. Browsing Test

Tests will verify that the BrowsingClient correctly retrieves and parses browsing-related data.

```csharp
public class BrowsingClientTests : IClassFixture<WireMockServerFixture>
{
    private readonly WireMockServerFixture _fixture;
    private readonly SubsonicClient _client;

    public BrowsingClientTests(WireMockServerFixture fixture)
    {
        _fixture = fixture;
        _client = new SubsonicClient(fixture.ConnectionInfo, fixture.Client);

        // Configure specific browsing endpoints
        _fixture.Server
            .Given(Request.Create().WithPath("/rest/getMusicFolders.view"))
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(TestKitResponseLoader.LoadJsonResponse("Browsing", "getMusicFolders"))
            );

        _fixture.Server
            .Given(Request.Create().WithPath("/rest/getArtists.view"))
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(TestKitResponseLoader.LoadJsonResponse("Browsing", "getArtists"))
            );
    }

    [Fact]
    public async Task GetMusicFolders_ReturnsExpectedFolders()
    {
        // Act
        var response = await _client.Browsing.GetMusicFoldersAsync();

        // Assert
        response.Should().NotBeNull();
        response.MusicFolders.Should().NotBeEmpty();
        response.MusicFolders.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetArtists_ReturnsExpectedArtists()
    {
        // Act
        var response = await _client.Browsing.GetArtistsAsync();

        // Assert
        response.Should().NotBeNull();
        response.Artists.Should().NotBeNull();
        response.Artists.Index.Should().NotBeEmpty();
    }
}
```

### 3. Media Streaming Test

Tests will verify the MediaClient correctly handles streaming media content.

```csharp
public class MediaClientTests : IClassFixture<WireMockServerFixture>
{
    private readonly WireMockServerFixture _fixture;
    private readonly SubsonicClient _client;

    public MediaClientTests(WireMockServerFixture fixture)
    {
        _fixture = fixture;
        _client = new SubsonicClient(fixture.ConnectionInfo, fixture.Client);

        // Configure media endpoint to return binary data
        WireMockServerFactory.ConfigureForBinaryResponse(
            _fixture.Server,
            "/rest/stream.view",
            Path.Combine("TestResources", "Binary", "sample_stream.mp3"),
            "audio/mpeg"
        );

        WireMockServerFactory.ConfigureForBinaryResponse(
            _fixture.Server,
            "/rest/getCoverArt.view",
            Path.Combine("TestResources", "Binary", "sample_coverart.jpg"),
            "image/jpeg"
        );
    }

    [Fact]
    public async Task StreamAsync_WithValidId_ReturnsStream()
    {
        // Arrange
        byte[] expectedData = TestKitResponseLoader.LoadBinaryResponse("Binary", "sample_stream.mp3");

        // Act
        var stream = await _client.Media.StreamAsync("123");

        // Assert
        stream.Should().NotBeNull();

        // Read the stream to verify it contains our expected data
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.ToArray().Should().BeEquivalentTo(expectedData);
    }

    [Fact]
    public async Task GetCoverArtAsync_WithValidId_ReturnsImageStream()
    {
        // Arrange
        byte[] expectedData = TestKitResponseLoader.LoadBinaryResponse("Binary", "sample_coverart.jpg");

        // Act
        var stream = await _client.Media.GetCoverArtAsync("album123");

        // Assert
        stream.Should().NotBeNull();

        // Read the stream to verify it contains our expected data
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.ToArray().Should().BeEquivalentTo(expectedData);
    }
}
```

### 4. Dynamic Mapping of API Requests

For more complex testing scenarios, we can create a dynamic mapping system that automatically configures WireMock.NET based on the available TestKit output files:

```csharp
public static class ApiResponseMapper
{
    public static void MapAllTestKitResponsesToServer(WireMockServer server, string testResourcesPath)
    {
        // Map all JSON files to appropriate endpoints
        foreach (var jsonFile in Directory.GetFiles(testResourcesPath, "*.json", SearchOption.AllDirectories))
        {
            string filename = Path.GetFileNameWithoutExtension(jsonFile);
            string endpoint = DeriveEndpointFromFilename(filename);

            if (!string.IsNullOrEmpty(endpoint))
            {
                server
                    .Given(Request.Create().WithPath($"/rest/{endpoint}.view"))
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithHeader("Content-Type", "application/json")
                            .WithBody(File.ReadAllText(jsonFile))
                    );

                Console.WriteLine($"Mapped {jsonFile} to /rest/{endpoint}.view");
            }
        }

        // Map binary files as needed
        // ...
    }

    private static string DeriveEndpointFromFilename(string filename)
    {
        // Simple mapping: assume filename matches endpoint
        // This could be enhanced with a more sophisticated mapping system if needed
        return filename;
    }
}
```

## CI/CD Integration

### Local Build Integration

To run tests during local builds, we'll update the project file to include test execution and coverage reporting:

```xml
<Project>
  <!-- ... existing content ... -->

  <Target Name="RunTests" AfterTargets="Build" Condition="'$(BuildingForLiveUnitTesting)' != 'true'">
    <Exec Command="dotnet test $(SolutionDir)SubSonicMedia.Tests/SubSonicMedia.Tests.csproj --no-build" />
  </Target>

</Project>
```

### GitHub Actions Integration

Create a GitHub Actions workflow in `.github/workflows/build-and-test.yml`:

```yaml
name: Build and Test

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v2

    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 8.0.x

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --no-restore

    - name: Test
      run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"

    - name: Upload coverage to Codecov
      uses: codecov/codecov-action@v1
```

## Best Practices

1. **Test Independence**: Each test should be independent and not rely on the state from other tests.

2. **Descriptive Test Names**: Use a consistent naming convention like `[MethodName]_[Scenario]_[ExpectedResult]` that clearly describes what's being tested.

3. **Arrange-Act-Assert Pattern**: Structure tests with clear separation between the setup (Arrange), the method being tested (Act), and the verification (Assert).

4. **Real-world Data**: Use the real responses captured by TestKit for maximum fidelity in tests.

5. **Test Exception Cases**: Include tests for expected exception cases, not just happy paths.

6. **API Version Consistency**: Ensure that test data and expected API versions are consistent with what the library supports.

7. **Separate Binary Testing**: When testing binary responses (streams, images), use separate test methods focused specifically on binary handling.

## Using TestKit to Generate New Test Data

If you need test data for a new API endpoint or scenario not already covered by existing TestKit outputs, follow these steps:

1. Run TestKit against a live Subsonic server:

   ```bash
   dotnet run --project TestKit/TestKit.csproj -- --server=http://yourserver:port --user=yourusername --password=yourpassword --generate-outputs
   ```

2. Copy the generated output files to the test resources directory:

   ```bash
   cp -R TestKit/Outputs/* SubSonicMedia.Tests/TestResources/
   ```

3. Update your tests to use the new data files.

## Implementation Plan

1. **Phase 1**
   - Setup test project and WireMock.NET infrastructure
   - Create test resource loading utilities
   - Implement Connection and System tests

2. **Phase 2**
   - Implement Browsing, Search, and Media tests
   - Set up CI/CD integration

3. **Phase 3**
   - Implement Playlist, Bookmark, and Radio tests
   - Add code coverage reporting

4. **Phase 4**
   - Implement User
   - Documentation and cleanup

## Conclusion

This comprehensive testing strategy leverages real-world data captured by TestKit with the flexibility of WireMock.NET to create robust, repeatable tests for the SubSonicMedia library. By using captured API responses, we ensure our tests accurately reflect the behavior of a real Subsonic server without requiring an active connection.

The approach allows us to systematically test the entire API surface, catch edge cases, and validate our implementation against real-world data. This strategy will help maintain the reliability of the library as development continues.
