// <copyright file="Child.cs" company="Fabian Schmieder">
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
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using SubSonicMedia.Serialization.Converters;

namespace SubSonicMedia.Responses.Browsing.Models
{
    /// <summary>
    /// A child item in a music directory (file or subdirectory).
    /// </summary>
    public class Child
    {
        /// <summary>
        /// Gets or sets the item ID.
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the parent directory ID.
        /// </summary>
        [XmlAttribute("parent")]
        public string? Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is a directory.
        /// </summary>
        [XmlAttribute("isDir")]
        public bool IsDir { get; set; }

        /// <summary>
        /// Gets or sets the item title.
        /// </summary>
        [XmlAttribute("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the album name (for files).
        /// </summary>
        [XmlAttribute("album")]
        public string Album { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the artist name (for files).
        /// </summary>
        [XmlAttribute("artist")]
        public string Artist { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the album artist name (for files).
        /// </summary>
        [XmlAttribute("albumArtist")]
        public string AlbumArtist { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the track number (for files).
        /// </summary>
        [XmlAttribute("track")]
        public int? Track { get; set; }

        /// <summary>
        /// Gets or sets the year (for files).
        /// </summary>
        [XmlAttribute("year")]
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the genre (for files).
        /// </summary>
        [XmlAttribute("genre")]
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the cover art ID.
        /// </summary>
        [XmlAttribute("coverArt")]
        public string CoverArt { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the file size in bytes (for files).
        /// </summary>
        [XmlAttribute("size")]
        public long? Size { get; set; }

        /// <summary>
        /// Gets or sets the content type (for files).
        /// </summary>
        [XmlAttribute("contentType")]
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the file suffix (for files).
        /// </summary>
        [XmlAttribute("suffix")]
        public string Suffix { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the transcoded content type (for files).
        /// </summary>
        [XmlAttribute("transcodedContentType")]
        public string TranscodedContentType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the transcoded suffix (for files).
        /// </summary>
        [XmlAttribute("transcodedSuffix")]
        public string TranscodedSuffix { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the duration in seconds (for files).
        /// </summary>
        [XmlAttribute("duration")]
        public int? Duration { get; set; }

        /// <summary>
        /// Gets or sets the bit rate in kilobits per second (for files).
        /// </summary>
        [XmlAttribute("bitRate")]
        public int? BitRate { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        [XmlAttribute("path")]
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this is a video file.
        /// </summary>
        [XmlAttribute("isVideo")]
        public bool? IsVideo { get; set; }

        /// <summary>
        /// Gets or sets the play count.
        /// </summary>
        [XmlAttribute("playCount")]
        public int? PlayCount { get; set; }

        /// <summary>
        /// Gets or sets the created date in milliseconds since epoch.
        /// </summary>
        [XmlAttribute("created")]
        [JsonConverter(typeof(FlexibleDateTimeToLongConverter))]
        public long? Created { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is starred.
        /// </summary>
        [XmlAttribute("starred")]
        public bool Starred { get; set; }

        /// <summary>
        /// Gets or sets the album ID (for files).
        /// </summary>
        [XmlAttribute("albumId")]
        public string AlbumId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the artist ID (for files).
        /// </summary>
        [XmlAttribute("artistId")]
        public string ArtistId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the media type.
        /// </summary>
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the disc number (for files).
        /// </summary>
        [XmlAttribute("discNumber")]
        public int? DiscNumber { get; set; }

        /// <summary>
        /// Gets or sets the user's rating (1-5).
        /// </summary>
        [XmlAttribute("userRating")]
        public int? UserRating { get; set; }

        /// <summary>
        /// Gets or sets the average rating (1-5).
        /// </summary>
        [XmlAttribute("averageRating")]
        public double? AverageRating { get; set; }
    }
}
