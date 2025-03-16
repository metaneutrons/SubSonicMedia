// <copyright file="AlbumListType.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Responses.Browsing
{
    /// <summary>
    /// Type of album list to return.
    /// </summary>
    public enum AlbumListType
    {
        /// <summary>
        /// Albums randomly selected from all albums.
        /// </summary>
        Random,

        /// <summary>
        /// Albums most recently played by the current user.
        /// </summary>
        Recent,

        /// <summary>
        /// Albums recently added to the media library.
        /// </summary>
        Newest,

        /// <summary>
        /// Albums frequently played by the current user.
        /// </summary>
        Frequent,

        /// <summary>
        /// Albums rated 4 or 5 stars by the current user.
        /// </summary>
        Highest,

        /// <summary>
        /// Albums starred by the current user.
        /// </summary>
        Starred,

        /// <summary>
        /// Albums not recently played by the current user.
        /// </summary>
        AlphabetalByName,

        /// <summary>
        /// Albums ordered alphabetically by artist.
        /// </summary>
        AlphabetalByArtist,

        /// <summary>
        /// Albums by the current user's starred artists.
        /// </summary>
        ByYear,

        /// <summary>
        /// Albums by the current user's genre.
        /// </summary>
        ByGenre
    }
}
