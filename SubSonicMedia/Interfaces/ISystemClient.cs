// <copyright file="ISystemClient.cs" company="Fabian Schmieder">
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

using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Responses.System;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API system methods.
    /// </summary>
    public interface ISystemClient
    {
        /// <summary>
        /// Tests connectivity with the server.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A base response indicating success or failure.</returns>
        Task<BaseResponse> PingAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets details about the software license.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing license details.</returns>
        Task<LicenseResponse> GetLicenseAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the current status of the scan.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing scan status.</returns>
        Task<ScanStatusResponse> GetScanStatusAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Initiates a media library scan.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing scan status.</returns>
        Task<ScanStatusResponse> StartScanAsync(CancellationToken cancellationToken = default);
    }
}
