// <copyright file="SubsonicClient.cs" company="Fabian Schmieder">
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
// along with SubSonicMedia. If not, see &lt;https://www.gnu.org/licenses/&gt;.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SubSonicMedia.Authentication;
using SubSonicMedia.Clients;
using SubSonicMedia.Exceptions;
using SubSonicMedia.Interfaces;
using SubSonicMedia.Models;
using SubSonicMedia.Responses;
using SubSonicMedia.Utilities;

namespace SubSonicMedia
{
    /// <summary>
    /// Main client for interacting with a Subsonic server.
    /// </summary>
    public class SubsonicClient : ISubsonicClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly SubsonicConnectionInfo _connectionInfo;
        private readonly IAuthenticationProvider _authProvider;
        private readonly ILogger<SubsonicClient> _logger;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubsonicClient"/> class.
        /// </summary>
        /// <param name="connectionInfo">Connection information for the Subsonic server.</param>
        /// <param name="authProvider">Authentication provider. If null, TokenAuthenticationProvider will be used.</param>
        /// <param name="httpClient">Optional HTTP client. If null, a new one will be created.</param>
        /// <param name="logger">Optional logger. If null, NullLogger will be used.</param>
        public SubsonicClient(
            SubsonicConnectionInfo connectionInfo,
            IAuthenticationProvider? authProvider = null,
            HttpClient? httpClient = null,
            ILogger<SubsonicClient>? logger = null
        )
        {
            this._connectionInfo =
                connectionInfo ?? throw new ArgumentNullException(nameof(connectionInfo));

            if (string.IsNullOrWhiteSpace(connectionInfo.ServerUrl))
            {
                throw new ArgumentException("Server URL cannot be empty", nameof(connectionInfo));
            }

            if (string.IsNullOrWhiteSpace(connectionInfo.Username))
            {
                throw new ArgumentException("Username cannot be empty", nameof(connectionInfo));
            }

            if (string.IsNullOrWhiteSpace(connectionInfo.Password))
            {
                throw new ArgumentException("Password cannot be empty", nameof(connectionInfo));
            }

            // Validate API version
            if (
                !string.IsNullOrWhiteSpace(connectionInfo.ApiVersion)
                && !VersionInfo.IsApiVersionSupported(connectionInfo.ApiVersion)
            )
            {
                throw new SubsonicApiVersionException(
                    connectionInfo.ApiVersion,
                    VersionInfo.SubsonicApiVersion
                );
            }

            this._httpClient = httpClient ?? new HttpClient();

            // Ensure the base address ends with a slash
            string baseAddress = connectionInfo.ServerUrl.TrimEnd('/') + "/";
            this._httpClient.BaseAddress = new Uri(baseAddress);

            this._authProvider = authProvider ?? new TokenAuthenticationProvider();
            this._logger = logger ?? NullLogger<SubsonicClient>.Instance;

            // Initialize the feature clients
            this.Browsing = new BrowsingClient(this);
            this.Media = new MediaClient(this);
            this.Playlists = new PlaylistClient(this);
            this.Search = new SearchClient(this);
            this.UserManagement = new UserManagementClient(this);
            this.User = new UserClient(this);
            this.System = new SystemClient(this);
            this.Chat = new ChatClient(this);
            this.Jukebox = new JukeboxClient(this);
            this.Video = new VideoClient(this);
            this.Podcasts = new PodcastClient(this);
            this.Radio = new RadioClient(this);
            this.Bookmarks = new BookmarkClient(this);
        }

        /// <summary>
        /// Gets the browsing client that provides access to browsing-related API methods.
        /// </summary>
        public IBrowsingClient Browsing { get; }

        /// <summary>
        /// Gets the media client that provides access to media-related API methods.
        /// </summary>
        public IMediaClient Media { get; }

        /// <summary>
        /// Gets the playlist client that provides access to playlist-related API methods.
        /// </summary>
        public IPlaylistClient Playlists { get; }

        /// <summary>
        /// Gets the search client that provides access to search-related API methods.
        /// </summary>
        public ISearchClient Search { get; }

        /// <summary>
        /// Gets the user management client that provides access to user-related API methods.
        /// </summary>
        public IUserManagementClient UserManagement { get; }

        /// <summary>
        /// Gets the user client that provides access to user avatar and management methods.
        /// </summary>
        public IUserClient User { get; }

        /// <summary>
        /// Gets the system client that provides access to system-related API methods.
        /// </summary>
        public ISystemClient System { get; }

        /// <summary>
        /// Gets the chat client that provides access to chat-related API methods.
        /// </summary>
        public IChatClient Chat { get; }

        /// <summary>
        /// Gets the jukebox client that provides access to jukebox-related API methods.
        /// </summary>
        public IJukeboxClient Jukebox { get; }

        /// <summary>
        /// Gets the video client that provides access to video-related API methods.
        /// </summary>
        public IVideoClient Video { get; }

        /// <summary>
        /// Gets the podcast client that provides access to podcast-related API methods.
        /// </summary>
        public IPodcastClient Podcasts { get; }

        /// <summary>
        /// Gets the radio client that provides access to internet radio-related API methods.
        /// </summary>
        public IRadioClient Radio { get; }

        /// <summary>
        /// Gets the bookmark client that provides access to bookmark-related API methods.
        /// </summary>
        public IBookmarkClient Bookmarks { get; }

        /// <summary>
        /// Executes a request and returns a typed response.
        /// </summary>
        /// <typeparam name="T">The type of response to return.</typeparam>
        /// <param name="endpoint">The API endpoint to call.</param>
        /// <param name="parameters">Optional parameters to include in the request.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>The response from the server.</returns>
        internal async Task<T> ExecuteRequestAsync<T>(
            string endpoint,
            Dictionary<string, string>? parameters = null,
            CancellationToken cancellationToken = default
        )
            where T : SubsonicResponse, new()
        {
            try
            {
                var requestParameters = parameters ?? new Dictionary<string, string>();
                this._authProvider.ApplyAuthentication(requestParameters, this._connectionInfo);

                var requestBuilder = new RequestBuilder(endpoint);
                foreach (var parameter in requestParameters)
                {
                    requestBuilder.AddParameter(parameter.Key, parameter.Value);
                }

                string requestUrl = requestBuilder.BuildRequestUrl();
                string fullUrl = $"{this._httpClient.BaseAddress}{requestUrl}";
                this._logger.LogDebug("Executing request: {RequestUrl}", fullUrl);

                using var response = await this
                    ._httpClient.GetAsync(requestUrl, cancellationToken)
                    .ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new SubsonicApiException(
                        (int)response.StatusCode,
                        $"HTTP error: {response.StatusCode}"
                    );
                }

                string content = await response
                    .Content.ReadAsStringAsync(cancellationToken)
                    .ConfigureAwait(false);

                T result;
                // Use the appropriate parser based on the response format
                if (this._connectionInfo.ResponseFormat?.ToLower() == "json")
                {
                    result = JsonParser.Parse<T>(content);
                }
                else
                {
                    result = XmlParser.Parse<T>(content);
                }

                if (!result.IsSuccess)
                {
                    if (result.Error != null)
                    {
                        if (result.Error.Code == 40 || result.Error.Code == 41)
                        {
                            throw new SubsonicAuthenticationException(
                                result.Error.Code,
                                result.Error.Message
                            );
                        }

                        throw SubsonicApiException.FromError(result.Error);
                    }

                    throw new SubsonicApiException(0, "Unknown error");
                }

                return result;
            }
            catch (Exception ex)
                when (ex is not SubsonicApiException && ex is not OperationCanceledException)
            {
                this._logger.LogError(ex, "Error executing request to {Endpoint}", endpoint);
                throw new SubsonicApiException(0, $"Error executing request: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Executes a request and returns a binary stream.
        /// </summary>
        /// <param name="endpoint">The API endpoint to call.</param>
        /// <param name="parameters">Optional parameters to include in the request.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A stream containing the binary data.</returns>
        internal async Task<Stream> ExecuteBinaryRequestAsync(
            string endpoint,
            Dictionary<string, string>? parameters = null,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var requestParameters = parameters ?? new Dictionary<string, string>();
                this._authProvider.ApplyAuthentication(requestParameters, this._connectionInfo);

                var requestBuilder = new RequestBuilder(endpoint);
                foreach (var parameter in requestParameters)
                {
                    requestBuilder.AddParameter(parameter.Key, parameter.Value);
                }

                string requestUrl = requestBuilder.BuildRequestUrl();
                this._logger.LogDebug("Executing binary request: {RequestUrl}", requestUrl);

                var response = await this
                    ._httpClient.GetAsync(
                        requestUrl,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken
                    )
                    .ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    // Check for error responses in either XML or JSON format
                    var contentType = response.Content.Headers.ContentType?.MediaType;
                    if (
                        contentType?.StartsWith("text/xml", StringComparison.OrdinalIgnoreCase)
                            == true
                        || contentType?.StartsWith(
                            "application/json",
                            StringComparison.OrdinalIgnoreCase
                        ) == true
                    )
                    {
                        // This is an error response
                        string content = await response
                            .Content.ReadAsStringAsync(cancellationToken)
                            .ConfigureAwait(false);

                        SubsonicResponse errorResponse;
                        if (this._connectionInfo.ResponseFormat?.ToLower() == "json")
                        {
                            errorResponse = JsonParser.Parse<SubsonicResponse>(content);
                        }
                        else
                        {
                            errorResponse = XmlParser.Parse<SubsonicResponse>(content);
                        }

                        if (!errorResponse.IsSuccess && errorResponse.Error != null)
                        {
                            throw SubsonicApiException.FromError(errorResponse.Error);
                        }
                    }

                    throw new SubsonicApiException(
                        (int)response.StatusCode,
                        $"HTTP error: {response.StatusCode}"
                    );
                }

                return await response
                    .Content.ReadAsStreamAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
                when (ex is not SubsonicApiException && ex is not OperationCanceledException)
            {
                this._logger.LogError(ex, "Error executing binary request to {Endpoint}", endpoint);
                throw new SubsonicApiException(
                    0,
                    $"Error executing binary request: {ex.Message}",
                    ex
                );
            }
        }

        /// <summary>
        /// Disposes the HTTP client.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes resources.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this._httpClient?.Dispose();
            }

            this._disposed = true;
        }
    }
}
