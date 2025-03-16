// <copyright file="AppSettings.cs" company="Fabian Schmieder">
// SubSonicMedia - A .NET client library for the Subsonic API
// Copyright (C) 2025 Fabian Schmieder
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// </copyright>

using SubSonicMedia.Models;

namespace SubSonicMedia.TestKit.Models
{
    /// <summary>
    /// Application configuration settings.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the URL of the Subsonic server.
        /// </summary>
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
        public string? ApiVersion { get; set; }

        /// <summary>
        /// Gets or sets the response format (json or xml).
        /// </summary>
        public string ResponseFormat { get; set; } = "json";

        /// <summary>
        /// Gets or sets a value indicating whether to record test results to JSON files.
        /// </summary>
        public bool RecordTestResults { get; set; } = true;

        /// <summary>
        /// Gets or sets the directory where test results should be saved.
        /// </summary>
        public string OutputDirectory { get; set; } = "./Outputs";

        /// <summary>
        /// Gets the connection information for the Subsonic client.
        /// </summary>
        /// <returns>A <see cref="SubsonicConnectionInfo"/> object with connection details.</returns>
        public SubsonicConnectionInfo GetConnectionInfo()
        {
            return new SubsonicConnectionInfo
            {
                ServerUrl = ServerUrl,
                Username = Username,
                Password = Password,
                ApiVersion = ApiVersion,
                ResponseFormat = ResponseFormat
            };
        }
    }
}
