// <copyright file="PlaylistClientTests.cs" company="Fabian Schmieder">
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

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SubSonicMedia.Models;
using SubSonicMedia.Tests.Fixtures;
using SubSonicMedia.Tests.Helpers;
using Xunit;

namespace SubSonicMedia.Tests.Clients
{
    /// <summary>
    /// Tests for the PlaylistClient implementation.
    /// </summary>
    [Collection("TestInit")]
    public class PlaylistClientTests : IClassFixture<WireMockServerFixture>, IDisposable
    {
        private readonly WireMockServerFixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistClientTests"/> class.
        /// </summary>
        /// <param name="fixture">The WireMock server fixture.</param>
        public PlaylistClientTests(WireMockServerFixture fixture)
        {
            this._fixture = fixture;
            this._fixture.Reset();
        }

        /// <summary>
        /// Tests that GetPlaylists returns the expected playlists.
        /// </summary>
        [Fact]
        public async Task GetPlaylists_ShouldReturnPlaylists()
        {
            // Arrange
            const string playlistsJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""playlists"": {
                            ""playlist"": [
                                {
                                    ""id"": ""1"",
                                    ""name"": ""Test Playlist"",
                                    ""owner"": ""admin"",
                                    ""public"": true,
                                    ""songCount"": 10,
                                    ""duration"": 3600,
                                    ""created"": ""2023-01-01T12:00:00.000Z"",
                                    ""changed"": ""2023-01-02T12:00:00.000Z""
                                },
                                {
                                    ""id"": ""2"",
                                    ""name"": ""Another Playlist"",
                                    ""owner"": ""admin"",
                                    ""public"": false,
                                    ""songCount"": 5,
                                    ""duration"": 1800,
                                    ""created"": ""2023-01-03T12:00:00.000Z"",
                                    ""changed"": ""2023-01-04T12:00:00.000Z""
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getPlaylists.view", playlistsJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Playlists.GetPlaylistsAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Playlists.Should().NotBeNull();
            response.Playlists.Playlist.Should().NotBeNull();
            response.Playlists.Playlist.Should().HaveCount(2);
            response.Playlists.Playlist[0].Id.Should().Be("1");
            response.Playlists.Playlist[0].Name.Should().Be("Test Playlist");
            response.Playlists.Playlist[0].Owner.Should().Be("admin");
            response.Playlists.Playlist[0].Public.Should().BeTrue();
            response.Playlists.Playlist[0].SongCount.Should().Be(10);
            response.Playlists.Playlist[0].Duration.Should().Be(3600);
            response.Playlists.Playlist[1].Id.Should().Be("2");
            response.Playlists.Playlist[1].Name.Should().Be("Another Playlist");
            response.Playlists.Playlist[1].Public.Should().BeFalse();
        }

        /// <summary>
        /// Tests that GetPlaylist returns the expected playlist details.
        /// </summary>
        [Fact]
        public async Task GetPlaylist_ShouldReturnPlaylistDetails()
        {
            // Arrange
            const string playlistJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""playlist"": {
                            ""id"": ""1"",
                            ""name"": ""Test Playlist"",
                            ""owner"": ""admin"",
                            ""public"": true,
                            ""songCount"": 2,
                            ""duration"": 600,
                            ""created"": ""2023-01-01T12:00:00.000Z"",
                            ""changed"": ""2023-01-02T12:00:00.000Z"",
                            ""entry"": [
                                {
                                    ""id"": ""song-1"",
                                    ""parent"": ""album-1"",
                                    ""title"": ""Test Song 1"",
                                    ""album"": ""Test Album"",
                                    ""artist"": ""Test Artist"",
                                    ""track"": 1,
                                    ""year"": 2023,
                                    ""duration"": 300,
                                    ""size"": 12345678,
                                    ""contentType"": ""audio/mp3"",
                                    ""suffix"": ""mp3"",
                                    ""path"": ""Test Artist/Test Album/01 - Test Song 1.mp3""
                                },
                                {
                                    ""id"": ""song-2"",
                                    ""parent"": ""album-1"",
                                    ""title"": ""Test Song 2"",
                                    ""album"": ""Test Album"",
                                    ""artist"": ""Test Artist"",
                                    ""track"": 2,
                                    ""year"": 2023,
                                    ""duration"": 300,
                                    ""size"": 12345678,
                                    ""contentType"": ""audio/mp3"",
                                    ""suffix"": ""mp3"",
                                    ""path"": ""Test Artist/Test Album/02 - Test Song 2.mp3""
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getPlaylist.view", playlistJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Playlists.GetPlaylistAsync("1");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Playlist.Should().NotBeNull();
            response.Playlist.Id.Should().Be("1");
            response.Playlist.Name.Should().Be("Test Playlist");
            response.Playlist.Owner.Should().Be("admin");
            response.Playlist.Public.Should().BeTrue();
            response.Playlist.SongCount.Should().Be(2);
            response.Playlist.Duration.Should().Be(600);
            response.Playlist.Entry.Should().NotBeNull();
            response.Playlist.Entry.Should().HaveCount(2);
            response.Playlist.Entry[0].Id.Should().Be("song-1");
            response.Playlist.Entry[0].Title.Should().Be("Test Song 1");
            response.Playlist.Entry[1].Id.Should().Be("song-2");
            response.Playlist.Entry[1].Title.Should().Be("Test Song 2");
        }

        /// <summary>
        /// Tests that CreatePlaylist creates a playlist successfully.
        /// </summary>
        [Fact]
        public async Task CreatePlaylist_ShouldCreatePlaylist()
        {
            // Arrange
            const string createPlaylistJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""playlist"": {
                            ""id"": ""new-playlist-1"",
                            ""name"": ""New Test Playlist"",
                            ""owner"": ""test"",
                            ""public"": false,
                            ""songCount"": 2,
                            ""duration"": 600,
                            ""created"": ""2023-01-01T12:00:00.000Z"",
                            ""changed"": ""2023-01-01T12:00:00.000Z"",
                            ""entry"": [
                                {
                                    ""id"": ""song-1"",
                                    ""title"": ""Test Song 1""
                                },
                                {
                                    ""id"": ""song-2"",
                                    ""title"": ""Test Song 2""
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/createPlaylist.view", createPlaylistJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);
            var songIds = new List<string> { "song-1", "song-2" };

            // Act
            var response = await client.Playlists.CreatePlaylistAsync("New Test Playlist", songIds);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Playlist.Should().NotBeNull();
            response.Playlist.Id.Should().Be("new-playlist-1");
            response.Playlist.Name.Should().Be("New Test Playlist");
            response.Playlist.Owner.Should().Be("test");
            response.Playlist.SongCount.Should().Be(2);
            response.Playlist.Entry.Should().HaveCount(2);
            response.Playlist.Entry[0].Id.Should().Be("song-1");
            response.Playlist.Entry[1].Id.Should().Be("song-2");
        }

        /// <summary>
        /// Tests that UpdatePlaylist updates a playlist successfully.
        /// </summary>
        [Fact]
        public async Task UpdatePlaylist_ShouldUpdatePlaylist()
        {
            // Arrange
            const string updatePlaylistJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1""
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/updatePlaylist.view", updatePlaylistJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);
            var songIdsToAdd = new List<string> { "song-3" };
            var songIndicesToRemove = new List<int> { 0 };

            // Act
            await client.Playlists.UpdatePlaylistAsync(
                "1",
                name: "Updated Playlist",
                comment: "Updated comment",
                isPublic: true,
                songIdsToAdd: songIdsToAdd,
                songIndicesToRemove: songIndicesToRemove
            );

            // Assert - No exceptions means success
            // The API only returns a basic success response with no additional data
            true.Should().BeTrue();
        }

        /// <summary>
        /// Tests that DeletePlaylist deletes a playlist successfully.
        /// </summary>
        [Fact]
        public async Task DeletePlaylist_ShouldDeletePlaylist()
        {
            // Arrange
            const string deletePlaylistJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1""
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/deletePlaylist.view", deletePlaylistJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            await client.Playlists.DeletePlaylistAsync("1");

            // Assert - No exceptions means success
            // The API only returns a basic success response with no additional data
            true.Should().BeTrue();
        }

        /// <summary>
        /// Tests that GetPlayQueue returns the expected play queue.
        /// </summary>
        [Fact]
        public async Task GetPlayQueue_ShouldReturnPlayQueue()
        {
            // Arrange
            const string playQueueJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""playQueue"": {
                            ""current"": ""song-1"",
                            ""position"": 12345,
                            ""username"": ""test"",
                            ""changed"": ""2023-01-01T12:00:00.000Z"",
                            ""entry"": [
                                {
                                    ""id"": ""song-1"",
                                    ""title"": ""Test Song 1"",
                                    ""album"": ""Test Album"",
                                    ""artist"": ""Test Artist""
                                },
                                {
                                    ""id"": ""song-2"",
                                    ""title"": ""Test Song 2"",
                                    ""album"": ""Test Album"",
                                    ""artist"": ""Test Artist""
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getPlayQueue.view", playQueueJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.Playlists.GetPlayQueueAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.PlayQueue.Should().NotBeNull();
            response.PlayQueue.Current.Should().Be("song-1");
            response.PlayQueue.Position.Should().Be(12345);
            response.PlayQueue.Username.Should().Be("test");
            response.PlayQueue.Changed.Should().BeGreaterThan(0);
            response.PlayQueue.Entry.Should().NotBeNull();
            response.PlayQueue.Entry.Should().HaveCount(2);
            response.PlayQueue.Entry[0].Id.Should().Be("song-1");
            response.PlayQueue.Entry[0].Title.Should().Be("Test Song 1");
            response.PlayQueue.Entry[1].Id.Should().Be("song-2");
            response.PlayQueue.Entry[1].Title.Should().Be("Test Song 2");
        }

        /// <summary>
        /// Tests that SavePlayQueue saves the play queue successfully.
        /// </summary>
        [Fact]
        public async Task SavePlayQueue_ShouldSavePlayQueue()
        {
            // Arrange
            const string savePlayQueueJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""playQueue"": {
                            ""current"": ""song-1"",
                            ""position"": 12345,
                            ""username"": ""test"",
                            ""changed"": ""2023-01-01T12:00:00.000Z"",
                            ""entry"": [
                                {
                                    ""id"": ""song-1"",
                                    ""title"": ""Test Song 1""
                                },
                                {
                                    ""id"": ""song-2"",
                                    ""title"": ""Test Song 2""
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/savePlayQueue.view", savePlayQueueJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);
            var songIds = new List<string> { "song-1", "song-2" };

            // Act
            var response = await client.Playlists.SavePlayQueueAsync(songIds, "song-1", 12345);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.PlayQueue.Should().NotBeNull();
            response.PlayQueue.Current.Should().Be("song-1");
            response.PlayQueue.Position.Should().Be(12345);
            response.PlayQueue.Username.Should().Be("test");
            response.PlayQueue.Changed.Should().BeGreaterThan(0);
            response.PlayQueue.Entry.Should().NotBeNull();
            response.PlayQueue.Entry.Should().HaveCount(2);
            response.PlayQueue.Entry[0].Id.Should().Be("song-1");
            response.PlayQueue.Entry[1].Id.Should().Be("song-2");
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
