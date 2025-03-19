// <copyright file="PlaylistTest.cs" company="Fabian Schmieder">
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
using Spectre.Console;
using SubSonicMedia.TestKit.Helpers;
using SubSonicMedia.TestKit.Models;

namespace SubSonicMedia.TestKit.Tests
{
    /// <summary>
    /// Tests the playlist capabilities of the Subsonic API.
    /// </summary>
    public class PlaylistTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public PlaylistTest(SubsonicClient client, AppSettings settings)
            : base(client, settings) { }

        /// <inheritdoc/>
        public override string Name => "Playlist Test";

        /// <inheritdoc/>
        public override string Description =>
            "Tests playlist capabilities including listing playlists and viewing playlist details";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            // Test 1: Get all playlists
            ConsoleHelper.LogInfo("Testing GetPlaylists...");

            // Enable detailed debugging
            ConsoleHelper.LogInfo("Debugging connection:");
            ConsoleHelper.LogInfo($"  Server URL: {this.Settings.ServerUrl}");
            ConsoleHelper.LogInfo($"  Username: {this.Settings.Username}");
            ConsoleHelper.LogInfo($"  API Version: {this.Settings.ApiVersion}");
            ConsoleHelper.LogInfo($"  Response Format: {this.Settings.ResponseFormat}");

            var playlistsResponse = await this.Client.Playlists.GetPlaylistsAsync();

            // Print the raw response for debugging
            try
            {
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                ConsoleHelper.LogInfo("Response received successfully");
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error serializing response: {ex.Message}");
            }

            this.RecordTestResult(playlistsResponse, "playlists_list");

            if (!playlistsResponse.IsSuccess)
            {
                ConsoleHelper.LogError(
                    $"Failed to get playlists: {playlistsResponse.Error?.Message}"
                );
                return TestResult.Fail;
            }

            ConsoleHelper.LogSuccess(
                $"Successfully retrieved {playlistsResponse.Playlists.Playlist.Count} playlists"
            );

            // Display playlist information in a table
            if (playlistsResponse.Playlists.Playlist.Count > 0)
            {
                var table = new Table();
                table.AddColumn("ID");
                table.AddColumn("Name");
                table.AddColumn("Owner");
                table.AddColumn("Songs");
                table.AddColumn("Duration");
                table.AddColumn("Public");

                foreach (var playlist in playlistsResponse.Playlists.Playlist)
                {
                    // Format duration from seconds to mm:ss
                    TimeSpan duration = TimeSpan.FromSeconds(playlist.Duration);
                    string formattedDuration =
                        $"{(int)duration.TotalMinutes}:{duration.Seconds:D2}";

                    table.AddRow(
                        playlist.Id ?? string.Empty,
                        playlist.Name ?? string.Empty,
                        playlist.Owner ?? string.Empty,
                        playlist.SongCount.ToString(),
                        formattedDuration,
                        playlist.Public ? "Yes" : "No"
                    );
                }

                AnsiConsole.Write(table);

                // Test 2: Get details of the first playlist
                var firstPlaylist = playlistsResponse.Playlists.Playlist[0];
                ConsoleHelper.LogInfo($"Testing GetPlaylist for '{firstPlaylist.Name}'...");

                var playlistResponse = await this.Client.Playlists.GetPlaylistAsync(
                    firstPlaylist.Id
                );
                this.RecordTestResult(playlistResponse, "playlist_details");

                if (!playlistResponse.IsSuccess)
                {
                    ConsoleHelper.LogError(
                        $"Failed to get playlist details: {playlistResponse.Error?.Message}"
                    );
                    return TestResult.Fail;
                }

                ConsoleHelper.LogSuccess(
                    $"Successfully retrieved playlist '{playlistResponse.Playlist.Name}' with {playlistResponse.Playlist.Entry.Count} songs"
                );

                // Display song information in a table if there are songs
                if (playlistResponse.Playlist.Entry.Count > 0)
                {
                    var songsTable = new Table();
                    songsTable.AddColumn("#");
                    songsTable.AddColumn("Title");
                    songsTable.AddColumn("Artist");
                    songsTable.AddColumn("Album");
                    songsTable.AddColumn("Duration");

                    int songNumber = 1;
                    foreach (var song in playlistResponse.Playlist.Entry)
                    {
                        // Format duration from seconds to mm:ss
                        TimeSpan songDuration = TimeSpan.FromSeconds(song.Duration);
                        string formattedSongDuration =
                            $"{(int)songDuration.TotalMinutes}:{songDuration.Seconds:D2}";

                        songsTable.AddRow(
                            songNumber.ToString(),
                            song.Title ?? string.Empty,
                            song.Artist ?? string.Empty,
                            song.Album ?? string.Empty,
                            formattedSongDuration
                        );

                        songNumber++;
                    }

                    AnsiConsole.Write(songsTable);
                }
                else
                {
                    ConsoleHelper.LogWarning("Playlist is empty (contains no songs)");
                }
            }
            else
            {
                ConsoleHelper.LogWarning("No playlists found. Skipping playlist details test.");
                ConsoleHelper.LogInfo("This appears to be a test environment without playlists.");
            }

            return TestResult.Pass;
        }
    }
}
