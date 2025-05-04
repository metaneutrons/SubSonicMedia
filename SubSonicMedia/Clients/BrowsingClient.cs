// <copyright file="BrowsingClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Browsing;
using SubSonicMedia.Responses.Video;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for browsing-related Subsonic API methods.
    /// </summary>
    internal class BrowsingClient : IBrowsingClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsingClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public BrowsingClient(SubsonicClient client)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public Task<MusicFoldersResponse> GetMusicFoldersAsync(
            CancellationToken cancellationToken = default
        )
        {
            return this._client.ExecuteRequestAsync<MusicFoldersResponse>(
                "getMusicFolders",
                null,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<IndexesResponse> GetIndexesAsync(
            string? musicFolderId = null,
            DateTime? ifModifiedSince = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(musicFolderId))
            {
                parameters.Add("musicFolderId", musicFolderId);
            }

            if (ifModifiedSince.HasValue)
            {
                var timestamp = new DateTimeOffset(ifModifiedSince.Value).ToUnixTimeMilliseconds();
                parameters.Add("ifModifiedSince", timestamp.ToString());
            }

            return this._client.ExecuteRequestAsync<IndexesResponse>(
                "getIndexes",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<MusicDirectoryResponse> GetMusicDirectoryAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Directory ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<MusicDirectoryResponse>(
                "getMusicDirectory",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<GenresResponse> GetGenresAsync(CancellationToken cancellationToken = default)
        {
            return this._client.ExecuteRequestAsync<GenresResponse>(
                "getGenres",
                null,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<ArtistsResponse> GetArtistsAsync(
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(musicFolderId))
            {
                parameters.Add("musicFolderId", musicFolderId);
            }

            return this._client.ExecuteRequestAsync<ArtistsResponse>(
                "getArtists",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<ArtistResponse> GetArtistAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Artist ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<ArtistResponse>(
                "getArtist",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<AlbumResponse> GetAlbumAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Album ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<AlbumResponse>(
                "getAlbum",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<SongResponse> GetSongAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Song ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<SongResponse>(
                "getSong",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<VideosResponse> GetVideosAsync(CancellationToken cancellationToken = default)
        {
            return this._client.ExecuteRequestAsync<VideosResponse>(
                "getVideos",
                null,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<VideoInfoResponse> GetVideoInfoAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Video ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<VideoInfoResponse>(
                "getVideoInfo",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<AlbumList2Response> GetAlbumListAsync(
            AlbumListType type,
            int? size = null,
            int? offset = null,
            int? fromYear = null,
            int? toYear = null,
            string? genre = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>
            {
                { "type", type.ToString().ToLowerInvariant() },
            };

            if (size.HasValue)
            {
                parameters.Add("size", size.Value.ToString());
            }

            if (offset.HasValue)
            {
                parameters.Add("offset", offset.Value.ToString());
            }

            if (fromYear.HasValue)
            {
                parameters.Add("fromYear", fromYear.Value.ToString());
            }

            if (toYear.HasValue)
            {
                parameters.Add("toYear", toYear.Value.ToString());
            }

            if (!string.IsNullOrEmpty(genre))
            {
                parameters.Add("genre", genre);
            }

            if (!string.IsNullOrEmpty(musicFolderId))
            {
                parameters.Add("musicFolderId", musicFolderId);
            }

            return this._client.ExecuteRequestAsync<AlbumList2Response>(
                "getAlbumList2",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<RandomSongsResponse> GetRandomSongsAsync(
            int? size = null,
            string? genre = null,
            int? fromYear = null,
            int? toYear = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>();

            if (size.HasValue)
            {
                parameters.Add("size", size.Value.ToString());
            }

            if (!string.IsNullOrEmpty(genre))
            {
                parameters.Add("genre", genre);
            }

            if (fromYear.HasValue)
            {
                parameters.Add("fromYear", fromYear.Value.ToString());
            }

            if (toYear.HasValue)
            {
                parameters.Add("toYear", toYear.Value.ToString());
            }

            if (!string.IsNullOrEmpty(musicFolderId))
            {
                parameters.Add("musicFolderId", musicFolderId);
            }

            return this._client.ExecuteRequestAsync<RandomSongsResponse>(
                "getRandomSongs",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<SongsByGenreResponse> GetSongsByGenreAsync(
            string genre,
            int? count = null,
            int? offset = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(genre))
            {
                throw new ArgumentException("Genre cannot be null or empty", nameof(genre));
            }

            var parameters = new Dictionary<string, string> { { "genre", genre } };

            if (count.HasValue)
            {
                parameters.Add("count", count.Value.ToString());
            }

            if (offset.HasValue)
            {
                parameters.Add("offset", offset.Value.ToString());
            }

            if (!string.IsNullOrEmpty(musicFolderId))
            {
                parameters.Add("musicFolderId", musicFolderId);
            }

            return this._client.ExecuteRequestAsync<SongsByGenreResponse>(
                "getSongsByGenre",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<StarredResponse> GetStarredAsync(
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(musicFolderId))
            {
                parameters.Add("musicFolderId", musicFolderId);
            }

            return this._client.ExecuteRequestAsync<StarredResponse>(
                "getStarred",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<Starred2Response> GetStarred2Async(
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(musicFolderId))
            {
                parameters.Add("musicFolderId", musicFolderId);
            }

            return this._client.ExecuteRequestAsync<Starred2Response>(
                "getStarred2",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<ArtistInfoResponse> GetArtistInfoAsync(
            string id,
            int? count = null,
            bool? includeNotPresent = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Artist ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            if (count.HasValue)
            {
                parameters.Add("count", count.Value.ToString());
            }

            if (includeNotPresent.HasValue)
            {
                parameters.Add(
                    "includeNotPresent",
                    includeNotPresent.Value.ToString().ToLowerInvariant()
                );
            }

            return this._client.ExecuteRequestAsync<ArtistInfoResponse>(
                "getArtistInfo",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<ArtistInfo2Response> GetArtistInfo2Async(
            string id,
            int? count = null,
            bool? includeNotPresent = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Artist ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            if (count.HasValue)
            {
                parameters.Add("count", count.Value.ToString());
            }

            if (includeNotPresent.HasValue)
            {
                parameters.Add(
                    "includeNotPresent",
                    includeNotPresent.Value.ToString().ToLowerInvariant()
                );
            }

            return this._client.ExecuteRequestAsync<ArtistInfo2Response>(
                "getArtistInfo2",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<AlbumInfoResponse> GetAlbumInfoAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Album ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<AlbumInfoResponse>(
                "getAlbumInfo",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<AlbumInfo2Response> GetAlbumInfo2Async(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Album ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<AlbumInfo2Response>(
                "getAlbumInfo2",
                parameters,
                cancellationToken
            );
        }
    }
}
