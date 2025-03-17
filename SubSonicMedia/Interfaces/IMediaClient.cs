// <copyright file="IMediaClient.cs" company="Fabian Schmieder">
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

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API media streaming methods.
    /// </summary>
    public interface IMediaClient
    {
        /// <summary>
        /// Streams a media file.
        /// </summary>
        /// <param name="id">The ID of the file to stream.</param>
        /// <param name="maxBitRate">If specified, the server will downsample the stream to this maximum bit rate.</param>
        /// <param name="format">The preferred streaming format. Can be one of: mp3, flv, ogg, aac, m4a, etc.</param>
        /// <param name="timeOffset">Only applicable for videos. Start streaming at the specified offset (in seconds).</param>
        /// <param name="size">The preferred video size, e.g. "640x480".</param>
        /// <param name="estimateContentLength">Set to true if the Content-Length HTTP header should be set in the response.</param>
        /// <param name="converted">Set to true if this media should be transcoded. Default is based on the system transcoding settings.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A stream containing the media file.</returns>
        Task<Stream> StreamAsync(
            string id,
            int? maxBitRate = null,
            string format = null,
            int? timeOffset = null,
            string size = null,
            bool? estimateContentLength = null,
            bool? converted = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Downloads a media file without transcoding.
        /// </summary>
        /// <param name="id">The ID of the file to download.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A stream containing the original media file data.</returns>
        Task<Stream> DownloadAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates an HLS (HTTP Live Streaming) playlist for streaming audio or video.
        /// </summary>
        /// <param name="id">The ID of the file to stream.</param>
        /// <param name="bitRate">The desired bit rate. Must be one of 128, 160, 192, 256, or 320.</param>
        /// <param name="audioTrack">The ID of the audio track to use.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A stream containing the M3U8 playlist.</returns>
        Task<Stream> GetHlsPlaylistAsync(
            string id,
            int? bitRate = null,
            int? audioTrack = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets a cover art image.
        /// </summary>
        /// <param name="id">The ID of the album or song.</param>
        /// <param name="size">The maximum width/height in pixels.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A stream containing the cover art image.</returns>
        Task<Stream> GetCoverArtAsync(
            string id,
            int? size = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets lyrics for a song.
        /// </summary>
        /// <param name="artist">The artist name.</param>
        /// <param name="title">The song title.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing the lyrics.</returns>
        Task<string> GetLyricsAsync(
            string artist,
            string title,
            CancellationToken cancellationToken = default
        );
    }
}
