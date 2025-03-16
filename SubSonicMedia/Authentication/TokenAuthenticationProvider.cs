// <copyright file="TokenAuthenticationProvider.cs" company="Fabian Schmieder">
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
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using SubSonicMedia.Interfaces;
using SubSonicMedia.Models;

namespace SubSonicMedia.Authentication
{
    /// <summary>
    /// Provides token-based authentication for Subsonic API calls (API version 1.13.0 and newer).
    /// </summary>
    /// <remarks>
    /// The token-based authentication is the recommended authentication method for Subsonic API version 1.13.0 and newer.
    /// It uses a one-way salted hash of the password for increased security.
    /// </remarks>
    public class TokenAuthenticationProvider : IAuthenticationProvider
    {
        /// <summary>
        /// Applies token-based authentication parameters to the request.
        /// </summary>
        /// <param name="parameters">Dictionary of request parameters to be modified with authentication information.</param>
        /// <param name="connectionInfo">Connection information for the Subsonic server.</param>
        public void ApplyAuthentication(
            Dictionary<string, string> parameters,
            SubsonicConnectionInfo connectionInfo
        )
        {
            // Generate a random salt
            string salt = GenerateRandomSalt();

            // Calculate token: MD5(password + salt)
            string token = CalculateMd5Hash(connectionInfo.Password + salt);

            // Add authentication parameters
            parameters["u"] = connectionInfo.Username;
            parameters["t"] = token;
            parameters["s"] = salt;
            parameters["v"] = connectionInfo.ApiVersion;
            parameters["c"] = connectionInfo.ClientName;
            parameters["f"] = connectionInfo.ResponseFormat ?? "json";

            // Authentication parameters have been added to the request
        }

        /// <summary>
        /// Generates a random salt string for use in token-based authentication.
        /// </summary>
        /// <param name="length">The length of the salt to generate. Default is 6 characters.</param>
        /// <returns>A random string to use as a salt.</returns>
        private string GenerateRandomSalt(int length = 6)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var stringBuilder = new StringBuilder(length);

            Span<byte> randomBytes = stackalloc byte[length];
            RandomNumberGenerator.Fill(randomBytes);

            for (int i = 0; i < length; i++)
            {
                int index = randomBytes[i] % chars.Length;
                stringBuilder.Append(chars[index]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Calculates the MD5 hash of a string.
        /// </summary>
        /// <param name="input">The input string to hash.</param>
        /// <returns>The MD5 hash as a lowercase hexadecimal string.</returns>
        private static string CalculateMd5Hash(string input)
        {
#pragma warning disable CA5351 // Do not use broken cryptographic algorithms
            // MD5 is required by the Subsonic API specification
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = MD5.HashData(inputBytes);

            // Convert the byte array to a lowercase hexadecimal string
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
#pragma warning restore CA5351
        }
    }
}
