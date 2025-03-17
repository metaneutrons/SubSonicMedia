// <copyright file="AlbumInfoResponse.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Responses.Browsing
{
    /// <summary>
    /// Response containing album info.
    /// </summary>
    public class AlbumInfoResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the album info.
        /// </summary>
        public AlbumInfo AlbumInfo { get; set; } = new AlbumInfo();
    }

    /// <summary>
    /// Album info with notes and other metadata.
    /// </summary>
    public class AlbumInfo
    {
        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        public string Notes { get; set; }

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
    }
}
