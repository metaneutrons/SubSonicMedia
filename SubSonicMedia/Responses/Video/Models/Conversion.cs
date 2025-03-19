// <copyright file="Conversion.cs" company="Fabian Schmieder">
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
    /// Represents a video conversion format.
    /// </summary>
    public class Conversion
    {
        /// <summary>
        /// Gets or sets the conversion ID.
        /// </summary>
        [XmlAttribute("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the bit rate in kilobits per second.
        /// </summary>
        [XmlAttribute("bitRate")]
        public int Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the audio track ID to use in the conversion.
        /// </summary>
        [XmlAttribute("audioTrackId")]
        public string? AudioTrackId { get; set; }
    }
}
