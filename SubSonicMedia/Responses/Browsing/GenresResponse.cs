// <copyright file="GenresResponse.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Responses.Browsing
{
    /// <summary>
    /// Response containing all genres.
    /// </summary>
    public class GenresResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the genres container.
        /// </summary>
        public GenresContainer Genres { get; set; } = new GenresContainer();
    }

    /// <summary>
    /// Container for genres.
    /// </summary>
    public class GenresContainer
    {
        /// <summary>
        /// Gets or sets the list of genres.
        /// </summary>
        public List<Genre> Genre { get; set; } = new List<Genre>();
    }

    /// <summary>
    /// Genre information.
    /// </summary>
    public class Genre
    {
        /// <summary>
        /// Gets or sets the genre name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the song count.
        /// </summary>
        public int SongCount { get; set; }

        /// <summary>
        /// Gets or sets the album count.
        /// </summary>
        public int AlbumCount { get; set; }
    }
}
