// <copyright file="AlbumInfo2Response.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Browsing.Models;

namespace SubSonicMedia.Responses.Browsing
{
    /// <summary>
    /// Response containing album info (version 2).
    /// </summary>
    public class AlbumInfo2Response : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the album info.
        /// </summary>
        public AlbumInfo2 AlbumInfo2 { get; set; } = new AlbumInfo2();
    }
}
