// <copyright file="IAuthenticationProvider.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for authentication providers that handle the authentication process with a Subsonic server.
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// Applies authentication parameters to the request parameters.
        /// </summary>
        /// <param name="parameters">Dictionary of request parameters to be modified with authentication information.</param>
        /// <param name="connectionInfo">Connection information for the Subsonic server.</param>
        void ApplyAuthentication(
            Dictionary<string, string> parameters,
            SubsonicConnectionInfo connectionInfo
        );
    }
}
