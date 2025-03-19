// <copyright file="SearchClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Interfaces;
using SubSonicMedia.Responses.Search;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for search-related Subsonic API methods.
    /// </summary>
    internal class SearchClient : ISearchClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public SearchClient(SubsonicClient client)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public Task<SearchResponse> SearchAsync(
            string query,
            int? artistCount = null,
            int? albumCount = null,
            int? songCount = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("Query cannot be null or empty", nameof(query));
            }

            var parameters = new Dictionary<string, string> { { "query", query } };

            if (artistCount.HasValue)
            {
                parameters.Add("artistCount", artistCount.Value.ToString());
            }

            if (albumCount.HasValue)
            {
                parameters.Add("albumCount", albumCount.Value.ToString());
            }

            if (songCount.HasValue)
            {
                parameters.Add("songCount", songCount.Value.ToString());
            }

            if (!string.IsNullOrEmpty(musicFolderId))
            {
                parameters.Add("musicFolderId", musicFolderId);
            }

            return this._client.ExecuteRequestAsync<SearchResponse>(
                "search",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<SearchResponse> Search2Async(
            string query,
            int? artistCount = null,
            int? artistOffset = null,
            int? albumCount = null,
            int? albumOffset = null,
            int? songCount = null,
            int? songOffset = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("Query cannot be null or empty", nameof(query));
            }

            var parameters = new Dictionary<string, string> { { "query", query } };

            if (artistCount.HasValue)
            {
                parameters.Add("artistCount", artistCount.Value.ToString());
            }

            if (artistOffset.HasValue)
            {
                parameters.Add("artistOffset", artistOffset.Value.ToString());
            }

            if (albumCount.HasValue)
            {
                parameters.Add("albumCount", albumCount.Value.ToString());
            }

            if (albumOffset.HasValue)
            {
                parameters.Add("albumOffset", albumOffset.Value.ToString());
            }

            if (songCount.HasValue)
            {
                parameters.Add("songCount", songCount.Value.ToString());
            }

            if (songOffset.HasValue)
            {
                parameters.Add("songOffset", songOffset.Value.ToString());
            }

            if (!string.IsNullOrEmpty(musicFolderId))
            {
                parameters.Add("musicFolderId", musicFolderId);
            }

            return this._client.ExecuteRequestAsync<SearchResponse>(
                "search2",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<Search3Response> Search3Async(
            string query,
            int? artistCount = null,
            int? artistOffset = null,
            int? albumCount = null,
            int? albumOffset = null,
            int? songCount = null,
            int? songOffset = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("Query cannot be null or empty", nameof(query));
            }

            var parameters = new Dictionary<string, string> { { "query", query } };

            if (artistCount.HasValue)
            {
                parameters.Add("artistCount", artistCount.Value.ToString());
            }

            if (artistOffset.HasValue)
            {
                parameters.Add("artistOffset", artistOffset.Value.ToString());
            }

            if (albumCount.HasValue)
            {
                parameters.Add("albumCount", albumCount.Value.ToString());
            }

            if (albumOffset.HasValue)
            {
                parameters.Add("albumOffset", albumOffset.Value.ToString());
            }

            if (songCount.HasValue)
            {
                parameters.Add("songCount", songCount.Value.ToString());
            }

            if (songOffset.HasValue)
            {
                parameters.Add("songOffset", songOffset.Value.ToString());
            }

            if (!string.IsNullOrEmpty(musicFolderId))
            {
                parameters.Add("musicFolderId", musicFolderId);
            }

            return this._client.ExecuteRequestAsync<Search3Response>(
                "search3",
                parameters,
                cancellationToken
            );
        }
    }
}
