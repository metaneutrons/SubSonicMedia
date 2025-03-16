// <copyright file="SubsonicApiVersionException.cs" company="Fabian Schmieder">
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

using System;

namespace SubSonicMedia.Exceptions
{
    /// <summary>
    /// Exception thrown when an unsupported Subsonic API version is requested.
    /// </summary>
    public class SubsonicApiVersionException : SubsonicApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubsonicApiVersionException"/> class.
        /// </summary>
        /// <param name="requestedVersion">The requested API version.</param>
        /// <param name="supportedVersion">The maximum supported API version.</param>
        public SubsonicApiVersionException(string requestedVersion, string supportedVersion)
            : base(
                70, // Error code for version mismatch
                $"Subsonic API version '{requestedVersion}' is not supported. Maximum supported version is '{supportedVersion}'."
            )
        {
            RequestedVersion = requestedVersion;
            SupportedVersion = supportedVersion;
        }

        /// <summary>
        /// Gets the requested API version that wasn't supported.
        /// </summary>
        public string RequestedVersion { get; }

        /// <summary>
        /// Gets the maximum supported API version.
        /// </summary>
        public string SupportedVersion { get; }
    }
}
