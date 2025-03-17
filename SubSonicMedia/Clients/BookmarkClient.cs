// <copyright file="BookmarkClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Bookmarks;
using SubSonicMedia.Responses.System;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for accessing Subsonic API bookmark methods.
    /// </summary>
    internal class BookmarkClient : IBookmarkClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public BookmarkClient(SubsonicClient client)
        {
            this._client = client;
        }

        /// <inheritdoc/>
        public Task<BookmarksResponse> GetBookmarksAsync(
            CancellationToken cancellationToken = default
        )
        {
            return this._client.ExecuteRequestAsync<BookmarksResponse>(
                "getBookmarks",
                null,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> CreateBookmarkAsync(
            string id,
            long position,
            string comment = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>
            {
                { "id", id },
                { "position", position.ToString() },
            };

            if (!string.IsNullOrEmpty(comment))
            {
                parameters.Add("comment", comment);
            }

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "createBookmark",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> DeleteBookmarkAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "deleteBookmark",
                parameters,
                cancellationToken
            );
        }
    }
}
