// <copyright file="UserResponse.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Responses.UserManagement
{
    /// <summary>
    /// Response containing user details.
    /// </summary>
    public class UserResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the user details.
        /// </summary>
        public User User { get; set; } = new User();
    }

    /// <summary>
    /// Details about a user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is scrobbling enabled.
        /// </summary>
        public bool ScrobblingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the maximum bitrate.
        /// </summary>
        public int MaxBitRate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has admin role.
        /// </summary>
        public bool AdminRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has settings role.
        /// </summary>
        public bool SettingsRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has download role.
        /// </summary>
        public bool DownloadRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has upload role.
        /// </summary>
        public bool UploadRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has playlist role.
        /// </summary>
        public bool PlaylistRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has cover art role.
        /// </summary>
        public bool CoverArtRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has comment role.
        /// </summary>
        public bool CommentRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has podcast role.
        /// </summary>
        public bool PodcastRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has stream role.
        /// </summary>
        public bool StreamRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has jukebox role.
        /// </summary>
        public bool JukeboxRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has share role.
        /// </summary>
        public bool ShareRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has video conversion role.
        /// </summary>
        public bool VideoConversionRole { get; set; }

        /// <summary>
        /// Gets or sets the music folders this user has access to.
        /// </summary>
        public List<MusicFolder> MusicFolder { get; set; } = new List<MusicFolder>();
    }

    /// <summary>
    /// A music folder that a user has access to.
    /// </summary>
    public class MusicFolder
    {
        /// <summary>
        /// Gets or sets the music folder ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the music folder name.
        /// </summary>
        public string Name { get; set; }
    }
}
