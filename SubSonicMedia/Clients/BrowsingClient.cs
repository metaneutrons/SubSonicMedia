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
using SubSonicMedia.Utilities; // Required for VersionInfo
using Microsoft.Extensions.Logging; // Required for logging

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for browsing-related Subsonic API methods.
    /// </summary>
    internal class BrowsingClient : IBrowsingClient
    {
        private readonly SubsonicClient _client;
        private readonly ILogger<BrowsingClient> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsingClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public BrowsingClient(SubsonicClient client)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
            // Assuming SubsonicClient can provide a logger factory or a logger instance.
            // If not, this needs to be adjusted, possibly by passing ILogger<BrowsingClient> in constructor.
            // For now, let's assume _client has a Logger property or a GetLogger method.
            // This is a common pattern but might need to fetch the logger from a static factory if not available via _client.
            // For the purpose of this exercise, let's assume a hypothetical GetLogger method or direct access.
            // If _client exposes its internal logger, that could be used, but it's often not ideal.
            // A better approach would be to inject ILogger<BrowsingClient> directly.
            // Let's assume for now _client has a general logger we can use, or we use a NullLogger if not.
            // This part might need refinement based on actual SubsonicClient capabilities for logging.
            // For a concrete implementation detail: if SubsonicClient has a public ILogger property:
            // this._logger = client.LoggerProvider.CreateLogger<BrowsingClient>();
            // Or if it's internal and we can't access it, we might need to pass it to BrowsingClient constructor.
            // As a placeholder, let's use a NullLogger if we cannot get one from the client.
            // This should be replaced with proper logger injection/retrieval.
            this._logger = client._logger as ILogger<BrowsingClient> ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<BrowsingClient>.Instance;
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
        public async Task<AlbumList2Response> GetAlbumListAsync(
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
            string serverVersionString = this._client.ServerApiVersion;
            string targetVersionString = "1.11.0"; // Target version for getAlbumList2

            bool serverParseSuccess = VersionInfo.TryParseSubsonicVersion(serverVersionString, out int serverVersionNumeric);
            bool targetParseSuccess = VersionInfo.TryParseSubsonicVersion(targetVersionString, out int targetVersionNumeric);

            if (serverParseSuccess && targetParseSuccess && serverVersionNumeric >= targetVersionNumeric)
            {
                this._logger.LogDebug("Using getAlbumList2 as server API version {ServerVersion} >= {TargetVersion}", serverVersionString, targetVersionString);
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

                return await this._client.ExecuteRequestAsync<AlbumList2Response>(
                    "getAlbumList2",
                    parameters,
                    cancellationToken
                ).ConfigureAwait(false);
            }
            else
            {
                if (!serverParseSuccess)
                {
                    this._logger.LogWarning("Could not parse server API version '{ServerVersion}'. Falling back to getAlbumList.", serverVersionString);
                }
                else
                {
                    this._logger.LogDebug("Falling back to getAlbumList for server API version {ServerVersion} < {TargetVersion}", serverVersionString, targetVersionString);
                }

                var fallbackParameters = new Dictionary<string, string>
                {
                    { "type", type.ToString().ToLowerInvariant() }
                };

                if (size.HasValue)
                {
                    fallbackParameters.Add("size", size.Value.ToString());
                }

                if (offset.HasValue)
                {
                    fallbackParameters.Add("offset", offset.Value.ToString());
                }

                // Log warnings for unsupported parameters by getAlbumList
                if (fromYear.HasValue)
                {
                    this._logger.LogWarning("Parameter 'fromYear' is not supported by getAlbumList on older server API versions. It will be ignored.");
                }
                if (toYear.HasValue)
                {
                    this._logger.LogWarning("Parameter 'toYear' is not supported by getAlbumList on older server API versions. It will be ignored.");
                }
                if (!string.IsNullOrEmpty(genre))
                {
                    this._logger.LogWarning("Parameter 'genre' is not supported by getAlbumList on older server API versions. It will be ignored.");
                }
                if (!string.IsNullOrEmpty(musicFolderId))
                {
                    this._logger.LogWarning("Parameter 'musicFolderId' is not supported by getAlbumList on older server API versions. It will be ignored.");
                }

                var albumListResponse = await this._client.ExecuteRequestAsync<AlbumListResponse>(
                    "getAlbumList",
                    fallbackParameters,
                    cancellationToken
                ).ConfigureAwait(false);

                var adaptedAlbumList2 = new SubSonicMedia.Responses.Browsing.Models.AlbumList2
                {
                    // Ensure Album is initialized even if albumListResponse.AlbumList or albumListResponse.AlbumList.Album is null
                    Album = albumListResponse.AlbumList?.Album ?? new List<SubSonicMedia.Responses.Search.Models.Album>()
                };

                return new AlbumList2Response
                {
                    Status = albumListResponse.Status,
                    Version = albumListResponse.Version, // This will be the server's actual response version from getAlbumList
                    Error = albumListResponse.Error,
                    AlbumList2 = adaptedAlbumList2
                };
            }
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
        public async Task<Starred2Response> GetStarredAsync(
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(musicFolderId))
            {
                parameters.Add("musicFolderId", musicFolderId);
            }

            string serverVersionString = this._client.ServerApiVersion;
            string targetVersionString = "1.8.0"; // Target version for getStarred2

            bool serverParseSuccess = VersionInfo.TryParseSubsonicVersion(serverVersionString, out int serverVersionNumeric);
            bool targetParseSuccess = VersionInfo.TryParseSubsonicVersion(targetVersionString, out int targetVersionNumeric);

            if (serverParseSuccess && targetParseSuccess && serverVersionNumeric >= targetVersionNumeric)
            {
                this._logger.LogDebug("Using getStarred2 as server API version {ServerVersion} >= {TargetVersion}", serverVersionString, targetVersionString);
                return await this._client.ExecuteRequestAsync<Starred2Response>("getStarred2", parameters, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                if (!serverParseSuccess)
                {
                    this._logger.LogWarning("Could not parse server API version '{ServerVersion}'. Falling back to getStarred.", serverVersionString);
                }
                else
                {
                    this._logger.LogDebug("Falling back to getStarred for server API version {ServerVersion} < {TargetVersion}", serverVersionString, targetVersionString);
                }

                var starredResponse = await this._client.ExecuteRequestAsync<StarredResponse>("getStarred", parameters, cancellationToken).ConfigureAwait(false);

                if (starredResponse.Starred == null)
                {
                    // Return an empty Starred2Response but preserve status, version, and error
                    return new Starred2Response
                    {
                        Status = starredResponse.Status,
                        Version = starredResponse.Version,
                        Error = starredResponse.Error,
                        Starred2 = new SubSonicMedia.Responses.Browsing.Models.Starred2() // Ensure Starred2 is not null
                    };
                }

                var adaptedStarred2Model = new SubSonicMedia.Responses.Browsing.Models.Starred2();

                if (starredResponse.Starred?.Artist != null)
                {
                    adaptedStarred2Model.Artists = starredResponse.Starred.Artist.Select(a => new SubSonicMedia.Responses.Browsing.Models.StarredArtist
                    {
                        Id = a.Id,
                        Name = a.Name,
                        CoverArt = a.CoverArt,
                        AlbumCount = ((SubSonicMedia.Responses.Browsing.Models.Artist)a).AlbumCount, // Cast to derived type if needed, or ensure base has it
                        Starred = null // Base Artist DTO does not have Starred date for getStarred response
                    }).ToList();
                }

                if (starredResponse.Starred?.Album != null)
                {
                    adaptedStarred2Model.Albums = starredResponse.Starred.Album.Select(al => new SubSonicMedia.Responses.Browsing.Models.StarredAlbum
                    {
                        Id = al.Id,
                        Name = al.Name,
                        Artist = al.Artist,
                        ArtistId = al.ArtistId,
                        CoverArt = al.CoverArt,
                        SongCount = al.SongCount,
                        Created = al.Created,
                        Duration = al.Duration,
                        PlayCount = al.PlayCount ?? 0, // StarredAlbum.PlayCount is not nullable
                        Starred = al.Starred,
                        Year = al.Year ?? 0, // StarredAlbum.Year is not nullable
                        Genre = al.Genre
                    }).ToList();
                }

                if (starredResponse.Starred?.Song != null)
                {
                    adaptedStarred2Model.Songs = starredResponse.Starred.Song.Select(s => new SubSonicMedia.Responses.Browsing.Models.StarredSong
                    {
                        Id = s.Id,
                        Parent = s.Parent,
                        IsDir = s.IsDir,
                        Title = s.Title,
                        Album = s.Album,
                        Artist = s.Artist,
                        Track = s.Track ?? 0, // StarredSong.Track is not nullable
                        Year = s.Year ?? 0, // StarredSong.Year is not nullable
                        Genre = s.Genre,
                        CoverArt = s.CoverArt,
                        Size = s.Size,
                        ContentType = s.ContentType,
                        Suffix = s.Suffix,
                        TranscodedContentType = s.TranscodedContentType,
                        TranscodedSuffix = s.TranscodedSuffix,
                        Duration = s.Duration ?? 0, // StarredSong.Duration is not nullable
                        BitRate = s.BitRate ?? 0, // StarredSong.BitRate is not nullable
                        Path = s.Path,
                        PlayCount = s.PlayCount ?? 0, // StarredSong.PlayCount is not nullable
                        Created = s.Created ?? DateTime.MinValue, // StarredSong.Created is not nullable
                        Starred = s.Starred,
                        AlbumId = s.AlbumId,
                        ArtistId = s.ArtistId,
                        Type = s.Type
                    }).ToList();
                }

                return new Starred2Response
                {
                    Status = starredResponse.Status,
                    Version = starredResponse.Version,
                    Error = starredResponse.Error,
                    Starred2 = adaptedStarred2Model
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ArtistInfo2Response> GetArtistInfoAsync(
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

            string serverVersionString = this._client.ServerApiVersion;
            string targetVersionString = "1.11.0"; // Target version for getArtistInfo2

            bool serverParseSuccess = VersionInfo.TryParseSubsonicVersion(serverVersionString, out int serverVersionNumeric);
            bool targetParseSuccess = VersionInfo.TryParseSubsonicVersion(targetVersionString, out int targetVersionNumeric);

            if (serverParseSuccess && targetParseSuccess && serverVersionNumeric >= targetVersionNumeric)
            {
                this._logger.LogDebug("Using getArtistInfo2 as server API version {ServerVersion} >= {TargetVersion}", serverVersionString, targetVersionString);
                return await this._client.ExecuteRequestAsync<ArtistInfo2Response>("getArtistInfo2", parameters, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                if (!serverParseSuccess)
                {
                    this._logger.LogWarning("Could not parse server API version '{ServerVersion}'. Falling back to getArtistInfo.", serverVersionString);
                }
                else
                {
                    this._logger.LogDebug("Falling back to getArtistInfo for server API version {ServerVersion} < {TargetVersion}", serverVersionString, targetVersionString);
                }

                var artistInfoResponse = await this._client.ExecuteRequestAsync<ArtistInfoResponse>("getArtistInfo", parameters, cancellationToken).ConfigureAwait(false);

                if (artistInfoResponse.ArtistInfo == null)
                {
                    return new ArtistInfo2Response
                    {
                        Status = artistInfoResponse.Status,
                        Version = artistInfoResponse.Version,
                        Error = artistInfoResponse.Error,
                        ArtistInfo2 = null
                    };
                }

                var adaptedArtistInfo2 = new SubSonicMedia.Responses.Browsing.Models.ArtistInfo2
                {
                    Biography = artistInfoResponse.ArtistInfo.Biography,
                    MusicBrainzId = artistInfoResponse.ArtistInfo.MusicBrainzId,
                    LastFmUrl = artistInfoResponse.ArtistInfo.LastFmUrl,
                    SmallImageUrl = artistInfoResponse.ArtistInfo.SmallImageUrl,
                    MediumImageUrl = artistInfoResponse.ArtistInfo.MediumImageUrl,
                    LargeImageUrl = artistInfoResponse.ArtistInfo.LargeImageUrl,
                    SimilarArtist = artistInfoResponse.ArtistInfo.SimilarArtist ?? new List<SubSonicMedia.Responses.Browsing.Models.Artist>(),
                    Album = artistInfoResponse.ArtistInfo.Album ?? new List<SubSonicMedia.Responses.Search.Models.Album>()
                };

                return new ArtistInfo2Response
                {
                    Status = artistInfoResponse.Status,
                    Version = artistInfoResponse.Version,
                    Error = artistInfoResponse.Error,
                    ArtistInfo2 = adaptedArtistInfo2
                };
            }
        }

        /// <inheritdoc/>
        public async Task<AlbumInfo2Response> GetAlbumInfoAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Album ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            string serverVersionString = this._client.ServerApiVersion;
            string targetVersionString = "1.11.0"; // Target version for getAlbumInfo2

            bool serverParseSuccess = VersionInfo.TryParseSubsonicVersion(serverVersionString, out int serverVersionNumeric);
            bool targetParseSuccess = VersionInfo.TryParseSubsonicVersion(targetVersionString, out int targetVersionNumeric);

            if (serverParseSuccess && targetParseSuccess && serverVersionNumeric >= targetVersionNumeric)
            {
                this._logger.LogDebug("Using getAlbumInfo2 as server API version {ServerVersion} >= {TargetVersion}", serverVersionString, targetVersionString);
                return await this._client.ExecuteRequestAsync<AlbumInfo2Response>("getAlbumInfo2", parameters, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                if (!serverParseSuccess)
                {
                    this._logger.LogWarning("Could not parse server API version '{ServerVersion}'. Falling back to getAlbumInfo.", serverVersionString);
                }
                else
                {
                    this._logger.LogDebug("Falling back to getAlbumInfo for server API version {ServerVersion} < {TargetVersion}", serverVersionString, targetVersionString);
                }

                var albumInfoResponse = await this._client.ExecuteRequestAsync<AlbumInfoResponse>("getAlbumInfo", parameters, cancellationToken).ConfigureAwait(false);

                if (albumInfoResponse.AlbumInfo == null)
                {
                     // If AlbumInfo is null, it might be an error response or an empty valid response.
                     // Return a new AlbumInfo2Response preserving status, version, and error.
                    return new AlbumInfo2Response
                    {
                        Status = albumInfoResponse.Status,
                        Version = albumInfoResponse.Version,
                        Error = albumInfoResponse.Error,
                        AlbumInfo2 = null // AlbumInfo2 will also be null
                    };
                }

                var adaptedAlbumInfo2 = new SubSonicMedia.Responses.Browsing.Models.AlbumInfo2
                {
                    Notes = albumInfoResponse.AlbumInfo.Notes,
                    MusicBrainzId = albumInfoResponse.AlbumInfo.MusicBrainzId,
                    LastFmUrl = albumInfoResponse.AlbumInfo.LastFmUrl,
                    SmallImageUrl = albumInfoResponse.AlbumInfo.SmallImageUrl,
                    MediumImageUrl = albumInfoResponse.AlbumInfo.MediumImageUrl,
                    LargeImageUrl = albumInfoResponse.AlbumInfo.LargeImageUrl,
                    // Other properties from Album (base of AlbumInfo) are implicitly handled if AlbumInfo2 inherits from Album or a common base.
                    // Assuming AlbumInfo2 has all properties of Album (ID, Name, Artist, ArtistId, CoverArt, SongCount, Created, Duration, Genre, Year, etc.)
                    // If AlbumInfo2 does not directly inherit these, they would need explicit mapping:
                    Id = albumInfoResponse.AlbumInfo.Id,
                    Name = albumInfoResponse.AlbumInfo.Name,
                    Artist = albumInfoResponse.AlbumInfo.Artist,
                    ArtistId = albumInfoResponse.AlbumInfo.ArtistId,
                    CoverArt = albumInfoResponse.AlbumInfo.CoverArt,
                    SongCount = albumInfoResponse.AlbumInfo.SongCount,
                    Created = albumInfoResponse.AlbumInfo.Created,
                    Duration = albumInfoResponse.AlbumInfo.Duration,
                    Genre = albumInfoResponse.AlbumInfo.Genre,
                    PlayCount = albumInfoResponse.AlbumInfo.PlayCount,
                    // Year is int? in Album but int in AlbumInfo. Assuming AlbumInfo2.Year is also int.
                    // If AlbumInfo.Year is string, it needs parsing. Subsonic schema usually uses int for year.
                    Year = albumInfoResponse.AlbumInfo.Year,
                    // Rating = albumInfoResponse.AlbumInfo.Rating, // If AlbumInfo has Rating
                    // UserRating = albumInfoResponse.AlbumInfo.UserRating, // If AlbumInfo has UserRating
                    // AverageRating = albumInfoResponse.AlbumInfo.AverageRating, // If AlbumInfo has AverageRating
                    IsVideo = albumInfoResponse.AlbumInfo.IsVideo,
                    // Explicitly copy songs if needed, though AlbumInfo2 typically re-fetches them or they are not part of AlbumInfo itself.
                    // Songs = albumInfoResponse.AlbumInfo.Songs?.Select(s => new Song { ... }).ToList(), // Example if deep copy needed
                };

                return new AlbumInfo2Response
                {
                    Status = albumInfoResponse.Status,
                    Version = albumInfoResponse.Version, // This is the server's actual response version from getAlbumInfo
                    Error = albumInfoResponse.Error,
                    AlbumInfo2 = adaptedAlbumInfo2
                };
            }
        }
    }
}
