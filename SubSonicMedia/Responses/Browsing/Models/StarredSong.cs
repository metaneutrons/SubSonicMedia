// <copyright file="StarredSong.cs" company="Fabian Schmieder">
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
using System.Xml.Serialization;

namespace SubSonicMedia.Responses.Browsing.Models
{
    /// <summary>
    /// Represents a starred song.
    /// </summary>
    public class StarredSong
    {
        /// <summary>
        /// Gets or sets the song ID.
        /// </summary>
        [XmlAttribute("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the parent folder ID.
        /// </summary>
        [XmlAttribute("parent")]
        public string? Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a directory.
        /// </summary>
        [XmlAttribute("isDir")]
        public bool IsDir { get; set; }

        /// <summary>
        /// Gets or sets the song title.
        /// </summary>
        [XmlAttribute("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the album name.
        /// </summary>
        [XmlAttribute("album")]
        public string? Album { get; set; }

        /// <summary>
        /// Gets or sets the artist name.
        /// </summary>
        [XmlAttribute("artist")]
        public string? Artist { get; set; }

        /// <summary>
        /// Gets or sets the track number.
        /// </summary>
        [XmlAttribute("track")]
        public int? Track { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        [XmlAttribute("year")]
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        [XmlAttribute("genre")]
        public string? Genre { get; set; }

        /// <summary>
        /// Gets or sets the cover art ID.
        /// </summary>
        [XmlAttribute("coverArt")]
        public string? CoverArt { get; set; }

        /// <summary>
        /// Gets or sets the file size in bytes.
        /// </summary>
        [XmlAttribute("size")]
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        [XmlAttribute("contentType")]
        public string? ContentType { get; set; }

        /// <summary>
        /// Gets or sets the file suffix.
        /// </summary>
        [XmlAttribute("suffix")]
        public string? Suffix { get; set; }

        /// <summary>
        /// Gets or sets the transcoded content type.
        /// </summary>
        [XmlAttribute("transcodedContentType")]
        public string? TranscodedContentType { get; set; }

        /// <summary>
        /// Gets or sets the transcoded suffix.
        /// </summary>
        [XmlAttribute("transcodedSuffix")]
        public string? TranscodedSuffix { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds.
        /// </summary>
        [XmlAttribute("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the bit rate in kilobits per second.
        /// </summary>
        [XmlAttribute("bitRate")]
        public int BitRate { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        [XmlAttribute("path")]
        public string? Path { get; set; }

        /// <summary>
        /// Gets or sets the play count.
        /// </summary>
        [XmlAttribute("playCount")]
        public int? PlayCount { get; set; }

        /// <summary>
        /// Gets or sets the creation date in milliseconds since epoch.
        /// </summary>
        [XmlAttribute("created")]
        public long Created { get; set; }

        /// <summary>
        /// Gets or sets the starred date in milliseconds since epoch.
        /// </summary>
        [XmlAttribute("starred")]
        public string? Starred { get; set; }

        /// <summary>
        /// Gets or sets the album ID.
        /// </summary>
        [XmlAttribute("albumId")]
        public string? AlbumId { get; set; }

        /// <summary>
        /// Gets or sets the artist ID.
        /// </summary>
        [XmlAttribute("artistId")]
        public string? ArtistId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public string? Type { get; set; }
    }
}
