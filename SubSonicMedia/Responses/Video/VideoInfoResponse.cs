// <copyright file="VideoInfoResponse.cs" company="Fabian Schmieder">
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

using System.Collections.Generic;
using System.Xml.Serialization;

namespace SubSonicMedia.Responses.Video
{
    /// <summary>
    /// Response containing video information.
    /// </summary>
    public class VideoInfoResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the video information.
        /// </summary>
        public VideoInfo VideoInfo { get; set; } = new VideoInfo();
    }

    /// <summary>
    /// Information about a video.
    /// </summary>
    public class VideoInfo
    {
        /// <summary>
        /// Gets or sets the video ID.
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the bit rate in kilobits per second.
        /// </summary>
        [XmlAttribute("bitRate")]
        public int Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds.
        /// </summary>
        [XmlAttribute("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the size in bytes.
        /// </summary>
        [XmlAttribute("size")]
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the width in pixels.
        /// </summary>
        [XmlAttribute("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height in pixels.
        /// </summary>
        [XmlAttribute("height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the video codec name.
        /// </summary>
        [XmlAttribute("videoCodec")]
        public string VideoCodec { get; set; }

        /// <summary>
        /// Gets or sets the audio codec name.
        /// </summary>
        [XmlAttribute("audioCodec")]
        public string AudioCodec { get; set; }

        /// <summary>
        /// Gets or sets the converted bit rate.
        /// </summary>
        [XmlAttribute("convertedBitRate")]
        public int? ConvertedBitrate { get; set; }

        /// <summary>
        /// Gets or sets the converted size.
        /// </summary>
        [XmlAttribute("convertedSize")]
        public long? ConvertedSize { get; set; }

        /// <summary>
        /// Gets or sets the list of audio tracks.
        /// </summary>
        [XmlElement("audioTrack")]
        public List<AudioTrack> AudioTrack { get; set; } = new List<AudioTrack>();

        /// <summary>
        /// Gets or sets the list of captions.
        /// </summary>
        [XmlElement("captions")]
        public List<Caption> Captions { get; set; } = new List<Caption>();

        /// <summary>
        /// Gets or sets the list of conversion formats.
        /// </summary>
        [XmlElement("conversion")]
        public List<Conversion> Conversion { get; set; } = new List<Conversion>();
    }

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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        [XmlAttribute("languageCode")]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the default audio track.
        /// </summary>
        [XmlAttribute("default")]
        public bool Default { get; set; }
    }

    /// <summary>
    /// Represents a caption/subtitle track.
    /// </summary>
    public class Caption
    {
        /// <summary>
        /// Gets or sets the caption ID.
        /// </summary>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the caption name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        [XmlAttribute("languageCode")]
        public string LanguageCode { get; set; }
    }

    /// <summary>
    /// Represents a video conversion format.
    /// </summary>
    public class Conversion
    {
        /// <summary>
        /// Gets or sets the conversion ID.
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the bit rate in kilobits per second.
        /// </summary>
        [XmlAttribute("bitRate")]
        public int Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the audio track ID to use in the conversion.
        /// </summary>
        [XmlAttribute("audioTrackId")]
        public string AudioTrackId { get; set; }
    }
}
