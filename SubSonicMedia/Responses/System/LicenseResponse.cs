// <copyright file="LicenseResponse.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Responses.System
{
    /// <summary>
    /// Response containing license information.
    /// </summary>
    public class LicenseResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the license information.
        /// </summary>
        public License License { get; set; } = new License();
    }

    /// <summary>
    /// License details for the Subsonic server.
    /// </summary>
    public class License
    {
        /// <summary>
        /// Gets or sets a value indicating whether the server is licensed.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets the email address the license is registered to.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the license key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the license expiration date.
        /// </summary>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Gets or sets the license expiration date from the server.
        /// </summary>
        public DateTime? LicenseExpires { get; set; }

        /// <summary>
        /// Gets or sets the name of the licensed server product.
        /// </summary>
        public string LicenseVersion { get; set; }

        /// <summary>
        /// Gets or sets the trial period information.
        /// </summary>
        public string Trial { get; set; }

        /// <summary>
        /// Gets or sets the trial expiration date.
        /// </summary>
        public DateTime? TrialExpires { get; set; }
    }
}
