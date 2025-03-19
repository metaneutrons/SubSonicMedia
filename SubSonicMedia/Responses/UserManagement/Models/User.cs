// <copyright file="User.cs" company="Fabian Schmieder">
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
namespace SubSonicMedia.Responses.UserManagement.Models
{
    /// <summary>
    /// User information.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user is an administrator.
        /// </summary>
        public bool AdminRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to download files.
        /// </summary>
        public bool DownloadRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to upload files.
        /// </summary>
        public bool UploadRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to play files.
        /// </summary>
        public bool PlaylistRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to create and edit playlists.
        /// </summary>
        public bool CoverArtRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to change cover art and tags.
        /// </summary>
        public bool CommentRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to create and edit comments.
        /// </summary>
        public bool PodcastRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to administrate podcasts.
        /// </summary>
        public bool StreamRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to play shared files.
        /// </summary>
        public bool JukeboxRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to play files in jukebox mode.
        /// </summary>
        public bool ShareRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to share files with anyone.
        /// </summary>
        public bool VideoConversionRole { get; set; }

        /// <summary>
        /// Gets or sets the maximum bit rate (in kilobits per second) the user is allowed to use.
        /// </summary>
        public int? MaxBitRate { get; set; }
    }
}
