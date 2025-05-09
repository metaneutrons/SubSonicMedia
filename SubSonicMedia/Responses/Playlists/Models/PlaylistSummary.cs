// <copyright file="PlaylistSummary.cs" company="Fabian Schmieder">
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
namespace SubSonicMedia.Responses.Playlists.Models
{
    /// <summary>
    /// Summary information about a playlist.
    /// </summary>
    public class PlaylistSummary
    {
        /// <summary>
        /// Gets or sets the playlist ID.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the playlist name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the playlist comment.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the playlist owner.
        /// </summary>
        public string? Owner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the playlist is public.
        /// </summary>
        public bool Public { get; set; }

        /// <summary>
        /// Gets or sets the number of songs in the playlist.
        /// </summary>
        public int SongCount { get; set; }

        /// <summary>
        /// Gets or sets the duration of the playlist in seconds.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the last modification date.
        /// </summary>
        public DateTime Changed { get; set; }

        /// <summary>
        /// Gets or sets the playlist's cover art ID.
        /// </summary>
        public string? CoverArt { get; set; }
    }
}
