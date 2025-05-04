// <copyright file="ConnectionTests.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Tests.Fixtures;

using Xunit;

namespace SubSonicMedia.Tests.Core
{
    /// <summary>
    /// Tests for the connection and authentication functionality.
    /// </summary>
    [Collection("TestInit")]
    public class ConnectionTests : IClassFixture<WireMockServerFixture>, IDisposable
    {
        private readonly WireMockServerFixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionTests"/> class.
        /// </summary>
        /// <param name="fixture">The WireMock server fixture.</param>
        public ConnectionTests(WireMockServerFixture fixture)
        {
            this._fixture = fixture;
            this._fixture.Reset();
        }

        /// <summary>
        /// Tests that the client can connect to the server and ping successfully.
        /// </summary>
        [Fact]
        public async Task PingTest_ShouldReturnSuccess()
        {
            // Arrange
            const string pingResponseJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1""
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/ping.view", pingResponseJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.System.PingAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Status.Should().Be("ok");
            response.Version.Should().Be("1.16.1");
            response.Error.Should().BeNull();
        }

        /// <summary>
        /// Tests that authentication failure is handled properly.
        /// </summary>
        [Fact]
        public async Task Authentication_WhenInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            const string authFailureResponse =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""failed"",
                        ""version"": ""1.16.1"",
                        ""error"": {
                            ""code"": 40,
                            ""message"": ""Wrong username or password""
                        }
                    }
                }";

            // Setup wrong credentials to cause auth failure
            this._fixture.SetupApiEndpoint(
                "rest/ping.view",
                authFailureResponse,
                HttpStatusCode.Unauthorized
            );

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "wrong",
                Password = "wrong",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<SubsonicApiException>(
                () => client.System.PingAsync()
            );
            ex.Should().BeOfType<SubsonicApiException>();
            ex.Message.Should().Contain("Unauthorized");
        }

        /// <summary>
        /// Tests that server connection errors are handled properly.
        /// </summary>
        [Fact]
        public async Task Connection_WhenServerUnavailable_ShouldThrowException()
        {
            // Arrange - Use a port that's definitely not in use
            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = "http://localhost:59999",
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<SubsonicApiException>(
                () => client.System.PingAsync()
            );
            ex.Message.Should().Contain("Error executing request");
            ex.InnerException.Should().NotBeNull();
            ex.InnerException.Should().BeOfType<HttpRequestException>();
        }

        /// <summary>
        /// Tests that API version validation works.
        /// </summary>
        [Fact]
        public async Task ApiVersion_WhenIncompatible_ShouldThrowException()
        {
            // Arrange
            string incompatibleVersionResponse =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""failed"",
                        ""version"": ""1.16.1"",
                        ""error"": {
                            ""code"": 50,
                            ""message"": ""Incompatible Subsonic REST protocol version. Client must upgrade.""
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint(
                "rest/ping.view",
                incompatibleVersionResponse,
                HttpStatusCode.OK
            );

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.0.0", // Very old version
            };

            var client = new SubsonicClient(connectionInfo);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<SubsonicApiException>(
                () => client.System.PingAsync()
            );
            ex.Message.Should().Contain("Incompatible Subsonic REST protocol version");
            ex.ErrorCode.Should().Be(50);
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
