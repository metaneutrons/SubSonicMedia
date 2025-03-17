// <copyright file="VideosResponse.cs" company="Fabian Schmieder">
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
// along with SubSonicMedia. If not, see &lt;https://www.gnu.org/licenses/&gt;.
// </copyright>

using System.Collections.Generic;
using System.Xml.Serialization;

namespace SubSonicMedia.Responses.Video
{
    /// <summary>
    /// Response containing a list of available videos.
    /// </summary>
    public class VideosResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the videos element.
        /// </summary>
        public Videos Videos { get; set; } = new Videos();
    }

    /// <summary>
    /// Container for available videos.
    /// </summary>
    public class Videos
    {
        /// <summary>
        /// Gets or sets the list of videos.
        /// </summary>
        [XmlElement("video")]
        public List<Video> VideoList { get; set; } = new List<Video>();
    }

    /// <summary>
    /// Represents a video.
    /// </summary>
    public class Video
    {
        /// <summary>
        /// Gets or sets the video ID.
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the parent folder ID.
        /// </summary>
        [XmlAttribute("parent")]
        public string Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a directory.
        /// </summary>
        [XmlAttribute("isDir")]
        public bool IsDir { get; set; }

        /// <summary>
        /// Gets or sets the video title.
        /// </summary>
        [XmlAttribute("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the video album.
        /// </summary>
        [XmlAttribute("album")]
        public string Album { get; set; }

        /// <summary>
        /// Gets or sets the video artist.
        /// </summary>
        [XmlAttribute("artist")]
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the cover art ID.
        /// </summary>
        [XmlAttribute("coverArt")]
        public string CoverArt { get; set; }

        /// <summary>
        /// Gets or sets the video size in bytes.
        /// </summary>
        [XmlAttribute("size")]
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        [XmlAttribute("contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the file suffix.
        /// </summary>
        [XmlAttribute("suffix")]
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets the transcoded content type.
        /// </summary>
        [XmlAttribute("transcodedContentType")]
        public string TranscodedContentType { get; set; }

        /// <summary>
        /// Gets or sets the transcoded suffix.
        /// </summary>
        [XmlAttribute("transcodedSuffix")]
        public string TranscodedSuffix { get; set; }

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
        /// Gets or sets the video width in pixels.
        /// </summary>
        [XmlAttribute("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the video height in pixels.
        /// </summary>
        [XmlAttribute("height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        [XmlAttribute("path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the video is playable.
        /// </summary>
        [XmlAttribute("isVideo")]
        public bool IsVideo { get; set; } = true;

        /// <summary>
        /// Gets or sets the video play count.
        /// </summary>
        [XmlAttribute("playCount")]
        public int? PlayCount { get; set; }

        /// <summary>
        /// Gets or sets the created date in milliseconds since epoch.
        /// </summary>
        [XmlAttribute("created")]
        public long? Created { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this video is starred.
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
    }
}
