// <copyright file="PlaylistClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Playlists;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for playlist-related Subsonic API methods.
    /// </summary>
    internal class PlaylistClient : IPlaylistClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public PlaylistClient(SubsonicClient client)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public Task<PlaylistsResponse> GetPlaylistsAsync(
            string? username = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(username))
            {
                parameters.Add("username", username);
            }

            return this._client.ExecuteRequestAsync<PlaylistsResponse>(
                "getPlaylists",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<PlaylistResponse> GetPlaylistAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Playlist ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<PlaylistResponse>(
                "getPlaylist",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<PlaylistResponse> CreatePlaylistAsync(
            string name,
            IEnumerable<string>? songIds = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Playlist name cannot be null or empty", nameof(name));
            }

            var parameters = new Dictionary<string, string> { { "name", name } };

            if (songIds != null)
            {
                var songIdList = songIds.ToList();
                if (songIdList.Any())
                {
                    parameters.Add("songId", string.Join(",", songIdList));
                }
            }

            return this._client.ExecuteRequestAsync<PlaylistResponse>(
                "createPlaylist",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task UpdatePlaylistAsync(
            string id,
            string? name = null,
            string? comment = null,
            bool? isPublic = null,
            IEnumerable<string>? songIdsToAdd = null,
            IEnumerable<int>? songIndicesToRemove = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Playlist ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "playlistId", id } };

            if (!string.IsNullOrEmpty(name))
            {
                parameters.Add("name", name);
            }

            if (!string.IsNullOrEmpty(comment))
            {
                parameters.Add("comment", comment);
            }

            if (isPublic.HasValue)
            {
                parameters.Add("public", isPublic.Value.ToString().ToLowerInvariant());
            }

            if (songIdsToAdd != null)
            {
                var songIdList = songIdsToAdd.ToList();
                if (songIdList.Any())
                {
                    parameters.Add("songIdToAdd", string.Join(",", songIdList));
                }
            }

            if (songIndicesToRemove != null)
            {
                var indexList = songIndicesToRemove.ToList();
                if (indexList.Any())
                {
                    parameters.Add("songIndexToRemove", string.Join(",", indexList));
                }
            }

            return this._client.ExecuteRequestAsync<Responses.SubsonicResponse>(
                "updatePlaylist",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task DeletePlaylistAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Playlist ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<Responses.SubsonicResponse>(
                "deletePlaylist",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<PlayQueueResponse> GetPlayQueueAsync(
            CancellationToken cancellationToken = default
        )
        {
            return this._client.ExecuteRequestAsync<PlayQueueResponse>(
                "getPlayQueue",
                null,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<PlayQueueResponse> SavePlayQueueAsync(
            IEnumerable<string> ids,
            string? current = null,
            long? position = null,
            CancellationToken cancellationToken = default
        )
        {
            if (ids == null || !ids.Any())
            {
                throw new ArgumentException("Song IDs cannot be null or empty", nameof(ids));
            }

            var parameters = new Dictionary<string, string> { { "id", string.Join(",", ids) } };

            if (!string.IsNullOrEmpty(current))
            {
                parameters.Add("current", current);
            }

            if (position.HasValue)
            {
                parameters.Add("position", position.Value.ToString());
            }

            return this._client.ExecuteRequestAsync<PlayQueueResponse>(
                "savePlayQueue",
                parameters,
                cancellationToken
            );
        }
    }
}
