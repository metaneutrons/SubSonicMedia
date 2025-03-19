// <copyright file="IChatClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Chat;
using SubSonicMedia.Responses.System;

namespace SubSonicMedia.Interfaces
{
    /// <summary>
    /// Interface for accessing Subsonic API chat methods.
    /// </summary>
    public interface IChatClient
    {
        /// <summary>
        /// Gets the current chat messages.
        /// </summary>
        /// <param name="since">Only return messages newer than this timestamp (milliseconds since 1970).</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A response containing chat messages.</returns>
        Task<ChatMessagesResponse> GetChatMessagesAsync(
            long? since = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Adds a message to the chat.
        /// </summary>
        /// <param name="message">The message to add.</param>
        /// <param name="cancellationToken">A token for canceling the operation.</param>
        /// <returns>A basic response indicating success or failure.</returns>
        Task<BaseResponse> AddChatMessageAsync(
            string message,
            CancellationToken cancellationToken = default
        );
    }
}
