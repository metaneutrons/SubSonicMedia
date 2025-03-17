// <copyright file="BookmarksResponse.cs" company="Fabian Schmieder">
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

using System;
using System.Collections.Generic;
using SubSonicMedia.Responses.Search;

namespace SubSonicMedia.Responses.Bookmarks
{
    /// <summary>
    /// Response containing bookmarks.
    /// </summary>
    public class BookmarksResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the bookmarks container.
        /// </summary>
        public BookmarksContainer Bookmarks { get; set; } = new BookmarksContainer();
    }

    /// <summary>
    /// Container for bookmarks.
    /// </summary>
    public class BookmarksContainer
    {
        /// <summary>
        /// Gets or sets the list of bookmarks.
        /// </summary>
        public List<Bookmark> Bookmark { get; set; } = new List<Bookmark>();
    }

    /// <summary>
    /// A bookmark for a media file.
    /// </summary>
    public class Bookmark
    {
        /// <summary>
        /// Gets or sets the entry ID.
        /// </summary>
        public string EntryId { get; set; }

        /// <summary>
        /// Gets or sets the position in seconds.
        /// </summary>
        public long Position { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets when the bookmark was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets when the bookmark was last changed.
        /// </summary>
        public DateTime Changed { get; set; }

        /// <summary>
        /// Gets or sets the bookmarked entry.
        /// </summary>
        public Song Entry { get; set; } = new Song();
    }
}
