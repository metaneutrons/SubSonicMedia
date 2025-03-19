// <copyright file="SubsonicApiException.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses;

namespace SubSonicMedia.Exceptions
{
    /// <summary>
    /// Exception thrown when a Subsonic API request fails.
    /// </summary>
    public class SubsonicApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubsonicApiException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code from the Subsonic API.</param>
        /// <param name="message">The error message.</param>
        public SubsonicApiException(int errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubsonicApiException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code from the Subsonic API.</param>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SubsonicApiException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the error code from the Subsonic API.
        /// </summary>
        public int ErrorCode { get; }

        /// <summary>
        /// Creates a <see cref="SubsonicApiException"/> from a <see cref="SubsonicError"/>.
        /// </summary>
        /// <param name="error">The Subsonic error.</param>
        /// <returns>A new <see cref="SubsonicApiException"/> instance.</returns>
        public static SubsonicApiException FromError(SubsonicError error)
        {
            return new SubsonicApiException(error.Code, error.Message);
        }
    }
}
