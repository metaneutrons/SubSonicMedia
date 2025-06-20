// <copyright file="StarredAlbum.cs" company="Fabian Schmieder">
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
    /// Represents a starred album.
    /// </summary>
    public class StarredAlbum
    {
        /// <summary>
        /// Gets or sets the album ID.
        /// </summary>
        [XmlAttribute("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the album name.
        /// </summary>
        [XmlAttribute("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the artist name.
        /// </summary>
        [XmlAttribute("artist")]
        public string? Artist { get; set; }

        /// <summary>
        /// Gets or sets the artist ID.
        /// </summary>
        [XmlAttribute("artistId")]
        public string? ArtistId { get; set; }

        /// <summary>
        /// Gets or sets the cover art ID.
        /// </summary>
        [XmlAttribute("coverArt")]
        public string? CoverArt { get; set; }

        /// <summary>
        /// Gets or sets the song count.
        /// </summary>
        [XmlAttribute("songCount")]
        public int SongCount { get; set; }

        /// <summary>
        /// Gets or sets the creation date in milliseconds since epoch.
        /// </summary>
        [XmlAttribute("created")]
        [JsonPropertyName("created")]
        [JsonConverter(typeof(FlexibleDateTimeToLongConverter))]
        public long? Created { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds.
        /// </summary>
        [XmlAttribute("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the play count.
        /// </summary>
        [XmlAttribute("playCount")]
        public int? PlayCount { get; set; }

        /// <summary>
        /// Gets or sets the starred date in milliseconds since epoch.
        /// </summary>
        [XmlAttribute("starred")]
        public string? Starred { get; set; }

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
    }
}
