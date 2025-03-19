// <copyright file="PodcastChannel.cs" company="Fabian Schmieder">
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
namespace SubSonicMedia.Responses.Podcasts.Models
{
    /// <summary>
    /// A podcast channel.
    /// </summary>
    public class PodcastChannel
    {
        /// <summary>
        /// Gets or sets the channel ID.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the cover art ID.
        /// </summary>
        public string? CoverArt { get; set; }

        /// <summary>
        /// Gets or sets the original image URL.
        /// </summary>
        public string? OriginalImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the episode count.
        /// </summary>
        public int EpisodeCount { get; set; }

        /// <summary>
        /// Gets or sets the episodes list.
        /// </summary>
        public List<PodcastEpisode> Episode { get; set; } = new List<PodcastEpisode>();
    }
}
