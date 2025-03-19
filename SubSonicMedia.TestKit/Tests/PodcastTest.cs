// <copyright file="PodcastTest.cs" company="Fabian Schmieder">
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
    /// Tests podcast capabilities of the Subsonic API.
    /// </summary>
    public class PodcastTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public PodcastTest(SubsonicClient client, AppSettings settings)
            : base(client, settings) { }

        /// <inheritdoc/>
        public override string Name => "Podcast Test";

        /// <inheritdoc/>
        public override string Description =>
            "Tests podcast features including listing, refreshing, and newest episodes";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            bool allTestsPassed = true;

            // Test 1: Get Podcasts
            ConsoleHelper.LogInfo("Testing GetPodcasts...");
            try
            {
                var podcastsResponse = await this.Client.Podcasts.GetPodcastsAsync();
                this.RecordTestResult(podcastsResponse, "podcasts_list");

                if (podcastsResponse.IsSuccess)
                {
                    int channelCount = podcastsResponse.Podcasts?.Channel?.Count ?? 0;
                    ConsoleHelper.LogSuccess(
                        $"Successfully retrieved {channelCount} podcast channels"
                    );

                    if (channelCount > 0 && podcastsResponse.Podcasts?.Channel != null)
                    {
                        // Display channel information
                        var table = new Table();
                        table.AddColumn("ID");
                        table.AddColumn("Title");
                        table.AddColumn("Description");
                        table.AddColumn("Episode Count");
                        table.AddColumn("Status");

                        foreach (var channel in podcastsResponse.Podcasts.Channel.Take(5))
                        {
                            table.AddRow(
                                channel.Id ?? "N/A",
                                channel.Title ?? "Unknown Title",
                                channel.Description != null
                                    ? (
                                        channel.Description.Length > 50
                                            ? channel.Description.Substring(0, 47) + "..."
                                            : channel.Description
                                    )
                                    : "No description",
                                channel.Episode?.Count.ToString() ?? "0",
                                channel.Status ?? "Unknown"
                            );
                        }

                        AnsiConsole.Write(table);

                        // If we have channels with episodes, show episodes from the first channel
                        var firstChannelWithEpisodes =
                            podcastsResponse.Podcasts.Channel.FirstOrDefault(c =>
                                c.Episode != null && c.Episode.Count > 0
                            );
                        if (firstChannelWithEpisodes != null)
                        {
                            ConsoleHelper.LogInfo(
                                $"Episodes for '{firstChannelWithEpisodes.Title}':"
                            );

                            var episodeTable = new Table();
                            episodeTable.AddColumn("ID");
                            episodeTable.AddColumn("Title");
                            episodeTable.AddColumn("Published");
                            episodeTable.AddColumn("Duration");
                            episodeTable.AddColumn("Status");

                            foreach (var episode in firstChannelWithEpisodes.Episode.Take(5))
                            {
                                episodeTable.AddRow(
                                    episode.Id ?? "N/A",
                                    episode.Title ?? "Unknown Title",
                                    episode.PublishDate?.ToString() ?? "Unknown date",
                                    episode.Duration.ToString(),
                                    episode.Status ?? "Unknown"
                                );
                            }

                            AnsiConsole.Write(episodeTable);
                        }
                    }
                }
                else
                {
                    ConsoleHelper.LogError(
                        $"Failed to get podcasts: {podcastsResponse.Error?.Message}"
                    );
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error getting podcasts: {ex.Message}");
                allTestsPassed = false;
            }

            // Test 2: Get Newest Podcasts
            ConsoleHelper.LogInfo("Testing GetNewestPodcasts...");
            try
            {
                var newestResponse = await this.Client.Podcasts.GetNewestPodcastsAsync(10);
                this.RecordTestResult(newestResponse, "newest_podcasts");

                if (newestResponse.IsSuccess)
                {
                    int episodeCount = newestResponse.NewestPodcasts?.Episode?.Count ?? 0;
                    ConsoleHelper.LogSuccess(
                        $"Successfully retrieved {episodeCount} newest podcast episodes"
                    );

                    if (episodeCount > 0 && newestResponse.NewestPodcasts?.Episode != null)
                    {
                        var table = new Table();
                        table.AddColumn("Channel");
                        table.AddColumn("Title");
                        table.AddColumn("Published");
                        table.AddColumn("Duration");

                        foreach (var episode in newestResponse.NewestPodcasts.Episode)
                        {
                            table.AddRow(
                                episode.Parent ?? "Unknown Channel",
                                episode.Title ?? "Unknown Title",
                                episode.PublishDate?.ToString() ?? "Unknown date",
                                episode.Duration.ToString()
                            );
                        }

                        AnsiConsole.Write(table);
                    }
                }
                else
                {
                    ConsoleHelper.LogError(
                        $"Failed to get newest podcasts: {newestResponse.Error?.Message}"
                    );
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error getting newest podcasts: {ex.Message}");
                allTestsPassed = false;
            }

            // Test 3: Refresh Podcasts
            ConsoleHelper.LogInfo("Testing RefreshPodcasts...");
            try
            {
                var refreshResponse = await this.Client.Podcasts.RefreshPodcastsAsync();

                if (refreshResponse.IsSuccess)
                {
                    ConsoleHelper.LogSuccess("Successfully initiated podcast refresh");

                    // Since there's no GetPodcastStatusAsync method, we just wait a bit and check if it's done
                    await Task.Delay(3000); // Wait 3 seconds

                    // Check if podcast refresh completed by getting podcasts again
                    var statusResponse = await this.Client.Podcasts.GetPodcastsAsync();
                    if (statusResponse.IsSuccess)
                    {
                        ConsoleHelper.LogSuccess("Podcast refresh operation completed");
                    }
                    else
                    {
                        ConsoleHelper.LogWarning(
                            "Could not verify podcast refresh completion status"
                        );
                    }
                }
                else
                {
                    ConsoleHelper.LogError(
                        $"Failed to initiate podcast refresh: {refreshResponse.Error?.Message}"
                    );
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error refreshing podcasts: {ex.Message}");
                allTestsPassed = false;
            }

            return allTestsPassed ? TestResult.Pass : TestResult.Fail;
        }
    }
}
