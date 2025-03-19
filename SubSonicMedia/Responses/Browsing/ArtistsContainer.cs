// <copyright file="ArtistsContainer.cs" company="Fabian Schmieder">
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
namespace SubSonicMedia.Responses.Browsing
{
    /// <summary>
    /// Container for all artists.
    /// </summary>
    public class ArtistsContainer
    {
        /// <summary>
        /// Gets or sets a value indicating whether the content is ignorable.
        /// </summary>
        public bool Ignorable { get; set; }

        /// <summary>
        /// Gets or sets the ignored articles string (e.g., "The,El,La").
        /// </summary>
        public string IgnoredArticles { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last modified timestamp.
        /// </summary>
        public long LastModified { get; set; }

        /// <summary>
        /// Gets or sets the artist indexes.
        /// </summary>
        public List<Index> Index { get; set; } = new List<Index>();
    }
}
