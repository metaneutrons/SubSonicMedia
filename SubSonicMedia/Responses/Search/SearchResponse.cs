// <copyright file="SearchResponse.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Responses.Search
{
    /// <summary>
    /// Response for search methods (search and search2).
    /// </summary>
    public class SearchResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the search result.
        /// </summary>
        public SearchResult SearchResult { get; set; } = new SearchResult();
    }

    /// <summary>
    /// Contains search results.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets the list of matching artists.
        /// </summary>
        public List<Artist> Artists { get; set; } = new List<Artist>();

        /// <summary>
        /// Gets or sets the list of matching albums.
        /// </summary>
        public List<Album> Albums { get; set; } = new List<Album>();

        /// <summary>
        /// Gets or sets the list of matching songs.
        /// </summary>
        public List<Song> Songs { get; set; } = new List<Song>();
    }

    /// <summary>
    /// Represents an artist.
    /// </summary>
    public class Artist
    {
        /// <summary>
        /// Gets or sets the artist ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the artist name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the artist's album count.
        /// </summary>
        public int AlbumCount { get; set; }

        /// <summary>
        /// Gets or sets the artist's cover art identifier.
        /// </summary>
        public string CoverArt { get; set; }
    }
}
