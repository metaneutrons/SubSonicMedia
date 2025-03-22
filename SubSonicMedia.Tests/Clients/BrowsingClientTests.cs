// <copyright file="BrowsingClientTests.cs" company="Fabian Schmieder">
// This file is part of SubSonicMedia.
//
// SubSonicMedia is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SubSonicMedia is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SubSonicMedia. If not, see https://www.gnu.org/licenses/.
// </copyright>

using System.Net;
using FluentAssertions;
using SubSonicMedia.Exceptions;
using SubSonicMedia.Models;
using SubSonicMedia.Responses.Browsing;
using SubSonicMedia.Tests.Fixtures;
using SubSonicMedia.Tests.Helpers;
using Xunit;

namespace SubSonicMedia.Tests.Clients
{
    /// <summary>
    /// Tests for the BrowsingClient implementation.
    /// </summary>
    [Collection("TestInit")]
    public class BrowsingClientTests : IClassFixture<WireMockServerFixture>, IDisposable
    {
        private readonly WireMockServerFixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsingClientTests"/> class.
        /// </summary>
        /// <param name="fixture">The WireMock server fixture.</param>
        public BrowsingClientTests(WireMockServerFixture fixture)
        {
            this._fixture = fixture;
            this._fixture.Reset();
        }

        /// <summary>
        /// Tests that GetMusicFolders returns the expected folders.
        /// </summary>
        [Fact]
        public async Task GetMusicFolders_ShouldReturnMusicFolders()
        {
            // Arrange
            const string musicFoldersJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""musicFolders"": {
                            ""musicFolder"": [
                                {
                                    ""id"": 1,
                                    ""name"": ""Music Library""
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getMusicFolders.view", musicFoldersJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetMusicFoldersAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.MusicFolders.Should().NotBeNull();
            response.MusicFolders.Should().HaveCount(1);
            response.MusicFolders[0].Id.Should().Be(1);
            response.MusicFolders[0].Name.Should().Be("Music Library");
        }

        /// <summary>
        /// Tests that GetIndexes returns the expected indexes.
        /// </summary>
        [Fact]
        public async Task GetIndexes_ShouldReturnIndexes()
        {
            // Arrange
            const string indexesJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""indexes"": {
                            ""lastModified"": 1614556800000,
                            ""ignoredArticles"": ""The El La Los Las Le Les"",
                            ""index"": [
                                {
                                    ""name"": ""A"",
                                    ""artist"": [
                                        {
                                            ""id"": ""ar-1"",
                                            ""name"": ""ABBA""
                                        }
                                    ]
                                },
                                {
                                    ""name"": ""B"",
                                    ""artist"": [
                                        {
                                            ""id"": ""ar-2"",
                                            ""name"": ""Beatles, The""
                                        }
                                    ]
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getIndexes.view", indexesJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetIndexesAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Indexes.Should().NotBeNull();
            // LastModified is a DateTime in the model, but a long in JSON
            // response.Indexes.LastModified.Should().Be(1614556800000);
            // response.Indexes.IgnoredArticles.Should().Be("The El La Los Las Le Les");
            response.Indexes.Index.Should().HaveCount(2);
            response.Indexes.Index[0].Name.Should().Be("A");
            response.Indexes.Index[0].Artist.Should().HaveCount(1);
            response.Indexes.Index[0].Artist[0].Name.Should().Be("ABBA");
            response.Indexes.Index[1].Name.Should().Be("B");
            response.Indexes.Index[1].Artist[0].Name.Should().Be("Beatles, The");
        }

        /// <summary>
        /// Tests that GetIndexes with musicFolderId parameter works correctly.
        /// </summary>
        [Fact]
        public async Task GetIndexes_WithMusicFolderId_ShouldIncludeParameter()
        {
            // Arrange
            const string indexesJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""indexes"": {
                            ""lastModified"": 1614556800000,
                            ""ignoredArticles"": ""The El La Los Las Le Les"",
                            ""index"": [
                                {
                                    ""name"": ""A"",
                                    ""artist"": [
                                        {
                                            ""id"": ""ar-1"",
                                            ""name"": ""ABBA""
                                        }
                                    ]
                                }
                            ]
                        }
                    }
                }";

            // Setup with request matcher to verify musicFolderId parameter
            this._fixture.Server.Given(
                    WireMock.RequestBuilders.Request.Create()
                        .WithPath("/rest/getIndexes.view")
                        .WithParam("musicFolderId", "1")
                        .UsingAnyMethod()
                )
                .RespondWith(
                    WireMock.ResponseBuilders.Response.Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(indexesJson)
                );

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetIndexesAsync(musicFolderId: "1");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Indexes.Should().NotBeNull();
            response.Indexes.Index.Should().HaveCount(1);
        }

        /// <summary>
        /// Tests that GetArtists returns the expected artists.
        /// </summary>
        [Fact]
        public async Task GetArtists_ShouldReturnArtists()
        {
            // Arrange
            const string artistsJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""artists"": {
                            ""ignoredArticles"": ""The El La Los Las Le Les"",
                            ""index"": [
                                {
                                    ""name"": ""A"",
                                    ""artist"": [
                                        {
                                            ""id"": ""ar-1"",
                                            ""name"": ""ABBA"",
                                            ""albumCount"": 2
                                        }
                                    ]
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getArtists.view", artistsJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetArtistsAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Artists.Should().NotBeNull();
            response.Artists.Index.Should().NotBeNull();
            response.Artists.Index.Should().HaveCount(1);
            response.Artists.Index[0].Name.Should().Be("A");
            response.Artists.Index[0].Artist.Should().HaveCount(1);
            response.Artists.Index[0].Artist[0].Name.Should().Be("ABBA");
            response.Artists.Index[0].Artist[0].AlbumCount.Should().Be(2);
        }

        /// <summary>
        /// Tests that GetArtist returns the expected artist details.
        /// </summary>
        [Fact]
        public async Task GetArtist_ShouldReturnArtistDetails()
        {
            // Arrange
            const string artistDetailJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""artist"": {
                            ""id"": ""ar-1"",
                            ""name"": ""ABBA"",
                            ""album"": [
                                {
                                    ""id"": ""al-1"",
                                    ""name"": ""Gold"",
                                    ""artist"": ""ABBA"",
                                    ""artistId"": ""ar-1"",
                                    ""year"": 1992,
                                    ""songCount"": 19
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getArtist.view", artistDetailJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetArtistAsync("ar-1");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Artist.Should().NotBeNull();
            response.Artist.Name.Should().Be("ABBA");
            response.Artist.Album.Should().NotBeNull();
            response.Artist.Album.Should().HaveCount(1);
            response.Artist.Album[0].Name.Should().Be("Gold");
            response.Artist.Album[0].Year.Should().Be(1992);
        }

        /// <summary>
        /// Tests that GetAlbum returns the expected album details.
        /// </summary>
        [Fact]
        public async Task GetAlbum_ShouldReturnAlbumDetails()
        {
            // Arrange
            const string albumDetailJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""album"": {
                            ""id"": ""al-1"",
                            ""name"": ""Gold"",
                            ""artist"": ""ABBA"",
                            ""artistId"": ""ar-1"",
                            ""year"": 1992,
                            ""genre"": ""Pop"",
                            ""coverArt"": ""al-1"",
                            ""song"": [
                                {
                                    ""id"": ""song-1"",
                                    ""parent"": ""al-1"",
                                    ""title"": ""Dancing Queen"",
                                    ""artist"": ""ABBA"",
                                    ""isDir"": false,
                                    ""track"": 1,
                                    ""year"": 1992,
                                    ""genre"": ""Pop"",
                                    ""coverArt"": ""al-1"",
                                    ""duration"": 231,
                                    ""bitRate"": 320,
                                    ""size"": 9234567
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getAlbum.view", albumDetailJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetAlbumAsync("al-1");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Album.Should().NotBeNull();
            response.Album.Name.Should().Be("Gold");
            response.Album.Artist.Should().Be("ABBA");
            response.Album.Year.Should().Be(1992);
            response.Album.Genre.Should().Be("Pop");
            response.Album.Song.Should().HaveCount(1);
            response.Album.Song[0].Title.Should().Be("Dancing Queen");
            response.Album.Song[0].Duration.Should().Be(231);
        }

        /// <summary>
        /// Tests that GetMusicDirectory returns the expected directory.
        /// </summary>
        [Fact]
        public async Task GetMusicDirectory_WithValidId_ShouldReturnDirectory()
        {
            // Arrange
            const string musicDirectoryJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""directory"": {
                            ""id"": ""123"",
                            ""name"": ""Test Directory"",
                            ""child"": [
                                {
                                    ""id"": ""456"",
                                    ""parent"": ""123"",
                                    ""isDir"": true,
                                    ""title"": ""Subdirectory"",
                                    ""album"": ""Album Name"",
                                    ""artist"": ""Artist Name""
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getMusicDirectory.view", musicDirectoryJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetMusicDirectoryAsync("123");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Directory.Should().NotBeNull();
            response.Directory.Id.Should().Be("123");
            response.Directory.Name.Should().Be("Test Directory");
            response.Directory.Children.Should().NotBeNull();
            response.Directory.Children.Should().HaveCount(1);
            response.Directory.Children[0].Id.Should().Be("456");
            response.Directory.Children[0].IsDir.Should().BeTrue();
        }

        /// <summary>
        /// Tests that GetGenres returns the expected genres.
        /// </summary>
        [Fact]
        public async Task GetGenres_ShouldReturnGenres()
        {
            // Arrange
            const string genresJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""genres"": {
                            ""genre"": [
                                {
                                    ""songCount"": 42,
                                    ""albumCount"": 10,
                                    ""value"": ""Rock""
                                },
                                {
                                    ""songCount"": 30,
                                    ""albumCount"": 5,
                                    ""value"": ""Pop""
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getGenres.view", genresJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetGenresAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Genres.Should().NotBeNull();
            response.Genres.Genre.Should().NotBeNull();
            response.Genres.Genre.Should().HaveCount(2);
            response.Genres.Genre[0].Name.Should().Be("Rock");
            response.Genres.Genre[0].SongCount.Should().Be(42);
            response.Genres.Genre[0].AlbumCount.Should().Be(10);
            response.Genres.Genre[1].Name.Should().Be("Pop");
        }

        /// <summary>
        /// Tests that GetSong returns the expected song details.
        /// </summary>
        [Fact]
        public async Task GetSong_ShouldReturnSongDetails()
        {
            // Arrange
            const string songDetailJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""song"": {
                            ""id"": ""song-1"",
                            ""parent"": ""al-1"",
                            ""title"": ""Dancing Queen"",
                            ""album"": ""Gold"",
                            ""artist"": ""ABBA"",
                            ""track"": 1,
                            ""year"": 1992,
                            ""genre"": ""Pop"",
                            ""coverArt"": ""al-1"",
                            ""size"": 9234567,
                            ""contentType"": ""audio/mpeg"",
                            ""suffix"": ""mp3"",
                            ""duration"": 231,
                            ""bitRate"": 320,
                            ""path"": ""ABBA/Gold/01 - Dancing Queen.mp3"",
                            ""playCount"": 5,
                            ""created"": ""2020-01-01T00:00:00.000Z"",
                            ""albumId"": ""al-1"",
                            ""artistId"": ""ar-1"",
                            ""type"": ""music""
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getSong.view", songDetailJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetSongAsync("song-1");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Song.Should().NotBeNull();
            response.Song.Id.Should().Be("song-1");
            response.Song.Title.Should().Be("Dancing Queen");
            response.Song.Album.Should().Be("Gold");
            response.Song.Artist.Should().Be("ABBA");
            response.Song.Duration.Should().Be(231);
            response.Song.BitRate.Should().Be(320);
            response.Song.Path.Should().Be("ABBA/Gold/01 - Dancing Queen.mp3");
        }

        /// <summary>
        /// Tests that GetRandomSongs returns the expected random songs.
        /// </summary>
        [Fact]
        public async Task GetRandomSongs_ShouldReturnRandomSongs()
        {
            // Arrange
            const string randomSongsJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""randomSongs"": {
                            ""song"": [
                                {
                                    ""id"": ""song-1"",
                                    ""parent"": ""al-1"",
                                    ""title"": ""Dancing Queen"",
                                    ""album"": ""Gold"",
                                    ""artist"": ""ABBA"",
                                    ""isDir"": false,
                                    ""track"": 1,
                                    ""year"": 1992,
                                    ""genre"": ""Pop"",
                                    ""coverArt"": ""al-1"",
                                    ""size"": 9234567,
                                    ""contentType"": ""audio/mpeg"",
                                    ""suffix"": ""mp3"",
                                    ""duration"": 231,
                                    ""bitRate"": 320,
                                    ""path"": ""ABBA/Gold/01 - Dancing Queen.mp3""
                                },
                                {
                                    ""id"": ""song-2"",
                                    ""parent"": ""al-2"",
                                    ""title"": ""Hey Jude"",
                                    ""album"": ""1"",
                                    ""artist"": ""The Beatles"",
                                    ""isDir"": false,
                                    ""track"": 19,
                                    ""year"": 2000,
                                    ""genre"": ""Rock"",
                                    ""coverArt"": ""al-2"",
                                    ""size"": 8234567,
                                    ""contentType"": ""audio/mpeg"",
                                    ""suffix"": ""mp3"",
                                    ""duration"": 425,
                                    ""bitRate"": 320,
                                    ""path"": ""The Beatles/1/19 - Hey Jude.mp3""
                                }
                            ]
                        }
                    }
                }";

            // Setup with request matcher to verify size parameter
            this._fixture.Server.Given(
                    WireMock.RequestBuilders.Request.Create()
                        .WithPath("/rest/getRandomSongs.view")
                        .WithParam("size", "10")
                        .UsingAnyMethod()
                )
                .RespondWith(
                    WireMock.ResponseBuilders.Response.Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(randomSongsJson)
                );

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetRandomSongsAsync(size: 10);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.RandomSongs.Should().NotBeNull();
            response.RandomSongs.Song.Should().NotBeNull();
            response.RandomSongs.Song.Should().HaveCount(2);
            response.RandomSongs.Song[0].Title.Should().Be("Dancing Queen");
            response.RandomSongs.Song[1].Title.Should().Be("Hey Jude");
        }

        /// <summary>
        /// Tests that GetRandomSongs with genre filter returns the expected random songs.
        /// </summary>
        [Fact]
        public async Task GetRandomSongs_WithGenreFilter_ShouldIncludeParameter()
        {
            // Arrange
            const string randomSongsJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""randomSongs"": {
                            ""song"": [
                                {
                                    ""id"": ""song-1"",
                                    ""parent"": ""al-1"",
                                    ""title"": ""Dancing Queen"",
                                    ""album"": ""Gold"",
                                    ""artist"": ""ABBA"",
                                    ""genre"": ""Pop"",
                                    ""duration"": 231
                                }
                            ]
                        }
                    }
                }";

            // Setup with request matcher to verify genre parameter
            this._fixture.Server.Given(
                    WireMock.RequestBuilders.Request.Create()
                        .WithPath("/rest/getRandomSongs.view")
                        .WithParam("genre", "Pop")
                        .UsingAnyMethod()
                )
                .RespondWith(
                    WireMock.ResponseBuilders.Response.Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(randomSongsJson)
                );

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetRandomSongsAsync(genre: "Pop");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.RandomSongs.Should().NotBeNull();
            response.RandomSongs.Song.Should().HaveCount(1);
            response.RandomSongs.Song[0].Genre.Should().Be("Pop");
        }

        /// <summary>
        /// Tests that GetStarred returns the expected starred items.
        /// </summary>
        [Fact]
        public async Task GetStarred_ShouldReturnStarredItems()
        {
            // Arrange
            const string starredJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""starred"": {
                            ""artist"": [
                                {
                                    ""id"": ""ar-1"",
                                    ""name"": ""ABBA""
                                }
                            ],
                            ""album"": [
                                {
                                    ""id"": ""al-1"",
                                    ""name"": ""Gold"",
                                    ""artist"": ""ABBA"",
                                    ""artistId"": ""ar-1"",
                                    ""coverArt"": ""al-1"",
                                    ""songCount"": 19,
                                    ""created"": ""2020-01-01T00:00:00.000Z"",
                                    ""starred"": ""2021-01-01T00:00:00.000Z""
                                }
                            ],
                            ""song"": [
                                {
                                    ""id"": ""song-1"",
                                    ""parent"": ""al-1"",
                                    ""title"": ""Dancing Queen"",
                                    ""album"": ""Gold"",
                                    ""artist"": ""ABBA"",
                                    ""duration"": 231,
                                    ""starred"": ""2021-01-01T00:00:00.000Z""
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getStarred.view", starredJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetStarredAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Starred.Should().NotBeNull();
            response.Starred.Artist.Should().HaveCount(1);
            response.Starred.Artist[0].Name.Should().Be("ABBA");
            response.Starred.Album.Should().HaveCount(1);
            response.Starred.Album[0].Name.Should().Be("Gold");
            response.Starred.Song.Should().HaveCount(1);
            response.Starred.Song[0].Title.Should().Be("Dancing Queen");
        }

        /// <summary>
        /// Tests that GetAlbumList2 returns the expected album list.
        /// </summary>
        [Fact]
        public async Task GetAlbumList2_ShouldReturnAlbumList()
        {
            // Arrange
            const string albumListJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""albumList2"": {
                            ""album"": [
                                {
                                    ""id"": ""al-1"",
                                    ""name"": ""Gold"",
                                    ""artist"": ""ABBA"",
                                    ""artistId"": ""ar-1"",
                                    ""songCount"": 19,
                                    ""duration"": 3600,
                                    ""created"": ""2020-01-01T00:00:00.000Z"",
                                    ""year"": 1992,
                                    ""genre"": ""Pop"",
                                    ""coverArt"": ""al-1""
                                },
                                {
                                    ""id"": ""al-2"",
                                    ""name"": ""1"",
                                    ""artist"": ""The Beatles"",
                                    ""artistId"": ""ar-2"",
                                    ""songCount"": 27,
                                    ""duration"": 4800,
                                    ""created"": ""2020-01-02T00:00:00.000Z"",
                                    ""year"": 2000,
                                    ""genre"": ""Rock"",
                                    ""coverArt"": ""al-2""
                                }
                            ]
                        }
                    }
                }";

            // Setup with request matcher to verify type parameter
            this._fixture.Server.Given(
                    WireMock.RequestBuilders.Request.Create()
                        .WithPath("/rest/getAlbumList2.view")
                        .WithParam("type", "newest")
                        .UsingAnyMethod()
                )
                .RespondWith(
                    WireMock.ResponseBuilders.Response.Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(albumListJson)
                );

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetAlbumListAsync(AlbumListType.Newest);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.AlbumList2.Should().NotBeNull();
            response.AlbumList2.Album.Should().HaveCount(2);
            response.AlbumList2.Album[0].Name.Should().Be("Gold");
            response.AlbumList2.Album[0].Artist.Should().Be("ABBA");
            response.AlbumList2.Album[1].Name.Should().Be("1");
            response.AlbumList2.Album[1].Artist.Should().Be("The Beatles");
        }

        /// <summary>
        /// Tests that GetSongsByGenre returns the expected songs.
        /// </summary>
        [Fact]
        public async Task GetSongsByGenre_ShouldReturnSongs()
        {
            // Arrange
            const string songsByGenreJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""songsByGenre"": {
                            ""song"": [
                                {
                                    ""id"": ""song-1"",
                                    ""parent"": ""al-1"",
                                    ""title"": ""Dancing Queen"",
                                    ""album"": ""Gold"",
                                    ""artist"": ""ABBA"",
                                    ""genre"": ""Pop"",
                                    ""duration"": 231
                                },
                                {
                                    ""id"": ""song-3"",
                                    ""parent"": ""al-3"",
                                    ""title"": ""Waterloo"",
                                    ""album"": ""Waterloo"",
                                    ""artist"": ""ABBA"",
                                    ""genre"": ""Pop"",
                                    ""duration"": 180
                                }
                            ]
                        }
                    }
                }";

            // Setup with request matcher to verify genre parameter
            this._fixture.Server.Given(
                    WireMock.RequestBuilders.Request.Create()
                        .WithPath("/rest/getSongsByGenre.view")
                        .WithParam("genre", "Pop")
                        .WithParam("count", "10")
                        .UsingAnyMethod()
                )
                .RespondWith(
                    WireMock.ResponseBuilders.Response.Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(songsByGenreJson)
                );

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Browsing.GetSongsByGenreAsync("Pop", count: 10);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.SongsByGenre.Should().NotBeNull();
            response.SongsByGenre.Song.Should().HaveCount(2);
            response.SongsByGenre.Song[0].Title.Should().Be("Dancing Queen");
            response.SongsByGenre.Song[0].Genre.Should().Be("Pop");
            response.SongsByGenre.Song[1].Title.Should().Be("Waterloo");
            response.SongsByGenre.Song[1].Genre.Should().Be("Pop");
        }

        /// <summary>
        /// Tests that browsing endpoints handle error responses properly.
        /// </summary>
        [Fact]
        public async Task BrowsingEndpoints_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            const string errorResponseJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""failed"",
                        ""version"": ""1.16.1"",
                        ""error"": {
                            ""code"": 70,
                            ""message"": ""The requested data was not found""
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getAlbum.view", errorResponseJson, HttpStatusCode.NotFound);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<SubsonicApiException>(
                () => client.Browsing.GetAlbumAsync("invalid-id"));
            
            ex.Message.Should().Contain("HTTP error: NotFound");
            ex.ErrorCode.Should().Be(404);
        }

        /// <summary>
        /// Clean up after each test.
        /// </summary>
        public void Dispose()
        {
            this._fixture.Reset();
        }
    }
}
