// <copyright file="ArtistInfoResponse.cs" company="Fabian Schmieder">
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

using System.Collections.Generic;
using SubSonicMedia.Responses.Search;

namespace SubSonicMedia.Responses.Browsing
{
    /// <summary>
    /// Response containing artist info.
    /// </summary>
    public class ArtistInfoResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the artist info.
        /// </summary>
        public ArtistInfo ArtistInfo { get; set; } = new ArtistInfo();
    }

    /// <summary>
    /// Artist info with biography and similar artists.
    /// </summary>
    public class ArtistInfo
    {
        /// <summary>
        /// Gets or sets the biography.
        /// </summary>
        public string Biography { get; set; }

        /// <summary>
        /// Gets or sets the MusicBrainz ID.
        /// </summary>
        public string MusicBrainzId { get; set; }

        /// <summary>
        /// Gets or sets the Last.fm URL.
        /// </summary>
        public string LastFmUrl { get; set; }

        /// <summary>
        /// Gets or sets the small image URL.
        /// </summary>
        public string SmallImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the medium image URL.
        /// </summary>
        public string MediumImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the large image URL.
        /// </summary>
        public string LargeImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the similar artists.
        /// </summary>
        public List<Artist> SimilarArtist { get; set; } = new List<Artist>();
    }
}
