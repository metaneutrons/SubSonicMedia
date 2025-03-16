// <copyright file="ConnectionTest.cs" company="Fabian Schmieder">
// SubSonicMedia - A .NET client library for the Subsonic API
// Copyright (C) 2025 Fabian Schmieder
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// </copyright>

using SubSonicMedia.TestKit.Helpers;
using SubSonicMedia.TestKit.Models;

namespace SubSonicMedia.TestKit.Tests
{
    /// <summary>
    /// Tests the connection to the Subsonic server.
    /// </summary>
    public class ConnectionTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public ConnectionTest(SubsonicClient client, AppSettings settings)
            : base(client, settings)
        {
        }

        /// <inheritdoc/>
        public override string Name => "Connection Test";

        /// <inheritdoc/>
        public override string Description => "Tests connection to the Subsonic server and API version compatibility";

        /// <inheritdoc/>
        protected override async Task<bool> ExecuteTestAsync()
        {
            ConsoleHelper.LogInfo("Testing server connection...");
            ConsoleHelper.LogServerConnection(Settings.ServerUrl);

            // Ping the server to check connection
            var response = await Client.System.PingAsync();
            
            RecordTestResult(response, "connection_ping");
            
            if (response.IsSuccess)
            {
                ConsoleHelper.LogSuccess($"Successfully connected to server version: {response.Version}");
                return true;
            }
            else
            {
                ConsoleHelper.LogError($"Connection failed: {response.Error?.Message}");
                return false;
            }
        }
    }
}
