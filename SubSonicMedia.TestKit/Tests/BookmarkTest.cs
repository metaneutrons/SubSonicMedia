// <copyright file="BookmarkTest.cs" company="Fabian Schmieder">
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

using SubSonicMedia.Exceptions;
using SubSonicMedia.TestKit.Helpers;
using SubSonicMedia.TestKit.Models;

namespace SubSonicMedia.TestKit.Tests
{
    /// <summary>
    /// Tests bookmark capabilities of the Subsonic API.
    /// </summary>
    public class BookmarkTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public BookmarkTest(SubsonicClient client, AppSettings settings)
            : base(client, settings) { }

        /// <inheritdoc/>
        public override string Name => "Bookmark Test";

        /// <inheritdoc/>
        public override string Description => "Tests retrieving, creating, and deleting bookmarks";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            ConsoleHelper.LogInfo("Testing bookmark functionality...");
            bool allTestsPassed = true;

            // Test 1: Get Bookmarks
            ConsoleHelper.LogInfo("Testing GetBookmarks...");
            try
            {
                var bookmarksResponse = await this.Client.Bookmarks.GetBookmarksAsync();
                this.RecordTestResult(bookmarksResponse, "bookmarks_list");

                if (bookmarksResponse.IsSuccess)
                {
                    int bookmarkCount = bookmarksResponse.Bookmarks?.Bookmark?.Count ?? 0;
                    ConsoleHelper.LogSuccess($"Successfully retrieved {bookmarkCount} bookmarks");

                    if (bookmarkCount > 0 && bookmarksResponse.Bookmarks?.Bookmark != null)
                    {
                        // Display the bookmarks in a table
                        var table = new Table();
                        table.AddColumn("Entry ID");
                        table.AddColumn("Position");
                        table.AddColumn("Comment");
                        table.AddColumn("Created");
                        table.AddColumn("Changed");

                        foreach (var bookmark in bookmarksResponse.Bookmarks.Bookmark.Take(5))
                        {
                            table.AddRow(
                                bookmark.Entry?.Id ?? "N/A",
                                bookmark.Position.ToString(),
                                bookmark.Comment ?? "No comment",
                                bookmark.Created.ToString(),
                                bookmark.Changed.ToString()
                            );
                        }

                        AnsiConsole.Write(table);
                    }
                }
                else
                {
                    ConsoleHelper.LogError(
                        $"Failed to get bookmarks: {bookmarksResponse.Error?.Message}"
                    );
                    allTestsPassed = false;
                }
            }
            // Feature unavailability exceptions are now handled by TestBase
            catch (Exception ex)
            {
                // Rethrow feature unavailability exceptions to be handled by TestBase
                if (this.IsFeatureUnavailable(ex))
                {
                    throw;
                }

                ConsoleHelper.LogError($"Error getting bookmarks: {ex.Message}");
                allTestsPassed = false;
            }

            // Test 2: Create and Delete a Bookmark (only if we have media files)
            if (allTestsPassed)
            {
                ConsoleHelper.LogInfo("Testing creating a bookmark...");

                try
                {
                    // First, get a media file ID to use
                    var randomSongsResponse = await this.Client.Browsing.GetRandomSongsAsync(1);

                    if (
                        randomSongsResponse.IsSuccess
                        && randomSongsResponse.RandomSongs?.Song != null
                        && randomSongsResponse.RandomSongs.Song.Count > 0
                        && !string.IsNullOrEmpty(randomSongsResponse.RandomSongs.Song[0].Id)
                    )
                    {
                        var song = randomSongsResponse.RandomSongs.Song[0];
                        ConsoleHelper.LogInfo($"Creating bookmark for song: {song.Title}");

                        // Create a bookmark at the halfway point
                        long position = (song.Duration > 0 ? song.Duration : 60) / 2;
                        var createResponse = await this.Client.Bookmarks.CreateBookmarkAsync(
                            song.Id,
                            position,
                            "Test bookmark created by SubSonicMedia.TestKit"
                        );

                        if (createResponse.IsSuccess)
                        {
                            ConsoleHelper.LogSuccess(
                                $"Successfully created bookmark for song: {song.Title}"
                            );

                            // Delete the bookmark afterward
                            ConsoleHelper.LogInfo($"Deleting bookmark for song: {song.Title}");
                            var deleteResponse = await this.Client.Bookmarks.DeleteBookmarkAsync(
                                song.Id
                            );

                            if (deleteResponse.IsSuccess)
                            {
                                ConsoleHelper.LogSuccess("Successfully deleted bookmark");
                            }
                            else
                            {
                                ConsoleHelper.LogError(
                                    $"Failed to delete bookmark: {deleteResponse.Error?.Message}"
                                );
                                allTestsPassed = false;
                            }
                        }
                        else
                        {
                            ConsoleHelper.LogError(
                                $"Failed to create bookmark: {createResponse.Error?.Message}"
                            );
                            allTestsPassed = false;
                        }
                    }
                    else
                    {
                        ConsoleHelper.LogWarning(
                            "No media files available to test bookmark creation"
                        );
                    }
                }
                // Feature unavailability exceptions are now handled by TestBase
                catch (Exception ex)
                {
                    // Rethrow feature unavailability exceptions to be handled by TestBase
                    if (this.IsFeatureUnavailable(ex))
                    {
                        throw;
                    }

                    ConsoleHelper.LogError(
                        $"Error testing bookmark creation/deletion: {ex.Message}"
                    );
                    allTestsPassed = false;
                }
            }

            return allTestsPassed ? TestResult.Pass : TestResult.Fail;
        }
    }
}
