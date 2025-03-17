// <copyright file="VideoClient.cs" company="Fabian Schmieder">
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Interfaces;
using SubSonicMedia.Responses.Video;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for accessing Subsonic API video methods.
    /// </summary>
    internal class VideoClient : IVideoClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public VideoClient(SubsonicClient client)
        {
            this._client = client;
        }

        /// <inheritdoc/>
        public Task<VideosResponse> GetVideosAsync(CancellationToken cancellationToken = default)
        {
            return this._client.ExecuteRequestAsync<VideosResponse>(
                "getVideos",
                null,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public async Task<byte[]> GetVideoCoverArtAsync(
            string id,
            int? maxWidth = null,
            int? maxHeight = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "id", id } };

            if (maxWidth.HasValue)
            {
                parameters.Add("size", maxWidth.Value.ToString());
            }

            if (maxHeight.HasValue)
            {
                parameters.Add("height", maxHeight.Value.ToString());
            }

            using var stream = await this
                ._client.ExecuteBinaryRequestAsync("getCoverArt", parameters, cancellationToken)
                .ConfigureAwait(false);
            using var memoryStream = new MemoryStream();

            await stream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
            return memoryStream.ToArray();
        }

        /// <inheritdoc/>
        public Task<string> GetVideoStreamUrlAsync(
            string id,
            int? maxBitRate = null,
            string format = null,
            int? timeOffset = null,
            string size = null,
            bool? estimateContentLength = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "id", id } };

            if (maxBitRate.HasValue)
            {
                parameters.Add("maxBitRate", maxBitRate.Value.ToString());
            }

            if (!string.IsNullOrEmpty(format))
            {
                parameters.Add("format", format);
            }

            if (timeOffset.HasValue)
            {
                parameters.Add("timeOffset", timeOffset.Value.ToString());
            }

            if (!string.IsNullOrEmpty(size))
            {
                parameters.Add("size", size);
            }

            if (estimateContentLength.HasValue)
            {
                parameters.Add(
                    "estimateContentLength",
                    estimateContentLength.Value.ToString().ToLower()
                );
            }

            var requestBuilder = new Utilities.RequestBuilder("videostream");
            foreach (var parameter in parameters)
            {
                requestBuilder.AddParameter(parameter.Key, parameter.Value);
            }

            // Return the URL so the client can stream the video
            return Task.FromResult(requestBuilder.BuildRequestUrl());
        }

        /// <inheritdoc/>
        public Task<VideoInfoResponse> GetVideoInfoAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Video ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteRequestAsync<VideoInfoResponse>(
                "getVideoInfo",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public async Task<Stream> GetCaptionsAsync(
            string id,
            string format = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Caption ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            if (!string.IsNullOrEmpty(format))
            {
                parameters.Add("format", format);
            }

            return await this
                ._client.ExecuteBinaryRequestAsync("getCaptions", parameters, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
