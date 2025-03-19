// <copyright file="MusicDirectory.cs" company="Fabian Schmieder">
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
    /// A music directory in the media library.
    /// </summary>
    public class MusicDirectory
    {
        /// <summary>
        /// Gets or sets the directory ID.
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the parent directory ID.
        /// </summary>
        [XmlAttribute("parent")]
        public string? Parent { get; set; }

        /// <summary>
        /// Gets or sets the directory name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this directory is starred.
        /// </summary>
        [XmlAttribute("starred")]
        public bool Starred { get; set; }

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

        /// <summary>
        /// Gets or sets the play count.
        /// </summary>
        [XmlAttribute("playCount")]
        public int? PlayCount { get; set; }

        /// <summary>
        /// Gets or sets the child items (files and subdirectories).
        /// </summary>
        [XmlElement("child")]
        public List<Child> Children { get; set; } = new List<Child>();
    }
}
