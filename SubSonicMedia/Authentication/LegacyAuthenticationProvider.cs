// <copyright file="LegacyAuthenticationProvider.cs" company="Fabian Schmieder">
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
using System.Text;

using SubSonicMedia.Interfaces;
using SubSonicMedia.Models;

namespace SubSonicMedia.Authentication
{
    /// <summary>
    /// Provides legacy authentication for Subsonic API calls (API version 1.12.0 and earlier).
    /// </summary>
    /// <remarks>
    /// The legacy authentication method sends the password either as plaintext or hex-encoded.
    /// This is less secure than token authentication and should only be used with older Subsonic servers.
    /// </remarks>
    public class LegacyAuthenticationProvider : IAuthenticationProvider
    {
        private readonly bool _useHexEncoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyAuthenticationProvider"/> class.
        /// </summary>
        /// <param name="useHexEncoding">Indicates whether to use hex encoding for the password.</param>
        public LegacyAuthenticationProvider(bool useHexEncoding = false)
        {
            this._useHexEncoding = useHexEncoding;
        }

        /// <summary>
        /// Applies legacy authentication parameters to the request.
        /// </summary>
        /// <param name="parameters">Dictionary of request parameters to be modified with authentication information.</param>
        /// <param name="connectionInfo">Connection information for the Subsonic server.</param>
        public void ApplyAuthentication(
            Dictionary<string, string> parameters,
            SubsonicConnectionInfo connectionInfo
        )
        {
            parameters["u"] = connectionInfo.Username;

            if (this._useHexEncoding)
            {
                // Hex-encode the password
                byte[] passwordBytes = Encoding.UTF8.GetBytes(connectionInfo.Password);
                string hexPassword = BitConverter
                    .ToString(passwordBytes)
                    .Replace("-", string.Empty)
                    .ToLower();
                parameters["p"] = "enc:" + hexPassword;
            }
            else
            {
                // Use plaintext password
                parameters["p"] = connectionInfo.Password;
            }

            parameters["v"] = connectionInfo.ApiVersion;
            parameters["c"] = connectionInfo.ClientName;
        }
    }
}
