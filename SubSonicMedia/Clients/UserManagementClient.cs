// <copyright file="UserManagementClient.cs" company="Fabian Schmieder">
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Interfaces;
using SubSonicMedia.Responses.UserManagement;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for user management-related Subsonic API methods.
    /// </summary>
    internal class UserManagementClient : IUserManagementClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManagementClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public UserManagementClient(SubsonicClient client)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public Task<UserResponse> GetUserAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(username));
            }

            var parameters = new Dictionary<string, string> { { "username", username } };

            return this._client.ExecuteRequestAsync<UserResponse>(
                "getUser",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<UsersResponse> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            return this._client.ExecuteRequestAsync<UsersResponse>(
                "getUsers",
                new Dictionary<string, string>(),
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task CreateUserAsync(
            string username,
            string password,
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
            IEnumerable<string>? musicFolderIds = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty", nameof(password));
            }

            var parameters = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
            };

            if (!string.IsNullOrEmpty(email))
            {
                parameters.Add("email", email);
            }

            if (ldapAuthenticated.HasValue)
            {
                parameters.Add(
                    "ldapAuthenticated",
                    ldapAuthenticated.Value.ToString().ToLowerInvariant()
                );
            }

            AddRoleParameters(
                parameters,
                adminRole,
                settingsRole,
                streamRole,
                jukeboxRole,
                downloadRole,
                uploadRole,
                playlistRole,
                coverArtRole,
                commentRole,
                podcastRole,
                shareRole,
                videoConversionRole
            );

            if (musicFolderIds != null)
            {
                var folderIdsList = musicFolderIds.Where(id => !string.IsNullOrEmpty(id)).ToList();
                if (folderIdsList.Any())
                {
                    parameters.Add("musicFolderId", string.Join(",", folderIdsList));
                }
            }

            return this._client.ExecuteRequestAsync<Responses.SubsonicResponse>(
                "createUser",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task UpdateUserAsync(
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
            IEnumerable<string>? musicFolderIds = null,
            int? maxBitRate = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(username));
            }

            var parameters = new Dictionary<string, string> { { "username", username } };

            if (!string.IsNullOrEmpty(password))
            {
                parameters.Add("password", password);
            }

            if (!string.IsNullOrEmpty(email))
            {
                parameters.Add("email", email);
            }

            if (ldapAuthenticated.HasValue)
            {
                parameters.Add(
                    "ldapAuthenticated",
                    ldapAuthenticated.Value.ToString().ToLowerInvariant()
                );
            }

            AddRoleParameters(
                parameters,
                adminRole,
                settingsRole,
                streamRole,
                jukeboxRole,
                downloadRole,
                uploadRole,
                playlistRole,
                coverArtRole,
                commentRole,
                podcastRole,
                shareRole,
                videoConversionRole
            );

            if (musicFolderIds != null)
            {
                var folderIdsList = musicFolderIds.Where(id => !string.IsNullOrEmpty(id)).ToList();
                if (folderIdsList.Any())
                {
                    parameters.Add("musicFolderId", string.Join(",", folderIdsList));
                }
            }

            if (maxBitRate.HasValue)
            {
                parameters.Add("maxBitRate", maxBitRate.Value.ToString());
            }

            return this._client.ExecuteRequestAsync<Responses.SubsonicResponse>(
                "updateUser",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task DeleteUserAsync(string username, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(username));
            }

            var parameters = new Dictionary<string, string> { { "username", username } };

            return this._client.ExecuteRequestAsync<Responses.SubsonicResponse>(
                "deleteUser",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task ChangePasswordAsync(
            string username,
            string password,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty", nameof(password));
            }

            var parameters = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
            };

            return this._client.ExecuteRequestAsync<Responses.SubsonicResponse>(
                "changePassword",
                parameters,
                cancellationToken
            );
        }

        /// <summary>
        /// Helper method to add role parameters to the request.
        /// </summary>
        /// <param name="parameters">The parameters dictionary to add to.</param>
        /// <param name="adminRole">Admin role flag.</param>
        /// <param name="settingsRole">Settings role flag.</param>
        /// <param name="streamRole">Stream role flag.</param>
        /// <param name="jukeboxRole">Jukebox role flag.</param>
        /// <param name="downloadRole">Download role flag.</param>
        /// <param name="uploadRole">Upload role flag.</param>
        /// <param name="playlistRole">Playlist role flag.</param>
        /// <param name="coverArtRole">Cover art role flag.</param>
        /// <param name="commentRole">Comment role flag.</param>
        /// <param name="podcastRole">Podcast role flag.</param>
        /// <param name="shareRole">Share role flag.</param>
        /// <param name="videoConversionRole">Video conversion role flag.</param>
        private static void AddRoleParameters(
            Dictionary<string, string> parameters,
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
            bool? videoConversionRole = null
        )
        {
            if (adminRole.HasValue)
            {
                parameters.Add("adminRole", adminRole.Value.ToString().ToLowerInvariant());
            }

            if (settingsRole.HasValue)
            {
                parameters.Add("settingsRole", settingsRole.Value.ToString().ToLowerInvariant());
            }

            if (streamRole.HasValue)
            {
                parameters.Add("streamRole", streamRole.Value.ToString().ToLowerInvariant());
            }

            if (jukeboxRole.HasValue)
            {
                parameters.Add("jukeboxRole", jukeboxRole.Value.ToString().ToLowerInvariant());
            }

            if (downloadRole.HasValue)
            {
                parameters.Add("downloadRole", downloadRole.Value.ToString().ToLowerInvariant());
            }

            if (uploadRole.HasValue)
            {
                parameters.Add("uploadRole", uploadRole.Value.ToString().ToLowerInvariant());
            }

            if (playlistRole.HasValue)
            {
                parameters.Add("playlistRole", playlistRole.Value.ToString().ToLowerInvariant());
            }

            if (coverArtRole.HasValue)
            {
                parameters.Add("coverArtRole", coverArtRole.Value.ToString().ToLowerInvariant());
            }

            if (commentRole.HasValue)
            {
                parameters.Add("commentRole", commentRole.Value.ToString().ToLowerInvariant());
            }

            if (podcastRole.HasValue)
            {
                parameters.Add("podcastRole", podcastRole.Value.ToString().ToLowerInvariant());
            }

            if (shareRole.HasValue)
            {
                parameters.Add("shareRole", shareRole.Value.ToString().ToLowerInvariant());
            }

            if (videoConversionRole.HasValue)
            {
                parameters.Add(
                    "videoConversionRole",
                    videoConversionRole.Value.ToString().ToLowerInvariant()
                );
            }
        }
    }
}
