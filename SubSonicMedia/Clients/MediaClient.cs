// <copyright file="MediaClient.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for media-related Subsonic API methods.
    /// </summary>
    internal class MediaClient : IMediaClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public MediaClient(SubsonicClient client)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public Task<Stream> StreamAsync(
            string id,
            int? maxBitRate = null,
            string? format = null,
            int? timeOffset = null,
            string? size = null,
            bool? estimateContentLength = null,
            bool? converted = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty", nameof(id));
            }

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
                    estimateContentLength.Value.ToString().ToLowerInvariant()
                );
            }

            if (converted.HasValue)
            {
                parameters.Add("converted", converted.Value.ToString().ToLowerInvariant());
            }

            return this._client.ExecuteBinaryRequestAsync("stream", parameters, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<Stream> DownloadAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            return this._client.ExecuteBinaryRequestAsync(
                "download",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<Stream> GetHlsPlaylistAsync(
            string id,
            int? bitRate = null,
            int? audioTrack = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            if (bitRate.HasValue)
            {
                parameters.Add("bitRate", bitRate.Value.ToString());
            }

            if (audioTrack.HasValue)
            {
                parameters.Add("audioTrack", audioTrack.Value.ToString());
            }

            return this._client.ExecuteBinaryRequestAsync("hls", parameters, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<Stream> GetCoverArtAsync(
            string id,
            int? size = null,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty", nameof(id));
            }

            var parameters = new Dictionary<string, string> { { "id", id } };

            if (size.HasValue)
            {
                parameters.Add("size", size.Value.ToString());
            }

            return this._client.ExecuteBinaryRequestAsync(
                "getCoverArt",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public async Task<string> GetLyricsAsync(
            string artist,
            string title,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(artist))
            {
                parameters.Add("artist", artist);
            }

            if (!string.IsNullOrEmpty(title))
            {
                parameters.Add("title", title);
            }

            // The lyrics endpoint returns XML with a <lyrics> element containing the lyrics text
            // We use a Stream here and will need to extract the lyrics text from the XML
            using (
                var stream = await this
                    ._client.ExecuteBinaryRequestAsync("getLyrics", parameters, cancellationToken)
                    .ConfigureAwait(false)
            )
            using (var reader = new StreamReader(stream))
            {
                string response = await reader.ReadToEndAsync().ConfigureAwait(false);

                // Simple extraction of lyrics text - in a real implementation this would use proper XML parsing
                int startIndex = response.IndexOf("<lyrics>");
                int endIndex = response.IndexOf("</lyrics>");

                if (startIndex >= 0 && endIndex >= 0)
                {
                    startIndex += "<lyrics>".Length;
                    return response.Substring(startIndex, endIndex - startIndex);
                }

                return string.Empty;
            }
        }
    }
}
