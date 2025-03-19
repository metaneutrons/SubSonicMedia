// <copyright file="AudioTrack.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Responses.Video.Models
{
    /// <summary>
    /// Represents an audio track in a video.
    /// </summary>
    public class AudioTrack
    {
        /// <summary>
        /// Gets or sets the audio track ID.
        /// </summary>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the audio track name.
        /// </summary>
        [XmlAttribute("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        [XmlAttribute("languageCode")]
        public string? LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the default audio track.
        /// </summary>
        [XmlAttribute("default")]
        public bool Default { get; set; }
    }
}
