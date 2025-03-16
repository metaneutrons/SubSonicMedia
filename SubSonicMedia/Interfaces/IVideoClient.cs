// <copyright file="IVideoClient.cs" company="Fabian Schmieder">
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

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Responses.Video;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API video methods.
    /// </summary>
    public interface IVideoClient
    {
        /// <summary>
        /// Gets all videos in the library.
        /// </summary>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing all videos.</returns>
        Task<VideosResponse> GetVideosAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a video or cover art image.
        /// </summary>
        /// <param name="id">The ID of the video or cover art.</param>
        /// <param name="maxWidth">The maximum width in pixels.</param>
        /// <param name="maxHeight">The maximum height in pixels.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>The video or cover art stream.</returns>
        Task<byte[]> GetVideoCoverArtAsync(
            string id,
            int? maxWidth = null,
            int? maxHeight = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Streams a given video.
        /// </summary>
        /// <param name="id">The ID of the video to stream.</param>
        /// <param name="maxBitRate">The maximum bit rate in kilobits per second.</param>
        /// <param name="format">The target format (mp4, webm, etc.).</param>
        /// <param name="timeOffset">The time offset in seconds.</param>
        /// <param name="size">The requested video size (WIDTHxHEIGHT).</param>
        /// <param name="estimateContentLength">Whether to include a content-length header.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>The video stream.</returns>
        Task<string> GetVideoStreamUrlAsync(
            string id,
            int? maxBitRate = null,
            string format = null,
            int? timeOffset = null,
            string size = null,
            bool? estimateContentLength = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets detailed information about a video, such as available audio tracks and captions.
        /// </summary>
        /// <param name="id">The ID of the video.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing video information.</returns>
        Task<VideoInfoResponse> GetVideoInfoAsync(
            string id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Gets captions (subtitles) for a video. Supports VTT and SRT formats.
        /// </summary>
        /// <param name="id">The ID of the caption.</param>
        /// <param name="format">The caption format (vtt or srt).</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>The captions as a stream.</returns>
        Task<Stream> GetCaptionsAsync(
            string id,
            string format = null,
            CancellationToken cancellationToken = default
        );
    }
}
