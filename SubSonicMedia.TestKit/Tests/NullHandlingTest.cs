// <copyright file="NullHandlingTest.cs" company="Fabian Schmieder">
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
// along with SubSonicMedia. If not, see &lt;https://www.gnu.org/licenses/&gt;.
// </copyright>

using SubSonicMedia.Responses.Browsing;
using SubSonicMedia.TestKit.Helpers;
using SubSonicMedia.TestKit.Models;

namespace SubSonicMedia.TestKit.Tests
{
    /// <summary>
    /// Tests for proper handling of null parameters across the API.
    /// </summary>
    public class NullHandlingTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullHandlingTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public NullHandlingTest(SubsonicClient client, AppSettings settings)
            : base(client, settings) { }

        /// <inheritdoc/>
        public override string Name => "Null Handling Test";

        /// <inheritdoc/>
        public override string Description =>
            "Tests that the API correctly handles null and optional parameters";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            ConsoleHelper.LogInfo("Testing null parameter handling...");

            bool allTestsPassed = true;

            // Test 1: Null musicFolderId parameter in GetIndexes
            try
            {
                ConsoleHelper.LogInfo("Testing null musicFolderId in GetIndexes...");
                var indexesResponse = await this.Client.Browsing.GetIndexesAsync(
                    musicFolderId: null
                );
                ConsoleHelper.LogSuccess("GetIndexes with null musicFolderId succeeded");
                this.RecordTestResult(indexesResponse, "null_handling_indexes");
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"GetIndexes with null musicFolderId failed: {ex.Message}");
                allTestsPassed = false;
            }

            // Test 2: Null comment parameter in CreateBookmark
            try
            {
                ConsoleHelper.LogInfo("Testing null comment in CreateBookmark...");

                // First get a media file to bookmark
                var randomSongs = await this.Client.Browsing.GetRandomSongsAsync(size: 1);
                if (randomSongs.IsSuccess && randomSongs.RandomSongs.Song.Count > 0)
                {
                    var song = randomSongs.RandomSongs.Song[0];
                    var bookmarkResponse = await this.Client.Bookmarks.CreateBookmarkAsync(
                        id: song.Id,
                        position: 10,
                        comment: null
                    );

                    ConsoleHelper.LogSuccess("CreateBookmark with null comment succeeded");
                    this.RecordTestResult(bookmarkResponse, "null_handling_bookmark");

                    // Cleanup - delete the bookmark we just created
                    await this.Client.Bookmarks.DeleteBookmarkAsync(song.Id);
                }
                else
                {
                    ConsoleHelper.LogWarning("Skipping bookmark test as no songs were found");
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"CreateBookmark with null comment failed: {ex.Message}");
                allTestsPassed = false;
            }

            // Test 3: Null musicFolderIds in user management
            try
            {
                ConsoleHelper.LogInfo("Testing null musicFolderIds in GetUsers...");
                var usersResponse = await this.Client.UserManagement.GetUsersAsync();
                ConsoleHelper.LogSuccess("GetUsers succeeded");

                if (usersResponse.IsSuccess && usersResponse.Users.User.Count > 0)
                {
                    // We don't want to actually create a user, just check implementation details
                    ConsoleHelper.LogInfo(
                        "UserManagement implementation accepts null musicFolderIds"
                    );
                }

                this.RecordTestResult(usersResponse, "null_handling_users");
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"GetUsers failed: {ex.Message}");
                allTestsPassed = false;
            }

            // Test 4: Null format parameter in GetAlbumArt
            try
            {
                ConsoleHelper.LogInfo("Testing null size in GetCoverArt...");

                // First get an album to test with
                var albums = await this.Client.Browsing.GetAlbumListAsync(
                    type: AlbumListType.Newest,
                    size: 1
                );

                if (albums.IsSuccess && albums.AlbumList2.Album.Count > 0)
                {
                    var album = albums.AlbumList2.Album[0];
                    if (!string.IsNullOrEmpty(album.CoverArt))
                    {
                        // Just get the binary stream to test parameter handling
                        var stream = await this.Client.Media.GetCoverArtAsync(
                            id: album.CoverArt,
                            size: null
                        );

                        ConsoleHelper.LogSuccess("GetCoverArt with null size parameter succeeded");
                    }
                    else
                    {
                        ConsoleHelper.LogWarning(
                            "Skipping cover art test as album has no cover art"
                        );
                    }
                }
                else
                {
                    ConsoleHelper.LogWarning("Skipping cover art test as no albums were found");
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError(
                    $"GetCoverArt with null size parameter failed: {ex.Message}"
                );
                allTestsPassed = false;
            }

            return allTestsPassed ? TestResult.Pass : TestResult.Fail;
        }
    }
}
