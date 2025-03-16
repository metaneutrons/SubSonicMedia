// <copyright file="JukeboxTest.cs" company="Fabian Schmieder">
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

using Spectre.Console;
using SubSonicMedia.TestKit.Helpers;
using SubSonicMedia.TestKit.Models;

namespace SubSonicMedia.TestKit.Tests
{
    /// <summary>
    /// Tests jukebox capabilities of the Subsonic API.
    /// </summary>
    public class JukeboxTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JukeboxTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public JukeboxTest(SubsonicClient client, AppSettings settings)
            : base(client, settings)
        {
        }

        /// <inheritdoc/>
        public override string Name => "Jukebox Test";

        /// <inheritdoc/>
        public override string Description => "Tests jukebox control and status features";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            bool allTestsPassed = true;
            string songId = string.Empty;
            
            // First, get a random song ID to add to the jukebox
            ConsoleHelper.LogInfo("Finding a random song for jukebox testing...");
            try
            {
                var randomSongsResponse = await Client.Browsing.GetRandomSongsAsync(1);
                
                if (randomSongsResponse.IsSuccess && 
                    randomSongsResponse.RandomSongs?.Song != null &&
                    randomSongsResponse.RandomSongs.Song.Count > 0 &&
                    !string.IsNullOrEmpty(randomSongsResponse.RandomSongs.Song[0].Id))
                {
                    songId = randomSongsResponse.RandomSongs.Song[0].Id;
                    ConsoleHelper.LogSuccess($"Found song: {randomSongsResponse.RandomSongs.Song[0].Title} by {randomSongsResponse.RandomSongs.Song[0].Artist}");
                }
                else
                {
                    ConsoleHelper.LogWarning("No songs found to test jukebox functionality");
                    return TestResult.Fail;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error finding a song for jukebox testing: {ex.Message}");
                return TestResult.Fail;
            }
            
            // Test 1: Get Jukebox Status
            ConsoleHelper.LogInfo("Testing GetJukeboxStatus...");
            try
            {
                var statusResponse = await Client.Jukebox.GetJukeboxStatusAsync();
                RecordTestResult(statusResponse, "jukebox_status");
                
                if (statusResponse.IsSuccess)
                {
                    ConsoleHelper.LogSuccess("Successfully retrieved jukebox status");
                    
                    // Display status information
                    var table = new Table();
                    table.AddColumn("Property");
                    table.AddColumn("Value");
                    
                    table.AddRow("Current Index", statusResponse.JukeboxStatus.CurrentIndex.ToString());
                    table.AddRow("Playing", statusResponse.JukeboxStatus.Playing.ToString());
                    table.AddRow("Gain", statusResponse.JukeboxStatus.Gain.ToString());
                    table.AddRow("Position", statusResponse.JukeboxStatus.Position.ToString());
                    
                    AnsiConsole.Write(table);
                }
                else
                {
                    ConsoleHelper.LogError($"Failed to get jukebox status: {statusResponse.Error?.Message}");
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error getting jukebox status: {ex.Message}");
                allTestsPassed = false;
            }
            
            // Test 2: Set, get, and clear jukebox playlist
            if (allTestsPassed && !string.IsNullOrEmpty(songId))
            {
                ConsoleHelper.LogInfo("Testing jukebox set/get/clear operations...");
                
                try
                {
                    // Set the jukebox playlist
                    ConsoleHelper.LogInfo("Setting jukebox playlist...");
                    var setResponse = await Client.Jukebox.SetJukeboxPlaylistAsync(new List<string> { songId });
                    
                    if (setResponse.IsSuccess)
                    {
                        ConsoleHelper.LogSuccess("Successfully set jukebox playlist");
                        
                        // Get and display the playlist
                        var playlistResponse = await Client.Jukebox.GetJukeboxStatusAsync();
                        RecordTestResult(playlistResponse, "jukebox_playlist");
                        
                        if (playlistResponse.IsSuccess)
                        {
                            int entryCount = playlistResponse.JukeboxStatus?.Entry?.Count ?? 0;
                            ConsoleHelper.LogSuccess($"Retrieved jukebox playlist with {entryCount} entries");
                            
                            if (entryCount > 0 && playlistResponse.JukeboxStatus?.Entry != null)
                            {
                                var table = new Table();
                                table.AddColumn("Position");
                                table.AddColumn("Title");
                                table.AddColumn("Artist");
                                table.AddColumn("Duration");
                                
                                int position = 0;
                                foreach (var entry in playlistResponse.JukeboxStatus.Entry)
                                {
                                    table.AddRow(
                                        (position++).ToString(),
                                        entry.Title ?? "Unknown Title",
                                        entry.Artist ?? "Unknown Artist",
                                        entry.Duration.ToString());
                                }
                                
                                AnsiConsole.Write(table);
                            }
                            
                            // Try a control operation
                            ConsoleHelper.LogInfo("Testing jukebox control (start)...");
                            var controlResponse = await Client.Jukebox.ControlJukeboxAsync("start");
                            
                            if (controlResponse.IsSuccess)
                            {
                                ConsoleHelper.LogSuccess("Successfully started jukebox playback");
                                
                                // Stop after a brief playback
                                await Task.Delay(2000); // Play for 2 seconds
                                
                                var stopResponse = await Client.Jukebox.ControlJukeboxAsync("stop");
                                if (stopResponse.IsSuccess)
                                {
                                    ConsoleHelper.LogSuccess("Successfully stopped jukebox playback");
                                }
                                else
                                {
                                    ConsoleHelper.LogError($"Failed to stop jukebox playback: {stopResponse.Error?.Message}");
                                }
                            }
                            else
                            {
                                ConsoleHelper.LogError($"Failed to start jukebox playback: {controlResponse.Error?.Message}");
                            }
                            
                            // Clear the jukebox playlist
                            ConsoleHelper.LogInfo("Clearing jukebox playlist...");
                            var clearResponse = await Client.Jukebox.SetJukeboxPlaylistAsync(new List<string>());
                            
                            if (clearResponse.IsSuccess)
                            {
                                ConsoleHelper.LogSuccess("Successfully cleared jukebox playlist");
                            }
                            else
                            {
                                ConsoleHelper.LogError($"Failed to clear jukebox playlist: {clearResponse.Error?.Message}");
                                allTestsPassed = false;
                            }
                        }
                        else
                        {
                            ConsoleHelper.LogError($"Failed to get jukebox playlist: {playlistResponse.Error?.Message}");
                            allTestsPassed = false;
                        }
                    }
                    else
                    {
                        ConsoleHelper.LogError($"Failed to set jukebox playlist: {setResponse.Error?.Message}");
                        allTestsPassed = false;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.LogError($"Error testing jukebox operations: {ex.Message}");
                    allTestsPassed = false;
                }
            }
            
            return allTestsPassed ? TestResult.Pass : TestResult.Fail;
        }
    }
}