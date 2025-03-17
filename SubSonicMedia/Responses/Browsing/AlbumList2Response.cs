// <copyright file="AlbumList2Response.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Search;

namespace SubSonicMedia.Responses.Browsing
{
    /// <summary>
    /// Response containing an album list using ID3 tags.
    /// </summary>
    public class AlbumList2Response : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the album list.
        /// </summary>
        public AlbumList2 AlbumList2 { get; set; } = new AlbumList2();
    }

    /// <summary>
    /// Container for album list.
    /// </summary>
    public class AlbumList2
    {
        /// <summary>
        /// Gets or sets the albums.
        /// </summary>
        public List<Album> Album { get; set; } = new List<Album>();
    }
}
