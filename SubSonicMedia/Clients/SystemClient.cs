// <copyright file="SystemClient.cs" company="Fabian Schmieder">
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
using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Interfaces;
using SubSonicMedia.Responses.System;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for system-related Subsonic API methods.
    /// </summary>
    internal class SystemClient : ISystemClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public SystemClient(SubsonicClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public Task<BaseResponse> PingAsync(CancellationToken cancellationToken = default)
        {
            return _client.ExecuteRequestAsync<BaseResponse>("ping", null, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<LicenseResponse> GetLicenseAsync(CancellationToken cancellationToken = default)
        {
            return _client.ExecuteRequestAsync<LicenseResponse>(
                "getLicense",
                null,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<ScanStatusResponse> GetScanStatusAsync(
            CancellationToken cancellationToken = default
        )
        {
            return _client.ExecuteRequestAsync<ScanStatusResponse>(
                "getScanStatus",
                null,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<ScanStatusResponse> StartScanAsync(
            CancellationToken cancellationToken = default
        )
        {
            return _client.ExecuteRequestAsync<ScanStatusResponse>(
                "startScan",
                null,
                cancellationToken
            );
        }
    }
}
