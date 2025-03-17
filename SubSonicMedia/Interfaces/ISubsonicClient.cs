// <copyright file="ISubsonicClient.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for the main Subsonic client that provides access to all Subsonic API functionality.
    /// </summary>
    public interface ISubsonicClient
    {
        /// <summary>
        /// Gets the browsing client that provides access to browsing-related API methods.
        /// </summary>
        IBrowsingClient Browsing { get; }

        /// <summary>
        /// Gets the media client that provides access to media-related API methods.
        /// </summary>
        IMediaClient Media { get; }

        /// <summary>
        /// Gets the playlist client that provides access to playlist-related API methods.
        /// </summary>
        IPlaylistClient Playlists { get; }

        /// <summary>
        /// Gets the search client that provides access to search-related API methods.
        /// </summary>
        ISearchClient Search { get; }

        /// <summary>
        /// Gets the user management client that provides access to user-related API methods.
        /// </summary>
        IUserManagementClient UserManagement { get; }

        /// <summary>
        /// Gets the user client that provides access to user avatar and management methods.
        /// </summary>
        IUserClient User { get; }

        /// <summary>
        /// Gets the system client that provides access to system-related API methods.
        /// </summary>
        ISystemClient System { get; }

        /// <summary>
        /// Gets the chat client that provides access to chat-related API methods.
        /// </summary>
        IChatClient Chat { get; }

        /// <summary>
        /// Gets the jukebox client that provides access to jukebox-related API methods.
        /// </summary>
        IJukeboxClient Jukebox { get; }

        /// <summary>
        /// Gets the video client that provides access to video-related API methods.
        /// </summary>
        IVideoClient Video { get; }

        /// <summary>
        /// Gets the podcast client that provides access to podcast-related API methods.
        /// </summary>
        IPodcastClient Podcasts { get; }

        /// <summary>
        /// Gets the radio client that provides access to internet radio-related API methods.
        /// </summary>
        IRadioClient Radio { get; }

        /// <summary>
        /// Gets the bookmark client that provides access to bookmark-related API methods.
        /// </summary>
        IBookmarkClient Bookmarks { get; }
    }
}
