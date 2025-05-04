// <copyright file="UserClientTests.cs" company="Fabian Schmieder">
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

using System.Text;

using FluentAssertions;

using SubSonicMedia.Exceptions;
using SubSonicMedia.Models;
using SubSonicMedia.Tests.Fixtures;
using SubSonicMedia.Tests.Helpers;

using Xunit;

namespace SubSonicMedia.Tests.Clients
{
    /// <summary>
    /// Tests for the UserClient implementation.
    /// </summary>
    [Collection("TestInit")]
    public class UserClientTests : IClassFixture<WireMockServerFixture>, IDisposable
    {
        private readonly WireMockServerFixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClientTests"/> class.
        /// </summary>
        /// <param name="fixture">The WireMock server fixture.</param>
        public UserClientTests(WireMockServerFixture fixture)
        {
            this._fixture = fixture;
            this._fixture.Reset();
        }

        /// <summary>
        /// Tests that GetUserAsync returns user information for a valid username.
        /// </summary>
        [Fact]
        public async Task GetUserAsync_WithValidUsername_ShouldReturnUserInfo()
        {
            // Arrange
            const string userResponseJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""user"": {
                            ""username"": ""testuser"",
                            ""email"": ""testuser@example.com"",
                            ""scrobblingEnabled"": ""true"",
                            ""adminRole"": true,
                            ""settingsRole"": true,
                            ""downloadRole"": true,
                            ""uploadRole"": true,
                            ""playlistRole"": true,
                            ""coverArtRole"": true,
                            ""commentRole"": true,
                            ""podcastRole"": true,
                            ""streamRole"": true,
                            ""jukeboxRole"": true,
                            ""shareRole"": true,
                            ""videoConversionRole"": true,
                            ""maxBitRate"": 0,
                            ""avatarScheme"": ""system"",
                            ""folder"": [1, 2, 3]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getUser.view", userResponseJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.User.GetUserAsync("testuser");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.User.Should().NotBeNull();
            response.User.Username.Should().Be("testuser");
            response.User.Email.Should().Be("testuser@example.com");
            response.User.IsAdmin.Should().BeTrue();
            response.User.SettingsRole.Should().BeTrue();
            response.User.DownloadRole.Should().BeTrue();
            response.User.UploadRole.Should().BeTrue();
            response.User.PlaylistRole.Should().BeTrue();
            response.User.CoverArtRole.Should().BeTrue();
            response.User.CommentRole.Should().BeTrue();
            response.User.PodcastRole.Should().BeTrue();
            response.User.StreamRole.Should().BeTrue();
            response.User.JukeboxRole.Should().BeTrue();
            response.User.ShareRole.Should().BeTrue();
            response.User.VideoConversionRole.Should().BeTrue();
        }

        /// <summary>
        /// Tests that GetUserAsync throws an ArgumentException for an empty username.
        /// </summary>
        [Fact]
        public async Task GetUserAsync_WithEmptyUsername_ShouldThrowArgumentException()
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
                async () => await client.User.GetUserAsync(string.Empty)
            );
        }

        /// <summary>
        /// Tests that GetUsersAsync returns a list of users.
        /// </summary>
        [Fact]
        public async Task GetUsersAsync_ShouldReturnUsersList()
        {
            // Arrange
            const string usersResponseJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""users"": {
                            ""user"": [
                                {
                                    ""username"": ""admin"",
                                    ""email"": ""admin@example.com"",
                                    ""scrobblingEnabled"": ""true"",
                                    ""adminRole"": true,
                                    ""settingsRole"": true,
                                    ""downloadRole"": true,
                                    ""uploadRole"": true,
                                    ""playlistRole"": true,
                                    ""coverArtRole"": true,
                                    ""commentRole"": true,
                                    ""podcastRole"": true,
                                    ""streamRole"": true,
                                    ""jukeboxRole"": true,
                                    ""shareRole"": true,
                                    ""videoConversionRole"": true
                                },
                                {
                                    ""username"": ""user1"",
                                    ""email"": ""user1@example.com"",
                                    ""scrobblingEnabled"": ""true"",
                                    ""streamRole"": true,
                                    ""downloadRole"": true,
                                    ""playlistRole"": true
                                }
                            ]
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getUsers.view", usersResponseJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.User.GetUsersAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Users.Should().NotBeNull();
            response.Users.User.Should().HaveCount(2);

            var admin = response.Users.User.FirstOrDefault(u => u.Username == "admin");
            admin.Should().NotBeNull();
            admin!.Email.Should().Be("admin@example.com");
            admin.IsAdmin.Should().BeTrue();
            admin.DownloadRole.Should().BeTrue();

            var user1 = response.Users.User.FirstOrDefault(u => u.Username == "user1");
            user1.Should().NotBeNull();
            user1!.Email.Should().Be("user1@example.com");
            user1.IsAdmin.Should().BeFalse();
            user1.DownloadRole.Should().BeTrue();
            user1.PlaylistRole.Should().BeTrue();
        }

        /// <summary>
        /// Tests that CreateUserAsync creates a user successfully.
        /// </summary>
        [Fact]
        public async Task CreateUserAsync_WithValidParameters_ShouldReturnSuccess()
        {
            // Arrange
            const string createUserResponseJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1""
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/createUser.view", createUserResponseJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.User.CreateUserAsync(
                "newuser",
                "password123",
                "newuser@example.com",
                adminRole: false,
                downloadRole: true,
                streamRole: true,
                playlistRole: true
            );

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Status.Should().Be("ok");
        }

        /// <summary>
        /// Tests that CreateUserAsync throws ArgumentException for empty username.
        /// </summary>
        [Fact]
        public async Task CreateUserAsync_WithEmptyUsername_ShouldThrowArgumentException()
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
                async () =>
                    await client.User.CreateUserAsync(
                        string.Empty,
                        "password123",
                        "newuser@example.com"
                    )
            );
        }

        /// <summary>
        /// Tests that UpdateUserAsync updates a user successfully.
        /// </summary>
        [Fact]
        public async Task UpdateUserAsync_WithValidParameters_ShouldReturnSuccess()
        {
            // Arrange
            const string updateUserResponseJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1""
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/updateUser.view", updateUserResponseJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.User.UpdateUserAsync(
                "existinguser",
                email: "updated@example.com",
                maxBitRate: 320,
                downloadRole: true
            );

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Status.Should().Be("ok");
        }

        /// <summary>
        /// Tests that DeleteUserAsync deletes a user successfully.
        /// </summary>
        [Fact]
        public async Task DeleteUserAsync_WithValidUsername_ShouldReturnSuccess()
        {
            // Arrange
            const string deleteUserResponseJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1""
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/deleteUser.view", deleteUserResponseJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.User.DeleteUserAsync("userToDelete");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Status.Should().Be("ok");
        }

        /// <summary>
        /// Tests that ChangePasswordAsync changes a password successfully.
        /// </summary>
        [Fact]
        public async Task ChangePasswordAsync_WithValidParameters_ShouldReturnSuccess()
        {
            // Arrange
            const string changePasswordResponseJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1""
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/changePassword.view", changePasswordResponseJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.User.ChangePasswordAsync("existinguser", "newpassword123");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Status.Should().Be("ok");
        }

        /// <summary>
        /// Tests that GetAvatarAsync returns an avatar stream.
        /// </summary>
        [Fact]
        public async Task GetAvatarAsync_WithValidUsername_ShouldReturnAvatarStream()
        {
            // Arrange
            byte[] avatarData = Encoding.UTF8.GetBytes("Mock avatar image data");

            this._fixture.SetupBinaryEndpoint("rest/getAvatar.view", avatarData, "image/jpeg");

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var stream = await client.User.GetAvatarAsync("user1");

            // Assert
            stream.Should().NotBeNull();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] resultBytes = memoryStream.ToArray();
            resultBytes.Should().BeEquivalentTo(avatarData);
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
