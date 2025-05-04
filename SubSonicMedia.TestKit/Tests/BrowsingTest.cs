// <copyright file="BrowsingTest.cs" company="Fabian Schmieder">
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
    /// Tests browsing capabilities of the Subsonic API.
    /// </summary>
    public class BrowsingTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsingTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public BrowsingTest(SubsonicClient client, AppSettings settings)
            : base(client, settings) { }

        /// <inheritdoc/>
        public override string Name => "Browsing Test";

        /// <inheritdoc/>
        public override string Description => "Tests browsing music folders, artists, and albums";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            bool allTestsPassed = true;

            // Test 1: Get Music Folders
            ConsoleHelper.LogInfo("Testing GetMusicFolders...");
            try
            {
                var foldersResponse = await this.Client.Browsing.GetMusicFoldersAsync();
                this.RecordTestResult(foldersResponse, "browsing_music_folders");

                if (foldersResponse.IsSuccess)
                {
                    int folderCount = foldersResponse.MusicFolders?.Count ?? 0;
                    ConsoleHelper.LogSuccess($"Successfully retrieved {folderCount} music folders");

                    if (folderCount > 0 && foldersResponse.MusicFolders != null)
                    {
                        // Display the first few folders in a table
                        var table = new Table();
                        table.AddColumn("ID");
                        table.AddColumn("Name");

                        foreach (var folder in foldersResponse.MusicFolders.Take(5))
                        {
                            table.AddRow(folder.Id.ToString(), folder.Name ?? "Unnamed Folder");
                        }

                        AnsiConsole.Write(table);
                    }
                }
                else
                {
                    ConsoleHelper.LogError(
                        $"Failed to get music folders: {foldersResponse.Error?.Message}"
                    );
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                // Rethrow feature unavailability exceptions to be handled by TestBase
                if (this.IsFeatureUnavailable(ex))
                {
                    throw;
                }

                ConsoleHelper.LogError($"Error getting music folders: {ex.Message}");
                allTestsPassed = false;
            }

            // Test 2: Get Artists
            ConsoleHelper.LogInfo("Testing GetArtists...");
            try
            {
                var artistsResponse = await this.Client.Browsing.GetArtistsAsync();
                this.RecordTestResult(artistsResponse, "browsing_artists");

                if (artistsResponse.IsSuccess)
                {
                    // Count all artists across all indexes
                    int artistCount = 0;
                    if (artistsResponse.Artists?.Index != null)
                    {
                        foreach (var index in artistsResponse.Artists.Index)
                        {
                            artistCount += index.Artist?.Count ?? 0;
                        }
                    }

                    ConsoleHelper.LogSuccess($"Successfully retrieved {artistCount} artists");

                    // Display a few artists if available
                    if (
                        artistsResponse.Artists?.Index != null
                        && artistsResponse.Artists.Index.Count > 0
                    )
                    {
                        var firstIndex = artistsResponse.Artists.Index.FirstOrDefault();
                        if (firstIndex?.Artist != null && firstIndex.Artist.Count > 0)
                        {
                            ConsoleHelper.LogInfo(
                                $"Sample artists from index '{firstIndex.Name}':"
                            );

                            var table = new Table();
                            table.AddColumn("ID");
                            table.AddColumn("Name");
                            table.AddColumn("Album Count");

                            foreach (var artist in firstIndex.Artist.Take(5))
                            {
                                table.AddRow(
                                    artist.Id ?? "N/A",
                                    artist.Name ?? "Unknown Artist",
                                    artist.AlbumCount.ToString()
                                );
                            }

                            AnsiConsole.Write(table);
                        }
                    }
                }
                else
                {
                    ConsoleHelper.LogError(
                        $"Failed to get artists: {artistsResponse.Error?.Message}"
                    );
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                // Rethrow feature unavailability exceptions to be handled by TestBase
                if (this.IsFeatureUnavailable(ex))
                {
                    throw;
                }
                ConsoleHelper.LogError($"Error getting artists: {ex.Message}");
                allTestsPassed = false;
            }

            // Test 3: If we have artists, try to get albums for the first artist
            if (allTestsPassed)
            {
                ConsoleHelper.LogInfo("Testing GetArtist (with albums)...");
                try
                {
                    var artistsResponse = await this.Client.Browsing.GetArtistsAsync();

                    if (
                        artistsResponse.IsSuccess
                        && artistsResponse.Artists?.Index != null
                        && artistsResponse.Artists.Index.Count > 0
                    )
                    {
                        // Find the first artist with an ID
                        var firstArtist = artistsResponse
                            .Artists.Index.SelectMany(i =>
                                i.Artist ?? new List<Responses.Browsing.Artist>()
                            )
                            .FirstOrDefault(a => !string.IsNullOrEmpty(a.Id));

                        if (firstArtist != null)
                        {
                            ConsoleHelper.LogInfo(
                                $"Getting details for artist: {firstArtist.Name}"
                            );

                            var artistResponse = await this.Client.Browsing.GetArtistAsync(
                                firstArtist.Id!
                            );
                            this.RecordTestResult(artistResponse, "browsing_artist_detail");

                            if (artistResponse.IsSuccess && artistResponse.Artist != null)
                            {
                                int albumCount = artistResponse.Artist.Album?.Count ?? 0;
                                ConsoleHelper.LogSuccess(
                                    $"Successfully retrieved {albumCount} albums for {firstArtist.Name}"
                                );

                                if (albumCount > 0 && artistResponse.Artist.Album != null)
                                {
                                    ConsoleHelper.LogInfo($"Sample albums for {firstArtist.Name}:");

                                    var table = new Table();
                                    table.AddColumn("ID");
                                    table.AddColumn("Name");
                                    table.AddColumn("Year");
                                    table.AddColumn("Song Count");

                                    foreach (var album in artistResponse.Artist.Album.Take(5))
                                    {
                                        table.AddRow(
                                            album.Id ?? "N/A",
                                            album.Name ?? "Unknown Album",
                                            album.Year.ToString() ?? "0",
                                            album.SongCount.ToString() ?? "0"
                                        );
                                    }

                                    AnsiConsole.Write(table);
                                }
                            }
                            else
                            {
                                ConsoleHelper.LogError(
                                    $"Failed to get artist details: {artistResponse.Error?.Message}"
                                );
                                allTestsPassed = false;
                            }
                        }
                        else
                        {
                            ConsoleHelper.LogWarning(
                                "No artists with valid IDs found to test GetArtist"
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Rethrow feature unavailability exceptions to be handled by TestBase
                    if (this.IsFeatureUnavailable(ex))
                    {
                        throw;
                    }
                    ConsoleHelper.LogError($"Error getting artist details: {ex.Message}");
                    allTestsPassed = false;
                }
            }

            return allTestsPassed ? TestResult.Pass : TestResult.Fail;
        }
    }
}
