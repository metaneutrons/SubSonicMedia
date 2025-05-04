// <copyright file="AnnotationClient.cs" company="Fabian Schmieder">
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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SubSonicMedia.Interfaces;
using SubSonicMedia.Responses.Annotation;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for media annotation endpoints (scrobble, star, unstar, setRating).
    /// </summary>
    internal class AnnotationClient : IAnnotationClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client instance used to execute API requests.</param>
        public AnnotationClient(SubsonicClient client) => this._client = client ?? throw new ArgumentNullException(nameof(client));

        /// <summary>
        /// Registers local playback (scrobble) of media files.
        /// </summary>
        /// <param name="ids">Collection of media file IDs that should be scrobbled.</param>
        /// <param name="times">Optional. Collection of timestamps (Unix time in milliseconds) when each media file playback occurred. If specified, the collection should have the same size as the ids collection.</param>
        /// <param name="submission">Optional. Whether this is a "submission" or a "now playing" notification. Default is true (submission).</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="ScrobbleResponse"/> containing the result of the operation.</returns>
        public async Task<ScrobbleResponse> ScrobbleAsync(
            IEnumerable<string> ids,
            IEnumerable<long>? times = null,
            bool? submission = null,
            CancellationToken cancellationToken = default)
        {
            var parameters = new Dictionary<string, string>
            { { "id", string.Join(",", ids) } };
            if (times != null)
            {
                parameters.Add("time", string.Join(",", times));
            }
            if (submission.HasValue)
            {
                parameters.Add("submission", submission.Value.ToString().ToLowerInvariant());
            }
            return await this._client.ExecuteRequestAsync<ScrobbleResponse>(
                "scrobble", parameters, cancellationToken);
        }

        /// <summary>
        /// Adds a star to the specified media file.
        /// </summary>
        /// <param name="id">The unique identifier of the media file to star.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="StarResponse"/> indicating the result of the operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the ID is null or empty.</exception>
        public async Task<StarResponse> StarAsync(
            string id,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };
            return await this._client.ExecuteRequestAsync<StarResponse>("star", parameters, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Removes a star from the specified media file.
        /// </summary>
        /// <param name="id">The unique identifier of the media file to unstar.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="StarResponse"/> indicating the result of the operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the ID is null or empty.</exception>
        public async Task<StarResponse> UnstarAsync(
            string id,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };
            return await this._client.ExecuteRequestAsync<StarResponse>("unstar", parameters, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Sets the rating for the specified media file (0-5).
        /// </summary>
        /// <param name="id">The unique identifier of the media file to rate.</param>
        /// <param name="rating">The rating to set, must be between 0 and 5 (inclusive). 0 removes the rating.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="RatingResponse"/> indicating the result of the operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the ID is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the rating is not between 0 and 5.</exception>
        public async Task<RatingResponse> SetRatingAsync(
            string id,
            int rating,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty", nameof(id));
            }

            if (rating < 0 || rating > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 0 and 5");
            }

            var parameters = new Dictionary<string, string>
            {
                { "id", id },
                { "rating", rating.ToString() },
            };

            return await this._client.ExecuteRequestAsync<RatingResponse>("setRating", parameters, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
