// <copyright file="IPodcastClient.cs" company="Fabian Schmieder">
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

using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Responses.Podcasts;
using SubSonicMedia.Responses.System;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API podcast methods.
    /// </summary>
    public interface IPodcastClient
    {
        /// <summary>
        /// Gets all podcasts.
        /// </summary>
        /// <param name="includeEpisodes">Whether to include episodes in the result.</param>
        /// <param name="id">Only return the podcast with this ID.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing podcasts.</returns>
        Task<PodcastsResponse> GetPodcastsAsync(
            bool? includeEpisodes = null,
            string id = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets the newest podcast episodes.
        /// </summary>
        /// <param name="count">The maximum number of episodes to return.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing the newest podcast episodes.</returns>
        Task<NewestPodcastsResponse> GetNewestPodcastsAsync(
            int? count = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Creates a new podcast channel.
        /// </summary>
        /// <param name="url">The URL of the podcast to add.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> CreatePodcastChannelAsync(
            string url,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes a podcast channel.
        /// </summary>
        /// <param name="id">The ID of the podcast channel to delete.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> DeletePodcastChannelAsync(
            string id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes a podcast episode.
        /// </summary>
        /// <param name="id">The ID of the podcast episode to delete.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> DeletePodcastEpisodeAsync(
            string id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Downloads a podcast episode.
        /// </summary>
        /// <param name="id">The ID of the podcast episode to download.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> DownloadPodcastEpisodeAsync(
            string id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Requests the server to check for new podcast episodes.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response indicating the status of the refresh operation.</returns>
        Task<RefreshPodcastsResponse> RefreshPodcastsAsync(
            CancellationToken cancellationToken = default
        );
    }
}
