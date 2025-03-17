// <copyright file="JukeboxStatusResponse.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Search;

namespace SubSonicMedia.Responses.Jukebox
{
    /// <summary>
    /// Response containing jukebox status.
    /// </summary>
    public class JukeboxStatusResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the jukebox status.
        /// </summary>
        public JukeboxStatus JukeboxStatus { get; set; } = new JukeboxStatus();
    }

    /// <summary>
    /// Status of the jukebox.
    /// </summary>
    public class JukeboxStatus
    {
        /// <summary>
        /// Gets or sets the current index in the playlist.
        /// </summary>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the jukebox is playing.
        /// </summary>
        public bool Playing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the jukebox is in gain mode.
        /// </summary>
        public bool Gain { get; set; }

        /// <summary>
        /// Gets or sets the position in seconds.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the playlist entries.
        /// </summary>
        public List<Song> Entry { get; set; } = new List<Song>();
    }
}
