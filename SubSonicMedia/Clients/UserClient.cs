// <copyright file="UserClient.cs" company="Fabian Schmieder">
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
using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Interfaces;
using SubSonicMedia.Responses.System;
using SubSonicMedia.Responses.User;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for accessing Subsonic API user methods.
    /// </summary>
    internal class UserClient : IUserClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public UserClient(SubsonicClient client)
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
        public Task<BaseResponse> CreateUserAsync(
            string username,
            string password,
            string email,
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
            string? musicFolderIds = null,
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

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            var parameters = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "email", email },
            };

            AddBoolParameter(parameters, "ldapAuthenticated", ldapAuthenticated);
            AddBoolParameter(parameters, "adminRole", adminRole);
            AddBoolParameter(parameters, "settingsRole", settingsRole);
            AddBoolParameter(parameters, "streamRole", streamRole);
            AddBoolParameter(parameters, "jukeboxRole", jukeboxRole);
            AddBoolParameter(parameters, "downloadRole", downloadRole);
            AddBoolParameter(parameters, "uploadRole", uploadRole);
            AddBoolParameter(parameters, "playlistRole", playlistRole);
            AddBoolParameter(parameters, "coverArtRole", coverArtRole);
            AddBoolParameter(parameters, "commentRole", commentRole);
            AddBoolParameter(parameters, "podcastRole", podcastRole);
            AddBoolParameter(parameters, "shareRole", shareRole);
            AddBoolParameter(parameters, "videoConversionRole", videoConversionRole);

            if (!string.IsNullOrEmpty(musicFolderIds))
            {
                parameters.Add("musicFolderId", musicFolderIds);
            }

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "createUser",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> UpdateUserAsync(
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
            string? musicFolderIds = null,
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

            AddBoolParameter(parameters, "ldapAuthenticated", ldapAuthenticated);
            AddBoolParameter(parameters, "adminRole", adminRole);
            AddBoolParameter(parameters, "settingsRole", settingsRole);
            AddBoolParameter(parameters, "streamRole", streamRole);
            AddBoolParameter(parameters, "jukeboxRole", jukeboxRole);
            AddBoolParameter(parameters, "downloadRole", downloadRole);
            AddBoolParameter(parameters, "uploadRole", uploadRole);
            AddBoolParameter(parameters, "playlistRole", playlistRole);
            AddBoolParameter(parameters, "coverArtRole", coverArtRole);
            AddBoolParameter(parameters, "commentRole", commentRole);
            AddBoolParameter(parameters, "podcastRole", podcastRole);
            AddBoolParameter(parameters, "shareRole", shareRole);
            AddBoolParameter(parameters, "videoConversionRole", videoConversionRole);

            if (!string.IsNullOrEmpty(musicFolderIds))
            {
                parameters.Add("musicFolderId", musicFolderIds);
            }

            if (maxBitRate.HasValue)
            {
                parameters.Add("maxBitRate", maxBitRate.Value.ToString());
            }

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "updateUser",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> DeleteUserAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(username));
            }

            var parameters = new Dictionary<string, string> { { "username", username } };

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "deleteUser",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> ChangePasswordAsync(
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

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "changePassword",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public async Task<Stream> GetAvatarAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(username));
            }

            var parameters = new Dictionary<string, string> { { "username", username } };

            return await this
                ._client.ExecuteBinaryRequestAsync("getAvatar", parameters, cancellationToken)
                .ConfigureAwait(false);
        }

        private static void AddBoolParameter(
            Dictionary<string, string> parameters,
            string name,
            bool? value
        )
        {
            if (value.HasValue)
            {
                parameters.Add(name, value.Value.ToString().ToLowerInvariant());
            }
        }
    }
}
