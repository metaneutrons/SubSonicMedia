// <copyright file="PodcastsResponse.cs" company="Fabian Schmieder">
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

using System;
using System.Collections.Generic;

namespace SubSonicMedia.Responses.Podcasts
{
    /// <summary>
    /// Response containing all podcasts.
    /// </summary>
    public class PodcastsResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the podcasts container.
        /// </summary>
        public PodcastsContainer Podcasts { get; set; } = new PodcastsContainer();
    }

    /// <summary>
    /// Container for podcasts.
    /// </summary>
    public class PodcastsContainer
    {
        /// <summary>
        /// Gets or sets the list of channels.
        /// </summary>
        public List<PodcastChannel> Channel { get; set; } = new List<PodcastChannel>();
    }

    /// <summary>
    /// A podcast channel.
    /// </summary>
    public class PodcastChannel
    {
        /// <summary>
        /// Gets or sets the channel ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the cover art ID.
        /// </summary>
        public string CoverArt { get; set; }

        /// <summary>
        /// Gets or sets the original image URL.
        /// </summary>
        public string OriginalImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the episode count.
        /// </summary>
        public int EpisodeCount { get; set; }

        /// <summary>
        /// Gets or sets the episodes list.
        /// </summary>
        public List<PodcastEpisode> Episode { get; set; } = new List<PodcastEpisode>();
    }

    /// <summary>
    /// A podcast episode.
    /// </summary>
    public class PodcastEpisode
    {
        /// <summary>
        /// Gets or sets the episode ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the parent channel ID.
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the stream ID.
        /// </summary>
        public string StreamId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the parent channel title.
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a directory.
        /// </summary>
        public bool IsDir { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the cover art ID.
        /// </summary>
        public string CoverArt { get; set; }

        /// <summary>
        /// Gets or sets the file size in bytes.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the file suffix.
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the bit rate.
        /// </summary>
        public int BitRate { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }
    }
}
