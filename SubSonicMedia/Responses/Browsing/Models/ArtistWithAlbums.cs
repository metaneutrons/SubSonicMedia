// <copyright file="ArtistWithAlbums.cs" company="Fabian Schmieder">
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
namespace SubSonicMedia.Responses.Browsing.Models
{
    /// <summary>
    /// Artist with album details.
    /// </summary>
    public class ArtistWithAlbums
    {
        /// <summary>
        /// Gets or sets the artist ID.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the artist name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the artist cover art ID.
        /// </summary>
        public string? CoverArt { get; set; }

        /// <summary>
        /// Gets or sets the artist albums count.
        /// </summary>
        public int AlbumCount { get; set; }

        /// <summary>
        /// Gets or sets the artist's image URL (Added in API 1.16.1).
        /// </summary>
        public string? ArtistImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the list of artist albums.
        /// </summary>
        public List<AlbumSummary> Album { get; set; } = new List<AlbumSummary>();
    }
}
