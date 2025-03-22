// <copyright file="WireMockServerFixture.cs" company="Fabian Schmieder">
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

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace SubSonicMedia.Tests.Fixtures
{
    /// <summary>
    /// WireMock server fixture for mocking Subsonic API responses.
    /// </summary>
    public class WireMockServerFixture : IDisposable
    {
        private static int _nextPort = 9876;
        private static readonly object _portLock = new object();
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="WireMockServerFixture"/> class.
        /// </summary>
        public WireMockServerFixture()
        {
            // Get a unique port for this instance to avoid conflicts
            int port;
            lock (_portLock)
            {
                port = _nextPort++;
            }

            try
            {
                // Create the WireMock server with dynamic port allocation
                this.Server = WireMockServer.Start(
                    new WireMockServerSettings
                    {
                        Urls = new[] { $"http://localhost:{port}/" },
                        StartAdminInterface = false,
                        ReadStaticMappings = false,
                    }
                );

                // Set base URL for the Subsonic API client to use
                this.BaseUrl = this.Server.Urls[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting WireMock server: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the WireMock server instance.
        /// </summary>
        public WireMockServer Server { get; }

        /// <summary>
        /// Gets the base URL of the mock server.
        /// </summary>
        public string BaseUrl { get; }

        /// <summary>
        /// Sets up a mock response for an API endpoint.
        /// </summary>
        /// <param name="endpoint">The API endpoint (e.g., "ping.view").</param>
        /// <param name="responseJson">The JSON response to return.</param>
        /// <param name="statusCode">Optional HTTP status code (defaults to 200).</param>
        public void SetupApiEndpoint(
            string endpoint,
            string responseJson,
            HttpStatusCode statusCode = HttpStatusCode.OK
        )
        {
            // Fix the JSON response to use the correct subsonic-response format if needed
            if (!responseJson.Contains("subsonic-response") && !string.IsNullOrEmpty(responseJson))
            {
                responseJson =
                    @"{
                    ""subsonic-response"": "
                    + responseJson
                    + @"
                }";
            }

            this.Server.Given(Request.Create().WithPath($"/{endpoint}").UsingAnyMethod())
                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(statusCode)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(responseJson, encoding: Encoding.UTF8)
                );
        }

        /// <summary>
        /// Sets up a mock binary response for an API endpoint.
        /// </summary>
        /// <param name="endpoint">The API endpoint (e.g., "getCoverArt.view").</param>
        /// <param name="binaryData">The binary data to return.</param>
        /// <param name="contentType">The content type of the response.</param>
        /// <param name="statusCode">Optional HTTP status code (defaults to 200).</param>
        public void SetupBinaryEndpoint(
            string endpoint,
            byte[] binaryData,
            string contentType,
            HttpStatusCode statusCode = HttpStatusCode.OK
        )
        {
            this.Server.Given(Request.Create().WithPath($"/{endpoint}").UsingAnyMethod())
                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(statusCode)
                        .WithHeader("Content-Type", contentType)
                        .WithBody(binaryData)
                );
        }

        /// <summary>
        /// Sets up authentication validation for all endpoints.
        /// </summary>
        /// <param name="username">The expected username.</param>
        /// <param name="password">The expected password (or token).</param>
        /// <param name="failureResponse">The JSON response to return on auth failure.</param>
        public void SetupAuthValidation(string username, string password, string failureResponse)
        {
            this.Server.Given(
                    Request
                        .Create()
                        .WithParam(p => p.ContainsKey("u") && !p["u"].Contains(username))
                        .UsingAnyMethod()
                )
                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(HttpStatusCode.Unauthorized)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(failureResponse, encoding: Encoding.UTF8)
                );

            this.Server.Given(
                    Request
                        .Create()
                        .WithParam(p => p.ContainsKey("p") && !p["p"].Contains(password))
                        .UsingAnyMethod()
                )
                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(HttpStatusCode.Unauthorized)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(failureResponse, encoding: Encoding.UTF8)
                );
        }

        /// <summary>
        /// Resets all configured mappings on the server.
        /// </summary>
        public void Reset()
        {
            this.Server.Reset();
        }

        /// <summary>
        /// Disposes the WireMock server.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the WireMock server.
        /// </summary>
        /// <param name="disposing">True if disposing; false if finalizing.</param>
        [SuppressMessage(
            "Usage",
            "CA1816:Dispose methods should call SuppressFinalize",
            Justification = "SuppressFinalize is called in the public Dispose method."
        )]
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this.Server?.Dispose();
                }

                this._disposed = true;
            }
        }
    }
}
