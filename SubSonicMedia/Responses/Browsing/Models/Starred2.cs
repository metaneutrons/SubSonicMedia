// <copyright file="Starred2.cs" company="Fabian Schmieder">
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
    /// Container for starred items.
    /// </summary>
    public class Starred2
    {
        /// <summary>
        /// Gets or sets the list of starred artists.
        /// </summary>
        [XmlElement("artist")]
        public List<StarredArtist> Artists { get; set; } = new List<StarredArtist>();

        /// <summary>
        /// Gets or sets the list of starred albums.
        /// </summary>
        [XmlElement("album")]
        public List<StarredAlbum> Albums { get; set; } = new List<StarredAlbum>();

        /// <summary>
        /// Gets or sets the list of starred songs.
        /// </summary>
        [XmlElement("song")]
        public List<StarredSong> Songs { get; set; } = new List<StarredSong>();
    }
}
