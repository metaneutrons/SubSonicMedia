// <copyright file="AppSettings.cs" company="Fabian Schmieder">
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

        // Response format is always JSON in this version

        /// <summary>
        /// Gets or sets a value indicating whether to record test results to JSON files.
        /// </summary>
        public bool RecordTestResults { get; set; } = true;

        /// <summary>
        /// Gets or sets the directory where test results should be saved.
        /// </summary>
        public string OutputDirectory { get; set; } = "./Outputs";

        /// <summary>
        /// Gets or sets a value indicating whether to exit immediately on first test failure.
        /// </summary>
        public bool FailFast { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to generate JUnit XML output for CI/CD integration.
        /// </summary>
        public bool JUnitXmlOutput { get; set; } = false;

        /// <summary>
        /// Gets the connection information for the Subsonic client.
        /// </summary>
        /// <returns>A <see cref="SubsonicConnectionInfo"/> object with connection details.</returns>
        public SubsonicConnectionInfo GetConnectionInfo()
        {
            return new SubsonicConnectionInfo
            {
                ServerUrl = this.ServerUrl,
                Username = this.Username,
                Password = this.Password,
                ApiVersion = this.ApiVersion ?? VersionInfo.SubsonicApiVersion,
            };
        }
    }
}
