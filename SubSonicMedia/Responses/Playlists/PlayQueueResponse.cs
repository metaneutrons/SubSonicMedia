// <copyright file="PlayQueueResponse.cs" company="Fabian Schmieder">
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

using System;
using System.Collections.Generic;
using SubSonicMedia.Responses.Browsing;

namespace SubSonicMedia.Responses.Playlists
{
    /// <summary>
    /// Response containing the play queue.
    /// </summary>
    public class PlayQueueResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the play queue.
        /// </summary>
        public PlayQueue PlayQueue { get; set; } = new PlayQueue();
    }

    /// <summary>
    /// Play queue information.
    /// </summary>
    public class PlayQueue
    {
        /// <summary>
        /// Gets or sets the current play queue version.
        /// </summary>
        public string Current { get; set; }

        /// <summary>
        /// Gets or sets the position in the play queue, in milliseconds.
        /// </summary>
        public long Position { get; set; }

        /// <summary>
        /// Gets or sets the username that saved the play queue.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the time when the play queue was saved, in milliseconds since 1970.
        /// </summary>
        public long Changed { get; set; }

        /// <summary>
        /// Gets or sets the songs in the play queue.
        /// </summary>
        public List<Child> Entry { get; set; } = new List<Child>();
    }
}
