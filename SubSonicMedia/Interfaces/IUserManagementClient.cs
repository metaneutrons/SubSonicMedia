// <copyright file="IUserManagementClient.cs" company="Fabian Schmieder">
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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Responses.UserManagement;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API user management methods.
    /// </summary>
    public interface IUserManagementClient
    {
        /// <summary>
        /// Gets details about a user.
        /// </summary>
        /// <param name="username">The username to get details for.</param>
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
        /// <returns>A response containing all users.</returns>
        Task<UsersResponse> GetUsersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email address.</param>
        /// <param name="ldapAuthenticated">Whether the user should be authenticated against LDAP.</param>
        /// <param name="adminRole">Whether the user should have admin privileges.</param>
        /// <param name="settingsRole">Whether the user should have settings privileges.</param>
        /// <param name="streamRole">Whether the user should have streaming privileges.</param>
        /// <param name="jukeboxRole">Whether the user should have jukebox privileges.</param>
        /// <param name="downloadRole">Whether the user should have download privileges.</param>
        /// <param name="uploadRole">Whether the user should have upload privileges.</param>
        /// <param name="playlistRole">Whether the user should have playlist privileges.</param>
        /// <param name="coverArtRole">Whether the user should have cover art privileges.</param>
        /// <param name="commentRole">Whether the user should have commenting privileges.</param>
        /// <param name="podcastRole">Whether the user should have podcast privileges.</param>
        /// <param name="shareRole">Whether the user should have sharing privileges.</param>
        /// <param name="videoConversionRole">Whether the user should have video conversion privileges.</param>
        /// <param name="musicFolderIds">The IDs of the music folders the user should have access to.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateUserAsync(
            string username,
            string password,
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
            IEnumerable<string> musicFolderIds = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email address.</param>
        /// <param name="ldapAuthenticated">Whether the user should be authenticated against LDAP.</param>
        /// <param name="adminRole">Whether the user should have admin privileges.</param>
        /// <param name="settingsRole">Whether the user should have settings privileges.</param>
        /// <param name="streamRole">Whether the user should have streaming privileges.</param>
        /// <param name="jukeboxRole">Whether the user should have jukebox privileges.</param>
        /// <param name="downloadRole">Whether the user should have download privileges.</param>
        /// <param name="uploadRole">Whether the user should have upload privileges.</param>
        /// <param name="playlistRole">Whether the user should have playlist privileges.</param>
        /// <param name="coverArtRole">Whether the user should have cover art privileges.</param>
        /// <param name="commentRole">Whether the user should have commenting privileges.</param>
        /// <param name="podcastRole">Whether the user should have podcast privileges.</param>
        /// <param name="shareRole">Whether the user should have sharing privileges.</param>
        /// <param name="videoConversionRole">Whether the user should have video conversion privileges.</param>
        /// <param name="musicFolderIds">The IDs of the music folders the user should have access to.</param>
        /// <param name="maxBitRate">The maximum bit rate for the user in kilobits per second. If 0, no limit is imposed.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateUserAsync(
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
            IEnumerable<string> musicFolderIds = null,
            int? maxBitRate = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="username">The username to delete.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteUserAsync(string username, CancellationToken cancellationToken = default);

        /// <summary>
        /// Changes the password of an existing user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The new password.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ChangePasswordAsync(
            string username,
            string password,
            CancellationToken cancellationToken = default
        );
    }
}
