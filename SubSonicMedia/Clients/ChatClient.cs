// <copyright file="ChatClient.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Responses.Chat;
using SubSonicMedia.Responses.System;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for accessing Subsonic API chat methods.
    /// </summary>
    internal class ChatClient : IChatClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public ChatClient(SubsonicClient client)
        {
            this._client = client;
        }

        /// <inheritdoc/>
        public Task<ChatMessagesResponse> GetChatMessagesAsync(
            long? since = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string>();

            if (since.HasValue)
            {
                parameters.Add("since", since.Value.ToString());
            }

            return this._client.ExecuteRequestAsync<ChatMessagesResponse>(
                "getChatMessages",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<BaseResponse> AddChatMessageAsync(
            string message,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "message", message } };

            return this._client.ExecuteRequestAsync<BaseResponse>(
                "addChatMessage",
                parameters,
                cancellationToken
            );
        }
    }
}
