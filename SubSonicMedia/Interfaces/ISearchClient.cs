// <copyright file="ISearchClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Search;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API search methods.
    /// </summary>
    public interface ISearchClient
    {
        /// <summary>
        /// Searches for artists, albums, and songs using simple search criteria.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="artistCount">Maximum number of artists to return.</param>
        /// <param name="albumCount">Maximum number of albums to return.</param>
        /// <param name="songCount">Maximum number of songs to return.</param>
        /// <param name="musicFolderId">Only return results from the music folder with this ID.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing search results.</returns>
        Task<SearchResponse> SearchAsync(
            string query,
            int? artistCount = null,
            int? albumCount = null,
            int? songCount = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Searches for artists, albums, and songs using advanced search criteria (ID3 tags).
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="artistCount">Maximum number of artists to return.</param>
        /// <param name="artistOffset">Starting offset for artists.</param>
        /// <param name="albumCount">Maximum number of albums to return.</param>
        /// <param name="albumOffset">Starting offset for albums.</param>
        /// <param name="songCount">Maximum number of songs to return.</param>
        /// <param name="songOffset">Starting offset for songs.</param>
        /// <param name="musicFolderId">Only return results from the music folder with this ID.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing search results.</returns>
        Task<Search2Response> Search2Async(
            string query,
            int? artistCount = null,
            int? artistOffset = null,
            int? albumCount = null,
            int? albumOffset = null,
            int? songCount = null,
            int? songOffset = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Searches for artists, albums, and songs using advanced search criteria (ID3 tags).
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="artistCount">Maximum number of artists to return.</param>
        /// <param name="artistOffset">Starting offset for artists.</param>
        /// <param name="albumCount">Maximum number of albums to return.</param>
        /// <param name="albumOffset">Starting offset for albums.</param>
        /// <param name="songCount">Maximum number of songs to return.</param>
        /// <param name="songOffset">Starting offset for songs.</param>
        /// <param name="musicFolderId">Only return results from the music folder with this ID.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing search results.</returns>
        Task<Search3Response> Search3Async(
            string query,
            int? artistCount = null,
            int? artistOffset = null,
            int? albumCount = null,
            int? albumOffset = null,
            int? songCount = null,
            int? songOffset = null,
            string? musicFolderId = null,
            CancellationToken cancellationToken = default
        );
    }
}
