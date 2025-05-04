// <copyright file="IAnnotationClient.cs" company="Fabian Schmieder">
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

using SubSonicMedia.Responses.Annotation;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Client for media annotation endpoints (star, unstar, setRating, scrobble).
    /// </summary>
    public interface IAnnotationClient
    {
        /// <summary>
        /// Registers local playback (scrobble) of media files.
        /// </summary>
        /// <param name="ids">Collection of media file IDs that should be scrobbled.</param>
        /// <param name="times">Optional. Collection of timestamps (Unix time in milliseconds) when each media file playback occurred. If specified, the collection should have the same size as the ids collection.</param>
        /// <param name="submission">Optional. Whether this is a "submission" or a "now playing" notification. Default is true (submission).</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="ScrobbleResponse"/> containing the result of the operation.</returns>
        Task<ScrobbleResponse> ScrobbleAsync(
            IEnumerable<string> ids,
            IEnumerable<long>? times = null,
            bool? submission = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Adds a star to the specified media file.
        /// </summary>
        /// <param name="id">The unique identifier of the media file to star.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="StarResponse"/> indicating the result of the operation.</returns>
        Task<StarResponse> StarAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a star from the specified media file.
        /// </summary>
        /// <param name="id">The unique identifier of the media file to unstar.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="StarResponse"/> indicating the result of the operation.</returns>
        Task<StarResponse> UnstarAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the rating for the specified media file (0-5).
        /// </summary>
        /// <param name="id">The unique identifier of the media file to rate.</param>
        /// <param name="rating">The rating to set, must be between 0 and 5 (inclusive). 0 removes the rating.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="RatingResponse"/> indicating the result of the operation.</returns>
        Task<RatingResponse> SetRatingAsync(
            string id,
            int rating,
            CancellationToken cancellationToken = default
        );
    }
}
