// <copyright file="Starred.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Search.Models;

namespace SubSonicMedia.Responses.Browsing.Models
{
    /// <summary>
    /// Container for starred items.
    /// </summary>
    public class Starred
    {
        /// <summary>
        /// Gets or sets the list of starred artists.
        /// </summary>
        public List<Artist> Artist { get; set; } = new List<Artist>();

        /// <summary>
        /// Gets or sets the list of starred albums.
        /// </summary>
        public List<Album> Album { get; set; } = new List<Album>();

        /// <summary>
        /// Gets or sets the list of starred songs.
        /// </summary>
        public List<Song> Song { get; set; } = new List<Song>();
    }
}
