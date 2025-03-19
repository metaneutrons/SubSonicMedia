// <copyright file="UserTest.cs" company="Fabian Schmieder">
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
    /// Tests user-related capabilities of the Subsonic API.
    /// </summary>
    public class UserTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public UserTest(SubsonicClient client, AppSettings settings)
            : base(client, settings) { }

        /// <inheritdoc/>
        public override string Name => "User Test";

        /// <inheritdoc/>
        public override string Description =>
            "Tests user features including avatars and user management";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            bool allTestsPassed = true;

            // Test 1: Get current user details
            ConsoleHelper.LogInfo("Testing GetUser...");
            try
            {
                var userResponse = await this.Client.User.GetUserAsync(this.Settings.Username);
                this.RecordTestResult(userResponse, "user_details");

                if (userResponse.IsSuccess)
                {
                    ConsoleHelper.LogSuccess(
                        $"Successfully retrieved user details for: {userResponse.User.Username}"
                    );

                    var table = new Table();
                    table.AddColumn("Property");
                    table.AddColumn("Value");

                    table.AddRow("Username", userResponse.User.Username ?? "Unknown");
                    table.AddRow("Email", userResponse.User.Email ?? "N/A");
                    table.AddRow("Is Admin", userResponse.User.IsAdmin.ToString());
                    table.AddRow("Download Role", userResponse.User.DownloadRole.ToString());
                    table.AddRow("Upload Role", userResponse.User.UploadRole.ToString());
                    table.AddRow("Playlist Role", userResponse.User.PlaylistRole.ToString());
                    table.AddRow("Jukebox Role", userResponse.User.JukeboxRole.ToString());

                    if (
                        userResponse.User.FolderIds != null
                        && userResponse.User.FolderIds.Length > 0
                    )
                    {
                        table.AddRow("Folders", string.Join(", ", userResponse.User.FolderIds));
                    }

                    AnsiConsole.Write(table);
                }
                else
                {
                    ConsoleHelper.LogError(
                        $"Failed to get user details: {userResponse.Error?.Message}"
                    );
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error getting user details: {ex.Message}");
                allTestsPassed = false;
            }

            // Test 2: Get all users (requires admin privileges)
            ConsoleHelper.LogInfo("Testing GetUsers (requires admin privileges)...");
            try
            {
                var usersResponse = await this.Client.UserManagement.GetUsersAsync();
                this.RecordTestResult(usersResponse, "users_list");

                if (usersResponse.IsSuccess)
                {
                    int userCount = usersResponse.Users?.User?.Count ?? 0;
                    ConsoleHelper.LogSuccess($"Successfully retrieved {userCount} users");

                    if (userCount > 0 && usersResponse.Users?.User != null)
                    {
                        var table = new Table();
                        table.AddColumn("Username");
                        table.AddColumn("Email");
                        table.AddColumn("Admin");
                        table.AddColumn("Settings");

                        foreach (var user in usersResponse.Users.User.Take(5))
                        {
                            table.AddRow(
                                user.Username ?? "Unknown",
                                user.Email ?? "N/A",
                                user.AdminRole.ToString(),
                                $"DL:{user.DownloadRole}, UL:{user.UploadRole}, PL:{user.PlaylistRole}"
                            );
                        }

                        AnsiConsole.Write(table);
                    }
                }
                else
                {
                    ConsoleHelper.LogWarning(
                        $"Could not get all users (likely not an admin): {usersResponse.Error?.Message}"
                    );
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error getting all users: {ex.Message}");

                // Not considering this a test failure as it might require admin privileges
            }

            // Test 3: Get avatar
            ConsoleHelper.LogInfo("Testing GetAvatar...");
            try
            {
                string username = this.Settings.Username;
                ConsoleHelper.LogInfo($"Retrieving avatar for user: {username}");

                using (var avatarStream = await this.Client.User.GetAvatarAsync(username))
                {
                    // Save avatar to file if recording test results
                    if (this.Settings.RecordTestResults)
                    {
                        string avatarPath = Path.Combine(
                            this.Settings.OutputDirectory,
                            $"user_avatar_{username}.jpg"
                        );

                        using (var fileStream = File.Create(avatarPath))
                        {
                            await avatarStream.CopyToAsync(fileStream);
                        }

                        ConsoleHelper.LogSuccess($"Avatar saved to: {avatarPath}");
                    }
                    else
                    {
                        // Just read the stream to verify it works
                        using var memoryStream = new MemoryStream();
                        await avatarStream.CopyToAsync(memoryStream);
                        int size = (int)memoryStream.Length;

                        ConsoleHelper.LogSuccess($"Successfully retrieved avatar ({size} bytes)");
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogWarning(
                    $"Error getting avatar (this is normal if the user doesn't have an avatar): {ex.Message}"
                );

                // Not considering this a test failure as the user might not have an avatar
            }

            return allTestsPassed ? TestResult.Pass : TestResult.Fail;
        }
    }
}
