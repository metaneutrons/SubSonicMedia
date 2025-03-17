// <copyright file="NewestPodcastsResponse.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Responses.Podcasts
{
    /// <summary>
    /// Response containing newest podcast episodes.
    /// </summary>
    public class NewestPodcastsResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the newest podcasts container.
        /// </summary>
        public NewestPodcasts NewestPodcasts { get; set; } = new NewestPodcasts();
    }

    /// <summary>
    /// Container for newest podcast episodes.
    /// </summary>
    public class NewestPodcasts
    {
        /// <summary>
        /// Gets or sets the list of podcast episodes.
        /// </summary>
        public List<PodcastEpisode> Episode { get; set; } = new List<PodcastEpisode>();
    }
}
