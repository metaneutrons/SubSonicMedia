// <copyright file="IPlaylistClient.cs" company="Fabian Schmieder">
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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Responses.Playlists;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API playlist methods.
    /// </summary>
    public interface IPlaylistClient
    {
        /// <summary>
        /// Gets all playlists.
        /// </summary>
        /// <param name="username">If specified, only return playlists for this user.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing all playlists.</returns>
        Task<PlaylistsResponse> GetPlaylistsAsync(
            string? username = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets a playlist by ID.
        /// </summary>
        /// <param name="id">The ID of the playlist to get.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing the playlist details.</returns>
        Task<PlaylistResponse> GetPlaylistAsync(
            string id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Creates a new playlist.
        /// </summary>
        /// <param name="name">The name of the playlist.</param>
        /// <param name="songIds">The IDs of songs to add to the playlist.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing the created playlist.</returns>
        Task<PlaylistResponse> CreatePlaylistAsync(
            string name,
            IEnumerable<string>? songIds = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Updates a playlist.
        /// </summary>
        /// <param name="id">The ID of the playlist to update.</param>
        /// <param name="name">The new name of the playlist.</param>
        /// <param name="comment">The new comment for the playlist.</param>
        /// <param name="isPublic">Whether the playlist should be public.</param>
        /// <param name="songIdsToAdd">The IDs of songs to add to the playlist.</param>
        /// <param name="songIndicesToRemove">The indexes of songs to remove from the playlist.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdatePlaylistAsync(
            string id,
            string? name = null,
            string? comment = null,
            bool? isPublic = null,
            IEnumerable<string>? songIdsToAdd = null,
            IEnumerable<int>? songIndicesToRemove = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes a playlist.
        /// </summary>
        /// <param name="id">The ID of the playlist to delete.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeletePlaylistAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the state of the play queue for this user.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing the current play queue.</returns>
        Task<PlayQueueResponse> GetPlayQueueAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves the state of the play queue for this user.
        /// </summary>
        /// <param name="ids">The ID of the songs in the play queue.</param>
        /// <param name="current">The ID of the current song.</param>
        /// <param name="position">The position in milliseconds.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing the updated play queue.</returns>
        Task<PlayQueueResponse> SavePlayQueueAsync(
            IEnumerable<string> ids,
            string? current = null,
            long? position = null,
            CancellationToken cancellationToken = default
        );
    }
}
