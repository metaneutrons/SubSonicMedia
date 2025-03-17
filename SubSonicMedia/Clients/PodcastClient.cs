// <copyright file="PodcastClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Interfaces;
using SubSonicMedia.Responses.Podcasts;
using SubSonicMedia.Responses.System;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for accessing Subsonic API podcast methods.
    /// </summary>
    internal class PodcastClient : IPodcastClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public PodcastClient(SubsonicClient client)
        {
            this._client = client;
        }

        /// <inheritdoc/>
        public Task<PodcastsResponse> GetPodcastsAsync(
            bool? includeEpisodes = null,
            string id = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>();

            if (includeEpisodes.HasValue)
            {
                parameters.Add("includeEpisodes", includeEpisodes.Value.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(id))
            {
                parameters.Add("id", id);
            }

            return this._client.ExecuteRequestAsync<PodcastsResponse>(
                "getPodcasts",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<NewestPodcastsResponse> GetNewestPodcastsAsync(
            int? count = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>();

            if (count.HasValue)
            {
                parameters.Add("count", count.Value.ToString());
            }

            return this._client.ExecuteRequestAsync<NewestPodcastsResponse>(
                "getNewestPodcasts",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> CreatePodcastChannelAsync(
            string url,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "url", url } };

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "createPodcastChannel",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> DeletePodcastChannelAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "deletePodcastChannel",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> DeletePodcastEpisodeAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "deletePodcastEpisode",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> DownloadPodcastEpisodeAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "downloadPodcastEpisode",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<RefreshPodcastsResponse> RefreshPodcastsAsync(
            CancellationToken cancellationToken = default
        )
        {
            return this._client.ExecuteRequestAsync<RefreshPodcastsResponse>(
                "refreshPodcasts",
                null,
                cancellationToken
            );
        }
    }
}
