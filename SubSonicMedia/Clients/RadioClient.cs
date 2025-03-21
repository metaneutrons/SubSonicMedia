// <copyright file="RadioClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Interfaces;
using SubSonicMedia.Responses.Radio;
using SubSonicMedia.Responses.System;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for accessing Subsonic API internet radio methods.
    /// </summary>
    internal class RadioClient : IRadioClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadioClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public RadioClient(SubsonicClient client)
        {
            this._client = client;
        }

        /// <inheritdoc/>
        public Task<InternetRadioStationsResponse> GetInternetRadioStationsAsync(
            CancellationToken cancellationToken = default
        )
        {
            return this._client.ExecuteRequestAsync<InternetRadioStationsResponse>(
                "getInternetRadioStations",
                new Dictionary<string, string>(),
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> CreateInternetRadioStationAsync(
            string streamUrl,
            string name,
            string? homepageUrl = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>
            {
                { "streamUrl", streamUrl },
                { "name", name },
            };

            if (!string.IsNullOrEmpty(homepageUrl))
            {
                parameters.Add("homepageUrl", homepageUrl);
            }

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "createInternetRadioStation",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> UpdateInternetRadioStationAsync(
            string id,
            string streamUrl,
            string name,
            string? homepageUrl = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>
            {
                { "id", id },
                { "streamUrl", streamUrl },
                { "name", name },
            };

            if (!string.IsNullOrEmpty(homepageUrl))
            {
                parameters.Add("homepageUrl", homepageUrl);
            }

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "updateInternetRadioStation",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> DeleteInternetRadioStationAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "deleteInternetRadioStation",
                parameters,
                cancellationToken
            );
        }
    }
}
