// <copyright file="IndexesResponse.cs" company="Fabian Schmieder">
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

using System;
using System.Collections.Generic;

namespace SubSonicMedia.Responses.Browsing
{
    /// <summary>
    /// Response containing artist indexes.
    /// </summary>
    public class IndexesResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the indexes container.
        /// </summary>
        public Indexes Indexes { get; set; } = new Indexes();
    }

    /// <summary>
    /// Container for artist indexes.
    /// </summary>
    public class Indexes
    {
        /// <summary>
        /// Gets or sets the timestamp when the index was last modified.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the index is ignorable.
        /// </summary>
        public bool Ignorable { get; set; }

        /// <summary>
        /// Gets or sets the list of indexes.
        /// </summary>
        public List<Index> Index { get; set; } = new List<Index>();

        /// <summary>
        /// Gets or sets the list of shortcuts.
        /// </summary>
        public List<Artist> Shortcut { get; set; } = new List<Artist>();

        /// <summary>
        /// Gets or sets the list of children.
        /// </summary>
        public List<Artist> Child { get; set; } = new List<Artist>();
    }

    /// <summary>
    /// An index of artists.
    /// </summary>
    public class Index
    {
        /// <summary>
        /// Gets or sets the index name (typically a letter).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of artists.
        /// </summary>
        public List<Artist> Artist { get; set; } = new List<Artist>();
    }

    /// <summary>
    /// An artist entry in an index.
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
        /// Gets or sets the artist's image or cover art ID.
        /// </summary>
        public string CoverArt { get; set; }

        /// <summary>
        /// Gets or sets the number of albums by this artist.
        /// </summary>
        public int AlbumCount { get; set; }

        /// <summary>
        /// Gets or sets the artist's image URL (Added in API 1.16.1).
        /// </summary>
        public string ArtistImageUrl { get; set; }
    }
}
