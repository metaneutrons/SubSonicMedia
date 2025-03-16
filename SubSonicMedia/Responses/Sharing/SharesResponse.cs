// <copyright file="SharesResponse.cs" company="Fabian Schmieder">
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

using System;
using System.Collections.Generic;

namespace SubSonicMedia.Responses.Sharing
{
    /// <summary>
    /// Response containing all shares.
    /// </summary>
    public class SharesResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the shares container.
        /// </summary>
        public SharesContainer Shares { get; set; } = new SharesContainer();
    }

    /// <summary>
    /// Container for shares.
    /// </summary>
    public class SharesContainer
    {
        /// <summary>
        /// Gets or sets the list of shares.
        /// </summary>
        public List<Share> Share { get; set; } = new List<Share>();
    }

    /// <summary>
    /// Details of a share.
    /// </summary>
    public class Share
    {
        /// <summary>
        /// Gets or sets the share ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the URL to access the share.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the share description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who created the share.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the share was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the share expires.
        /// </summary>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the share was last visited.
        /// </summary>
        public DateTime? LastVisited { get; set; }

        /// <summary>
        /// Gets or sets the number of times the share has been visited.
        /// </summary>
        public int VisitCount { get; set; }

        /// <summary>
        /// Gets or sets the list of entries in the share.
        /// </summary>
        public List<ShareEntry> Entry { get; set; } = new List<ShareEntry>();
    }

    /// <summary>
    /// An entry in a share.
    /// </summary>
    public class ShareEntry
    {
        /// <summary>
        /// Gets or sets the entry ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the parent ID.
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this entry is a directory.
        /// </summary>
        public bool IsDir { get; set; }

        /// <summary>
        /// Gets or sets the entry title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the album name.
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// Gets or sets the artist name.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the track number.
        /// </summary>
        public int? Track { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the cover art ID.
        /// </summary>
        public string CoverArt { get; set; }

        /// <summary>
        /// Gets or sets the file size in bytes.
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the file suffix.
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds.
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// Gets or sets the bit rate in kilobits per second.
        /// </summary>
        public int? BitRate { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }
    }
}
