// <copyright file="SearchTest.cs" company="Fabian Schmieder">
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
    /// Tests the search capabilities of the Subsonic API.
    /// </summary>
    public class SearchTest : TestBase
    {
        private readonly string _searchQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        /// <param name="searchQuery">The search query to use for testing. If null, a default query will be used.</param>
        public SearchTest(SubsonicClient client, AppSettings settings, string? searchQuery = null)
            : base(client, settings)
        {
            // Use a default query if none was provided
            this._searchQuery = string.IsNullOrEmpty(searchQuery) ? "Keith" : searchQuery;
        }

        /// <inheritdoc/>
        public override string Name => "Search Test";

        /// <inheritdoc/>
        public override string Description =>
            $"Tests search functionality using query: '{this._searchQuery}'";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            bool allTestsPassed = true;

            // Test 1: Basic Search3
            ConsoleHelper.LogInfo($"Testing Search3 with query: '{this._searchQuery}'...");
            try
            {
                var searchResponse = await this.Client.Search.Search3Async(this._searchQuery);
                this.RecordTestResult(searchResponse, "search_results");

                if (searchResponse.IsSuccess && searchResponse.SearchResult != null)
                {
                    // Get counts of each result type
                    int artistCount = searchResponse.SearchResult.Artists?.Count ?? 0;
                    int albumCount = searchResponse.SearchResult.Albums?.Count ?? 0;
                    int songCount = searchResponse.SearchResult.Songs?.Count ?? 0;

                    ConsoleHelper.LogSuccess(
                        $"Search successful: Found {artistCount} artists, {albumCount} albums, and {songCount} songs"
                    );

                    // Display artists if found
                    if (artistCount > 0 && searchResponse.SearchResult.Artists != null)
                    {
                        ConsoleHelper.LogInfo("Artists found:");
                        var table = new Table();
                        table.AddColumn("Name");
                        table.AddColumn("Albums");

                        foreach (var artist in searchResponse.SearchResult.Artists.Take(5))
                        {
                            table.AddRow(artist.Name ?? "Unknown Artist", "N/A");
                        }

                        AnsiConsole.Write(table);
                    }

                    // Display albums if found
                    if (albumCount > 0 && searchResponse.SearchResult.Albums != null)
                    {
                        ConsoleHelper.LogInfo("Albums found:");
                        var table = new Table();
                        table.AddColumn("Album");
                        table.AddColumn("Artist");
                        table.AddColumn("Year");

                        foreach (var album in searchResponse.SearchResult.Albums.Take(5))
                        {
                            table.AddRow(
                                album.Name ?? "Unknown Album",
                                album.Artist ?? "Unknown Artist",
                                album.Year.ToString() ?? "0"
                            );
                        }

                        AnsiConsole.Write(table);
                    }

                    // Display songs if found
                    if (songCount > 0 && searchResponse.SearchResult.Songs != null)
                    {
                        ConsoleHelper.LogInfo("Songs found:");
                        var table = new Table();
                        table.AddColumn("Title");
                        table.AddColumn("Artist");
                        table.AddColumn("Album");
                        table.AddColumn("Duration");

                        foreach (var song in searchResponse.SearchResult.Songs.Take(5))
                        {
                            // Format duration from seconds to mm:ss
                            string duration = "N/A";
                            if (song.Duration > 0)
                            {
                                TimeSpan time = TimeSpan.FromSeconds(song.Duration);
                                duration = $"{(int)time.TotalMinutes}:{time.Seconds:D2}";
                            }

                            table.AddRow(
                                song.Title ?? "Unknown Title",
                                song.Artist ?? "Unknown Artist",
                                song.Album ?? "Unknown Album",
                                duration
                            );
                        }

                        AnsiConsole.Write(table);
                    }

                    // If no results were found, consider it a warning but not a failure
                    if (artistCount == 0 && albumCount == 0 && songCount == 0)
                    {
                        ConsoleHelper.LogWarning(
                            $"No results found for query: '{this._searchQuery}'"
                        );
                    }
                }
                else
                {
                    ConsoleHelper.LogError($"Search failed: {searchResponse.Error?.Message}");
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
                ConsoleHelper.LogError($"Error during search: {ex.Message}");
                allTestsPassed = false;
            }

            return allTestsPassed ? TestResult.Pass : TestResult.Fail;
        }
    }
}
