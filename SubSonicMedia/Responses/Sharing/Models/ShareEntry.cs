// <copyright file="ShareEntry.cs" company="Fabian Schmieder">
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
namespace SubSonicMedia.Responses.Sharing.Models
{
    /// <summary>
    /// An entry in a share.
    /// </summary>
    public class ShareEntry
    {
        /// <summary>
        /// Gets or sets the entry ID.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the parent ID.
        /// </summary>
        public string? Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this entry is a directory.
        /// </summary>
        public bool IsDir { get; set; }

        /// <summary>
        /// Gets or sets the entry title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the album name.
        /// </summary>
        public string? Album { get; set; }

        /// <summary>
        /// Gets or sets the artist name.
        /// </summary>
        public string? Artist { get; set; }

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
        public string? Genre { get; set; }

        /// <summary>
        /// Gets or sets the cover art ID.
        /// </summary>
        public string? CoverArt { get; set; }

        /// <summary>
        /// Gets or sets the file size in bytes.
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// Gets or sets the file suffix.
        /// </summary>
        public string? Suffix { get; set; }

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
        public string? Path { get; set; }
    }
}
