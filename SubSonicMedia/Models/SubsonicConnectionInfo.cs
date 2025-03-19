// <copyright file="SubsonicConnectionInfo.cs" company="Fabian Schmieder">
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
namespace SubSonicMedia.Models
{
    /// <summary>
    /// Contains connection information for a Subsonic server.
    /// </summary>
    public class SubsonicConnectionInfo
    {
        /// <summary>
        /// Gets or sets the URL of the Subsonic server.
        /// </summary>
        /// <remarks>
        /// The URL should include the protocol (http:// or https://) and may include a port number.
        /// Example: https://subsonic.example.com:4040.
        /// </remarks>
        public string ServerUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username for authentication.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for authentication.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Subsonic API version to use.
        /// </summary>
        /// <remarks>
        /// The default is <see cref="VersionInfo.SubsonicApiVersion"/>, which is the latest version at the time of implementation.
        /// </remarks>
        public string ApiVersion { get; set; } = VersionInfo.SubsonicApiVersion;

        /// <summary>
        /// Gets or sets the client name for identification to the Subsonic server.
        /// </summary>
        public string ClientName { get; set; } = "SubSonicMedia";

        // Response format is always JSON in this version
    }
}
