// <copyright file="IBookmarkClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Bookmarks;
using SubSonicMedia.Responses.System;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API bookmark methods.
    /// </summary>
    public interface IBookmarkClient
    {
        /// <summary>
        /// Gets all bookmarks for the current user.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing all bookmarks.</returns>
        Task<BookmarksResponse> GetBookmarksAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates or updates a bookmark.
        /// </summary>
        /// <param name="id">The ID of the media file to bookmark.</param>
        /// <param name="position">The position in seconds.</param>
        /// <param name="comment">A user-defined comment.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> CreateBookmarkAsync(
            string id,
            long position,
            string comment = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes a bookmark.
        /// </summary>
        /// <param name="id">The ID of the media file to remove the bookmark from.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> DeleteBookmarkAsync(
            string id,
            CancellationToken cancellationToken = default
        );
    }
}
