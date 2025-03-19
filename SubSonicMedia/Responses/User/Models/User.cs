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
namespace SubSonicMedia.Responses.User.Models
{
    /// <summary>
    /// Represents a Subsonic user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user is an administrator.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has settings permission.
        /// </summary>
        public bool SettingsRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has download permission.
        /// </summary>
        public bool DownloadRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has upload permission.
        /// </summary>
        public bool UploadRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has playlist permission.
        /// </summary>
        public bool PlaylistRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has cover art permission.
        /// </summary>
        public bool CoverArtRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has comment permission.
        /// </summary>
        public bool CommentRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has podcast permission.
        /// </summary>
        public bool PodcastRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has streaming permission.
        /// </summary>
        public bool StreamRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has jukebox permission.
        /// </summary>
        public bool JukeboxRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has share permission.
        /// </summary>
        public bool ShareRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has video conversion permission.
        /// </summary>
        public bool VideoConversionRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's LDAP authentication is enabled.
        /// </summary>
        public bool? LdapAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the maximum bitrate for the user.
        /// </summary>
        public int? MaxBitRate { get; set; }

        /// <summary>
        /// Gets or sets the scrobbling configuration.
        /// </summary>
        public string ScrobblingEnabled { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the avatar scheme.
        /// </summary>
        public string AvatarScheme { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the folder IDs to which this user has access.
        /// </summary>
        public string[] FolderIds { get; set; } = new string[0];
    }
}
