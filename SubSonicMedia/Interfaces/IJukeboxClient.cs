// <copyright file="IJukeboxClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Jukebox;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API jukebox methods.
    /// </summary>
    public interface IJukeboxClient
    {
        /// <summary>
        /// Gets the current status of the jukebox.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing jukebox status.</returns>
        Task<JukeboxStatusResponse> GetJukeboxStatusAsync(
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Sets the gain of the jukebox.
        /// </summary>
        /// <param name="gain">The gain between 0.0 and 1.0.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing the updated jukebox status.</returns>
        Task<JukeboxControlResponse> SetJukeboxGainAsync(
            float gain,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Controls the jukebox player.
        /// </summary>
        /// <param name="action">The action to perform (start, stop, skip, etc.).</param>
        /// <param name="index">The playlist index, required for some actions.</param>
        /// <param name="offset">The offset in seconds, required for some actions.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing the updated jukebox status.</returns>
        Task<JukeboxControlResponse> ControlJukeboxAsync(
            string action,
            int? index = null,
            int? offset = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Adds songs to the jukebox playlist.
        /// </summary>
        /// <param name="ids">The IDs of the songs to add.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing the updated jukebox status.</returns>
        Task<JukeboxPlaylistResponse> AddToJukeboxAsync(
            IEnumerable<string> ids,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Removes songs from the jukebox playlist.
        /// </summary>
        /// <param name="indexes">The indexes of the songs to remove.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing the updated jukebox status.</returns>
        Task<JukeboxPlaylistResponse> RemoveFromJukeboxAsync(
            IEnumerable<int> indexes,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Sets the jukebox playlist.
        /// </summary>
        /// <param name="ids">The IDs of the songs to include in the playlist.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing the updated jukebox status.</returns>
        Task<JukeboxPlaylistResponse> SetJukeboxPlaylistAsync(
            IEnumerable<string> ids,
            CancellationToken cancellationToken = default
        );
    }
}
