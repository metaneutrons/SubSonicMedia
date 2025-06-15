// <copyright file="ArtistInfo2.cs" company="Fabian Schmieder">
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
using System.Collections.Generic; // Required for List<>
using SubSonicMedia.Responses.Search.Models; // Required for Album

namespace SubSonicMedia.Responses.Browsing.Models
{
    /// <summary>
    /// Artist info with biography and similar artists (version 2).
    /// </summary>
    public class ArtistInfo2
    {
        /// <summary>
        /// Gets or sets the biography.
        /// </summary>
        public string? Biography { get; set; }

        /// <summary>
        /// Gets or sets the MusicBrainz ID.
        /// </summary>
        public string? MusicBrainzId { get; set; }

        /// <summary>
        /// Gets or sets the Last.fm URL.
        /// </summary>
        public string? LastFmUrl { get; set; }

        /// <summary>
        /// Gets or sets the small image URL.
        /// </summary>
        public string? SmallImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the medium image URL.
        /// </summary>
        public string? MediumImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the large image URL.
        /// </summary>
        public string? LargeImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the similar artists.
        /// </summary>
        public List<Artist> SimilarArtist { get; set; } = new List<Artist>();

        /// <summary>
        /// Gets or sets the list of albums by this artist.
        /// </summary>
        public List<Album> Album { get; set; } = new List<Album>();
    }
}
