// <copyright file="SubsonicAuthenticationException.cs" company="Fabian Schmieder">
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
    /// Exception thrown when a Subsonic API authentication fails.
    /// </summary>
    public class SubsonicAuthenticationException : SubsonicApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubsonicAuthenticationException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code from the Subsonic API.</param>
        /// <param name="message">The error message.</param>
        public SubsonicAuthenticationException(int errorCode, string message)
            : base(errorCode, message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubsonicAuthenticationException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code from the Subsonic API.</param>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SubsonicAuthenticationException(
            int errorCode,
            string message,
            Exception innerException
        )
            : base(errorCode, message, innerException) { }
    }
}
