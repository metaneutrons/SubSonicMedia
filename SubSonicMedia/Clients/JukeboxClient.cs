// <copyright file="JukeboxClient.cs" company="Fabian Schmieder">
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

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SubSonicMedia.Interfaces;
using SubSonicMedia.Responses.Jukebox;

namespace SubSonicMedia.Clients
{
    /// <summary>
    /// Client for accessing Subsonic API jukebox methods.
    /// </summary>
    internal class JukeboxClient : IJukeboxClient
    {
        private readonly SubsonicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="JukeboxClient"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        public JukeboxClient(SubsonicClient client)
        {
            _client = client;
        }

        /// <inheritdoc/>
        public Task<JukeboxStatusResponse> GetJukeboxStatusAsync(
            CancellationToken cancellationToken = default
        )
        {
            return _client.ExecuteRequestAsync<JukeboxStatusResponse>(
                "jukeboxControl",
                new Dictionary<string, string> { { "action", "status" } },
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<JukeboxControlResponse> SetJukeboxGainAsync(
            float gain,
            CancellationToken cancellationToken = default
        )
        {
            return _client.ExecuteRequestAsync<JukeboxControlResponse>(
                "jukeboxControl",
                new Dictionary<string, string>
                {
                    { "action", "setGain" },
                    { "gain", gain.ToString() }
                },
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<JukeboxControlResponse> ControlJukeboxAsync(
            string action,
            int? index = null,
            int? offset = null,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "action", action } };

            if (index.HasValue)
            {
                parameters.Add("index", index.Value.ToString());
            }

            if (offset.HasValue)
            {
                parameters.Add("offset", offset.Value.ToString());
            }

            return _client.ExecuteRequestAsync<JukeboxControlResponse>(
                "jukeboxControl",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<JukeboxPlaylistResponse> AddToJukeboxAsync(
            IEnumerable<string> ids,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "action", "add" } };

            var idArray = ids.ToArray();
            for (int i = 0; i < idArray.Length; i++)
            {
                parameters.Add($"id", idArray[i]);
            }

            return _client.ExecuteRequestAsync<JukeboxPlaylistResponse>(
                "jukeboxControl",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<JukeboxPlaylistResponse> RemoveFromJukeboxAsync(
            IEnumerable<int> indexes,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "action", "remove" } };

            var indexArray = indexes.ToArray();
            for (int i = 0; i < indexArray.Length; i++)
            {
                parameters.Add($"index", indexArray[i].ToString());
            }

            return _client.ExecuteRequestAsync<JukeboxPlaylistResponse>(
                "jukeboxControl",
                parameters,
                cancellationToken
            );
        }

        /// <inheritdoc/>
        public Task<JukeboxPlaylistResponse> SetJukeboxPlaylistAsync(
            IEnumerable<string> ids,
            CancellationToken cancellationToken = default
        )
        {
            var parameters = new Dictionary<string, string> { { "action", "set" } };

            var idArray = ids.ToArray();
            for (int i = 0; i < idArray.Length; i++)
            {
                parameters.Add($"id", idArray[i]);
            }

            return _client.ExecuteRequestAsync<JukeboxPlaylistResponse>(
                "jukeboxControl",
                parameters,
                cancellationToken
            );
        }
    }
}
