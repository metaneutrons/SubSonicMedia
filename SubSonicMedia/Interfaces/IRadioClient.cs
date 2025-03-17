// <copyright file="IRadioClient.cs" company="Fabian Schmieder">
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
// along with SubSonicMedia. If not, see &lt;https://www.gnu.org/licenses/&gt;.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Responses.Radio;
using SubSonicMedia.Responses.System;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API internet radio methods.
    /// </summary>
    public interface IRadioClient
    {
        /// <summary>
        /// Gets all internet radio stations.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing all internet radio stations.</returns>
        Task<InternetRadioStationsResponse> GetInternetRadioStationsAsync(
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Creates an internet radio station.
        /// </summary>
        /// <param name="streamUrl">The stream URL for the station.</param>
        /// <param name="name">The name of the station.</param>
        /// <param name="homepageUrl">The homepage URL for the station.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> CreateInternetRadioStationAsync(
            string streamUrl,
            string name,
            string? homepageUrl = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Updates an internet radio station.
        /// </summary>
        /// <param name="id">The ID of the station to update.</param>
        /// <param name="streamUrl">The stream URL for the station.</param>
        /// <param name="name">The name of the station.</param>
        /// <param name="homepageUrl">The homepage URL for the station.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> UpdateInternetRadioStationAsync(
            string id,
            string streamUrl,
            string name,
            string? homepageUrl = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes an internet radio station.
        /// </summary>
        /// <param name="id">The ID of the station to delete.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> DeleteInternetRadioStationAsync(
            string id,
            CancellationToken cancellationToken = default
        );
    }
}
