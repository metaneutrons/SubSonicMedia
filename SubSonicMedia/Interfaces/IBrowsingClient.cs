// <copyright file="IBrowsingClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Browsing;
using SubSonicMedia.Responses.Video;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API browsing methods.
    /// </summary>
    public interface IBrowsingClient
    {
        /// <summary>
        /// Gets all configured music folders.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing music folders.</returns>
        Task<MusicFoldersResponse> GetMusicFoldersAsync(
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets an indexed structure of all artists.
        /// </summary>
        /// <param name="musicFolderId">The ID of the music folder to browse.</param>
        /// <param name="ifModifiedSince">Only return a result if the content has changed since this timestamp.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing indexes of artists.</returns>
        Task<IndexesResponse> GetIndexesAsync(
            string? musicFolderId = null,
            DateTime? ifModifiedSince = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets a listing of files in a music directory.
        /// </summary>
        /// <param name="id">The ID of the directory to list.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing directory contents.</returns>
        Task<MusicDirectoryResponse> GetMusicDirectoryAsync(
            string id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets all genres.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing all genres.</returns>
        Task<GenresResponse> GetGenresAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets random songs.
        /// </summary>
        /// <param name="size">The maximum number of songs to return.</param>
        /// <param name="genre">Only returns songs from the given genre.</param>
        /// <param name="fromYear">Only return songs published after or in this year.</param>
        /// <param name="toYear">Only return songs published before or in this year.</param>
        /// <param name="musicFolderId">Only return songs from the music folder with the given ID.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing random songs.</returns>
        Task<RandomSongsResponse> GetRandomSongsAsync(
            int? size = null,
            string? genre = null,
            int? fromYear = null,
            int? toYear = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets all artists, organized by ID3 tags.
        /// </summary>
        /// <param name="musicFolderId">The ID of the music folder to browse.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing all artists.</returns>
        Task<ArtistsResponse> GetArtistsAsync(
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets details for an artist, including a list of albums.
        /// </summary>
        /// <param name="id">The ID of the artist.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing artist details.</returns>
        Task<ArtistResponse> GetArtistAsync(
            string id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets details for an album, including a list of songs.
        /// </summary>
        /// <param name="id">The ID of the album.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing album details.</returns>
        Task<AlbumResponse> GetAlbumAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets details for a song.
        /// </summary>
        /// <param name="id">The ID of the song.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing song details.</returns>
        Task<SongResponse> GetSongAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns artist info with biography, image URLs and similar artists, using data from last.fm.
        /// </summary>
        /// <param name="id">The artist ID.</param>
        /// <param name="count">Max number of similar artists to return.</param>
        /// <param name="includeNotPresent">Whether to include artists that are not present in the media library.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing artist info.</returns>
        Task<ArtistInfoResponse> GetArtistInfoAsync(
            string id,
            int? count = null,
            bool? includeNotPresent = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Similar to getArtistInfo, but organizes music according to ID3 tags.
        /// </summary>
        /// <param name="id">The artist ID.</param>
        /// <param name="count">Max number of similar artists to return.</param>
        /// <param name="includeNotPresent">Whether to include artists that are not present in the media library.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing artist info (version 2).</returns>
        Task<ArtistInfo2Response> GetArtistInfo2Async(
            string id,
            int? count = null,
            bool? includeNotPresent = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Returns album notes, image URLs etc, using data from last.fm.
        /// </summary>
        /// <param name="id">The album ID.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing album info.</returns>
        Task<AlbumInfoResponse> GetAlbumInfoAsync(
            string id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Similar to getAlbumInfo, but organizes music according to ID3 tags.
        /// </summary>
        /// <param name="id">The album ID.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing album info (version 2).</returns>
        Task<AlbumInfo2Response> GetAlbumInfo2Async(
            string id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Returns songs by a given genre.
        /// </summary>
        /// <param name="genre">The genre name.</param>
        /// <param name="count">The maximum number of songs to return. Default 10.</param>
        /// <param name="offset">The offset. Used for paging. Default 0.</param>
        /// <param name="musicFolderId">Only return songs from the music folder with the given ID. Default all folders.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing songs by genre.</returns>
        Task<SongsByGenreResponse> GetSongsByGenreAsync(
            string genre,
            int? count = null,
            int? offset = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets all videos.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing all videos.</returns>
        Task<VideosResponse> GetVideosAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets details for a video.
        /// </summary>
        /// <param name="id">The ID of the video.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing video details.</returns>
        Task<VideoInfoResponse> GetVideoInfoAsync(
            string id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets a listing of albums using ID3 tag organization.
        /// </summary>
        /// <param name="type">The list type.</param>
        /// <param name="size">The maximum number of albums to return.</param>
        /// <param name="offset">The offset. Used for paging.</param>
        /// <param name="fromYear">Only return albums from this year or later.</param>
        /// <param name="toYear">Only return albums from this year or earlier.</param>
        /// <param name="genre">Only return albums of this genre.</param>
        /// <param name="musicFolderId">Only return albums from the music folder with the given ID.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing albums.</returns>
        Task<AlbumList2Response> GetAlbumListAsync(
            AlbumListType type,
            int? size = null,
            int? offset = null,
            int? fromYear = null,
            int? toYear = null,
            string? genre = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets starred items (songs, albums, artists).
        /// </summary>
        /// <param name="musicFolderId">Only return items from the music folder with the given ID.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing starred items.</returns>
        Task<StarredResponse> GetStarredAsync(
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Similar to getStarred, but organizes music according to ID3 tags.
        /// </summary>
        /// <param name="musicFolderId">Only return items from the music folder with the given ID.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing starred items (version 2).</returns>
        Task<Starred2Response> GetStarred2Async(
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        );
    }
}
