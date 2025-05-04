// <copyright file="SearchClientTests.cs" company="Fabian Schmieder">
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

using System.Threading.Tasks;

using FluentAssertions;

using SubSonicMedia.Models;
using SubSonicMedia.Tests.Fixtures;
using SubSonicMedia.Tests.Helpers;

using Xunit;

namespace SubSonicMedia.Tests.Clients
{
    /// <summary>
    /// Tests for the SearchClient implementation.
    /// </summary>
    [Collection("TestInit")]
    public class SearchClientTests : IClassFixture<WireMockServerFixture>, IDisposable
    {
        private readonly WireMockServerFixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchClientTests"/> class.
        /// </summary>
        /// <param name="fixture">The WireMock server fixture.</param>
        public SearchClientTests(WireMockServerFixture fixture)
        {
            this._fixture = fixture;
            this._fixture.Reset();
        }

        /// <summary>
        /// Tests that Search returns the expected search results.
        /// </summary>
        [Fact]
        public async Task Search_ShouldReturnSearchResults()
        {
            // Arrange
            const string searchJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""searchResult"": {
                            ""artist"": [
                                {
                                    ""id"": ""ar-1"",
                                    ""name"": ""Test Artist"",
                                    ""albumCount"": 2
                                }
                            ],
                            ""album"": [
                                {
                                    ""id"": ""al-1"",
                                    ""name"": ""Test Album"",
                                    ""artist"": ""Test Artist"",
                                    ""artistId"": ""ar-1"",
                                    ""year"": 2023,
                                    ""songCount"": 10
                                }
                            ],
                            ""song"": [
                                {
                                    ""id"": ""song-1"",
                                    ""parent"": ""al-1"",
                                    ""title"": ""Test Song"",
                                    ""album"": ""Test Album"",
                                    ""artist"": ""Test Artist"",
                                    ""year"": 2023,
                                    ""track"": 1,
                                    ""duration"": 180,
                                    ""size"": 3600000,
                                    ""contentType"": ""audio/mp3"",
                                    ""suffix"": ""mp3""
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/search.view", searchJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Search.SearchAsync("test", 10, 10, 10);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.SearchResult.Should().NotBeNull();

            // Check artists
            response.SearchResult.Artists.Should().NotBeNull();
            response.SearchResult.Artists.Should().HaveCount(1);
            response.SearchResult.Artists[0].Id.Should().Be("ar-1");
            response.SearchResult.Artists[0].Name.Should().Be("Test Artist");
            response.SearchResult.Artists[0].AlbumCount.Should().Be(2);

            // Check albums
            response.SearchResult.Albums.Should().NotBeNull();
            response.SearchResult.Albums.Should().HaveCount(1);
            response.SearchResult.Albums[0].Id.Should().Be("al-1");
            response.SearchResult.Albums[0].Name.Should().Be("Test Album");
            response.SearchResult.Albums[0].Artist.Should().Be("Test Artist");
            response.SearchResult.Albums[0].Year.Should().Be(2023);

            // Check songs
            response.SearchResult.Songs.Should().NotBeNull();
            response.SearchResult.Songs.Should().HaveCount(1);
            response.SearchResult.Songs[0].Id.Should().Be("song-1");
            response.SearchResult.Songs[0].Title.Should().Be("Test Song");
            response.SearchResult.Songs[0].Album.Should().Be("Test Album");
            response.SearchResult.Songs[0].Artist.Should().Be("Test Artist");
        }

        /// <summary>
        /// Tests that Search2 returns the expected search results.
        /// </summary>
        [Fact]
        public async Task Search2_ShouldReturnSearchResults()
        {
            // Arrange
            const string search2Json =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""searchResult2"": {
                            ""artist"": [
                                {
                                    ""id"": ""ar-1"",
                                    ""name"": ""Test Artist"",
                                    ""albumCount"": 2
                                }
                            ],
                            ""album"": [
                                {
                                    ""id"": ""al-1"",
                                    ""name"": ""Test Album"",
                                    ""artist"": ""Test Artist"",
                                    ""artistId"": ""ar-1"",
                                    ""year"": 2023,
                                    ""songCount"": 10
                                }
                            ],
                            ""song"": [
                                {
                                    ""id"": ""song-1"",
                                    ""parent"": ""al-1"",
                                    ""title"": ""Test Song"",
                                    ""album"": ""Test Album"",
                                    ""artist"": ""Test Artist"",
                                    ""year"": 2023,
                                    ""track"": 1,
                                    ""duration"": 180,
                                    ""size"": 3600000,
                                    ""contentType"": ""audio/mp3"",
                                    ""suffix"": ""mp3""
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/search2.view", search2Json);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Search.Search2Async("test", 10, 0, 10, 0, 10, 0);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.SearchResult.Should().NotBeNull();

            // Check artists
            response.SearchResult.Artists.Should().NotBeNull();
            response.SearchResult.Artists.Should().HaveCount(1);
            response.SearchResult.Artists[0].Id.Should().Be("ar-1");
            response.SearchResult.Artists[0].Name.Should().Be("Test Artist");

            // Check albums
            response.SearchResult.Albums.Should().NotBeNull();
            response.SearchResult.Albums.Should().HaveCount(1);
            response.SearchResult.Albums[0].Id.Should().Be("al-1");
            response.SearchResult.Albums[0].Name.Should().Be("Test Album");

            // Check songs
            response.SearchResult.Songs.Should().NotBeNull();
            response.SearchResult.Songs.Should().HaveCount(1);
            response.SearchResult.Songs[0].Id.Should().Be("song-1");
            response.SearchResult.Songs[0].Title.Should().Be("Test Song");
        }

        /// <summary>
        /// Tests that Search3 returns the expected search results.
        /// </summary>
        [Fact]
        public async Task Search3_ShouldReturnSearchResults()
        {
            // Arrange
            const string search3Json =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""searchResult3"": {
                            ""artist"": [
                                {
                                    ""id"": ""ar-1"",
                                    ""name"": ""Test Artist"",
                                    ""albumCount"": 2
                                }
                            ],
                            ""album"": [
                                {
                                    ""id"": ""al-1"",
                                    ""name"": ""Test Album"",
                                    ""artist"": ""Test Artist"",
                                    ""artistId"": ""ar-1"",
                                    ""year"": 2023,
                                    ""songCount"": 10
                                }
                            ],
                            ""song"": [
                                {
                                    ""id"": ""song-1"",
                                    ""parent"": ""al-1"",
                                    ""title"": ""Test Song"",
                                    ""album"": ""Test Album"",
                                    ""artist"": ""Test Artist"",
                                    ""year"": 2023,
                                    ""track"": 1,
                                    ""duration"": 180,
                                    ""size"": 3600000,
                                    ""contentType"": ""audio/mp3"",
                                    ""suffix"": ""mp3""
                                }
                            ],
                            ""artistCount"": 5,
                            ""artistOffset"": 0,
                            ""albumCount"": 10,
                            ""albumOffset"": 0,
                            ""songCount"": 20,
                            ""songOffset"": 0
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/search3.view", search3Json);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Search.Search3Async("test", 10, 0, 10, 0, 10, 0);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.SearchResult.Should().NotBeNull();

            // Check artists
            response.SearchResult.Artists.Should().NotBeNull();
            response.SearchResult.Artists.Should().HaveCount(1);
            response.SearchResult.Artists[0].Id.Should().Be("ar-1");
            response.SearchResult.Artists[0].Name.Should().Be("Test Artist");

            // Check albums
            response.SearchResult.Albums.Should().NotBeNull();
            response.SearchResult.Albums.Should().HaveCount(1);
            response.SearchResult.Albums[0].Id.Should().Be("al-1");
            response.SearchResult.Albums[0].Name.Should().Be("Test Album");

            // Check songs
            response.SearchResult.Songs.Should().NotBeNull();
            response.SearchResult.Songs.Should().HaveCount(1);
            response.SearchResult.Songs[0].Id.Should().Be("song-1");
            response.SearchResult.Songs[0].Title.Should().Be("Test Song");

            // Check count and offset properties
            response.SearchResult.ArtistCount.Should().Be(5);
            response.SearchResult.ArtistOffset.Should().Be(0);
            response.SearchResult.AlbumCount.Should().Be(10);
            response.SearchResult.AlbumOffset.Should().Be(0);
            response.SearchResult.SongCount.Should().Be(20);
            response.SearchResult.SongOffset.Should().Be(0);
        }

        /// <summary>
        /// Tests that Search with invalid parameters throws appropriate exceptions.
        /// </summary>
        [Fact]
        public async Task Search_WithInvalidQuery_ShouldThrowArgumentException()
        {
            // Arrange
            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => client.Search.SearchAsync(string.Empty)
            );
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            await Assert.ThrowsAsync<ArgumentException>(() => client.Search.SearchAsync(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
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
