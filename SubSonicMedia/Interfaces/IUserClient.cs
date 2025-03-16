// <copyright file="IUserClient.cs" company="Fabian Schmieder">
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
// along with SubSonicMedia. If not, see <https://www.gnu.org/licenses/>.
// </copyright>

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Responses.System;
using SubSonicMedia.Responses.User;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API user management methods.
    /// </summary>
    public interface IUserClient
    {
        /// <summary>
        /// Gets details about a particular user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing user details.</returns>
        Task<UserResponse> GetUserAsync(
            string username,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets details about all users.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing all user details.</returns>
        Task<UsersResponse> GetUsersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email address.</param>
        /// <param name="ldapAuthenticated">Whether the user is authenticated by LDAP.</param>
        /// <param name="adminRole">Whether the user has admin role.</param>
        /// <param name="settingsRole">Whether the user has settings role.</param>
        /// <param name="streamRole">Whether the user has stream role.</param>
        /// <param name="jukeboxRole">Whether the user has jukebox role.</param>
        /// <param name="downloadRole">Whether the user has download role.</param>
        /// <param name="uploadRole">Whether the user has upload role.</param>
        /// <param name="playlistRole">Whether the user has playlist role.</param>
        /// <param name="coverArtRole">Whether the user has coverArt role.</param>
        /// <param name="commentRole">Whether the user has comment role.</param>
        /// <param name="podcastRole">Whether the user has podcast role.</param>
        /// <param name="shareRole">Whether the user has share role.</param>
        /// <param name="videoConversionRole">Whether the user has videoConversion role.</param>
        /// <param name="musicFolderIds">Music folder IDs the user has access to.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> CreateUserAsync(
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
            string musicFolderIds = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email address.</param>
        /// <param name="ldapAuthenticated">Whether the user is authenticated by LDAP.</param>
        /// <param name="adminRole">Whether the user has admin role.</param>
        /// <param name="settingsRole">Whether the user has settings role.</param>
        /// <param name="streamRole">Whether the user has stream role.</param>
        /// <param name="jukeboxRole">Whether the user has jukebox role.</param>
        /// <param name="downloadRole">Whether the user has download role.</param>
        /// <param name="uploadRole">Whether the user has upload role.</param>
        /// <param name="playlistRole">Whether the user has playlist role.</param>
        /// <param name="coverArtRole">Whether the user has coverArt role.</param>
        /// <param name="commentRole">Whether the user has comment role.</param>
        /// <param name="podcastRole">Whether the user has podcast role.</param>
        /// <param name="shareRole">Whether the user has share role.</param>
        /// <param name="videoConversionRole">Whether the user has videoConversion role.</param>
        /// <param name="musicFolderIds">Music folder IDs the user has access to.</param>
        /// <param name="maxBitRate">The maximum bit rate allowed for the user.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> UpdateUserAsync(
            string username,
            string password = null,
            string email = null,
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
            string musicFolderIds = null,
            int? maxBitRate = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> DeleteUserAsync(
            string username,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Changes a user's password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The new password.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> ChangePasswordAsync(
            string username,
            string password,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets a user's avatar image.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>The avatar image as a stream.</returns>
        Task<Stream> GetAvatarAsync(string username, CancellationToken cancellationToken = default);
    }
}
