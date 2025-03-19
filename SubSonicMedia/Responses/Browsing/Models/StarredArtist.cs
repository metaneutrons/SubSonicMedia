// <copyright file="StarredArtist.cs" company="Fabian Schmieder">
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
    /// Represents a starred artist.
    /// </summary>
    public class StarredArtist
    {
        /// <summary>
        /// Gets or sets the artist ID.
        /// </summary>
        [XmlAttribute("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the artist name.
        /// </summary>
        [XmlAttribute("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the cover art ID.
        /// </summary>
        [XmlAttribute("coverArt")]
        public string? CoverArt { get; set; }

        /// <summary>
        /// Gets or sets the album count.
        /// </summary>
        [XmlAttribute("albumCount")]
        public int AlbumCount { get; set; }

        /// <summary>
        /// Gets or sets the starred date in milliseconds since epoch.
        /// </summary>
        [XmlAttribute("starred")]
        public string? Starred { get; set; }
    }
}
