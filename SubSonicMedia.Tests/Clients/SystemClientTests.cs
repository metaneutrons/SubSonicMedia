// <copyright file="SystemClientTests.cs" company="Fabian Schmieder">
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

using FluentAssertions;

using SubSonicMedia.Exceptions;
using SubSonicMedia.Models;
using SubSonicMedia.Tests.Fixtures;

using Xunit;

namespace SubSonicMedia.Tests.Clients
{
    /// <summary>
    /// Tests for the SystemClient implementation.
    /// </summary>
    [Collection("TestInit")]
    public class SystemClientTests : IClassFixture<WireMockServerFixture>, IDisposable
    {
        private readonly WireMockServerFixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemClientTests"/> class.
        /// </summary>
        /// <param name="fixture">The WireMock server fixture.</param>
        public SystemClientTests(WireMockServerFixture fixture)
        {
            this._fixture = fixture;
            this._fixture.Reset();
        }

        /// <summary>
        /// Tests that PingAsync returns a successful response.
        /// </summary>
        [Fact]
        public async Task PingAsync_ShouldReturnSuccessResponse()
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
        }

        /// <summary>
        /// Tests that GetLicenseAsync returns license information.
        /// </summary>
        [Fact]
        public async Task GetLicenseAsync_ShouldReturnLicenseInfo()
        {
            // Arrange
            const string licenseResponseJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""license"": {
                            ""valid"": true,
                            ""email"": ""test@example.com"",
                            ""key"": ""ABCDEF123456"",
                            ""licenseExpires"": ""2024-12-31T00:00:00.000Z"",
                            ""licenseVersion"": ""Super Sonic Server""
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getLicense.view", licenseResponseJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.System.GetLicenseAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.License.Should().NotBeNull();
            response.License.Valid.Should().BeTrue();
            response.License.Email.Should().Be("test@example.com");
            response.License.Key.Should().Be("ABCDEF123456");
            response.License.LicenseVersion.Should().Be("Super Sonic Server");
        }

        /// <summary>
        /// Tests that GetScanStatusAsync returns scan status information.
        /// </summary>
        [Fact]
        public async Task GetScanStatusAsync_ShouldReturnScanStatus()
        {
            // Arrange
            const string scanStatusResponseJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""scanStatus"": {
                            ""scanning"": true,
                            ""count"": 123,
                            ""folderCount"": 45,
                            ""folder"": ""/music/rock""
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/getScanStatus.view", scanStatusResponseJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.System.GetScanStatusAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ScanStatus.Should().NotBeNull();
            response.ScanStatus.Scanning.Should().BeTrue();
            response.ScanStatus.Count.Should().Be(123);
            response.ScanStatus.FolderCount.Should().Be(45);
            response.ScanStatus.Folder.Should().Be("/music/rock");
        }

        /// <summary>
        /// Tests that StartScanAsync returns scan status information.
        /// </summary>
        [Fact]
        public async Task StartScanAsync_ShouldStartScanAndReturnStatus()
        {
            // Arrange
            const string startScanResponseJson =
                @"{
                    ""subsonic-response"": {
                        ""status"": ""ok"",
                        ""version"": ""1.16.1"",
                        ""scanStatus"": {
                            ""scanning"": true,
                            ""count"": 0,
                            ""folderCount"": 0,
                            ""folder"": ""/music""
                        }
                    }
                }";

            this._fixture.SetupApiEndpoint("rest/startScan.view", startScanResponseJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act
            var response = await client.System.StartScanAsync();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ScanStatus.Should().NotBeNull();
            response.ScanStatus.Scanning.Should().BeTrue();
            response.ScanStatus.Count.Should().Be(0);
            response.ScanStatus.FolderCount.Should().Be(0);
            response.ScanStatus.Folder.Should().Be("/music");
        }

        /// <summary>
        /// Tests that PingAsync throws SubsonicAuthenticationException for authentication errors.
        /// </summary>
        [Fact]
        public async Task PingAsync_WithAuthErrorResponse_ShouldThrowAuthException()
        {
            // Arrange
            const string errorResponseJson =
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

            this._fixture.SetupApiEndpoint("rest/ping.view", errorResponseJson);

            var connectionInfo = new SubsonicConnectionInfo
            {
                ServerUrl = this._fixture.BaseUrl,
                Username = "test",
                Password = "test",
                ApiVersion = "1.16.1",
            };

            var client = new SubsonicClient(connectionInfo);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SubsonicAuthenticationException>(
                async () => await client.System.PingAsync()
            );

            exception.Should().NotBeNull();
            exception.ErrorCode.Should().Be(40);
            exception.Message.Should().Be("Wrong username or password");
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
