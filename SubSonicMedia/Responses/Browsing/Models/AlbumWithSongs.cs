// <copyright file="AlbumWithSongs.cs" company="Fabian Schmieder">
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
    /// Album with its songs.
    /// </summary>
    public class AlbumWithSongs
    {
        /// <summary>
        /// Gets or sets the album ID.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the album name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the artist name.
        /// </summary>
        public string Artist { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the artist ID.
        /// </summary>
        public string ArtistId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the cover art ID.
        /// </summary>
        public string CoverArt { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the song count.
        /// </summary>
        public int SongCount { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the played count.
        /// </summary>
        public int? PlayCount { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public long? Created { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this album is starred.
        /// </summary>
        public bool Starred { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the songs in the album.
        /// </summary>
        public List<Song> Song { get; set; } = new List<Song>();
    }
}
