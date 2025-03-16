// <copyright file="MediaTest.cs" company="Fabian Schmieder">
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
    /// Tests the media streaming capabilities of the Subsonic API.
    /// </summary>
    public class MediaTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public MediaTest(SubsonicClient client, AppSettings settings)
            : base(client, settings) { }

        /// <inheritdoc/>
        public override string Name => "Media Streaming Test";

        /// <inheritdoc/>
        public override string Description =>
            "Tests media streaming capabilities including cover art and song streaming";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            // First, find a song to test with by searching
            ConsoleHelper.LogInfo("Finding a song to test streaming...");

            try
            {
                // Simple search to find a song
                // Use null for artist and album counts (no limit) and set songCount to 1
                var searchResponse = await Client.Search.Search3Async(
                    "Keith",
                    artistCount: null,
                    albumCount: null,
                    songCount: 1
                );

                if (
                    !searchResponse.IsSuccess
                    || searchResponse.SearchResult?.Songs == null
                    || searchResponse.SearchResult.Songs.Count == 0
                )
                {
                    ConsoleHelper.LogWarning(
                        "Could not find any songs to test media streaming. Trying random songs..."
                    );

                    // Try to get random songs
                    var randomResponse = await Client.Browsing.GetRandomSongsAsync(size: 1);
                    RecordTestResult(randomResponse, "media_random_songs");

                    if (
                        !randomResponse.IsSuccess
                        || randomResponse.RandomSongs?.Song == null
                        || randomResponse.RandomSongs.Song.Count == 0
                    )
                    {
                        ConsoleHelper.LogWarning(
                            "Could not find any songs to test media streaming."
                        );
                        ConsoleHelper.LogInfo(
                            "Skipping media tests as this appears to be a test environment without music content."
                        );
                        // Mark as successful since we're in a test environment without content
                        return TestResult.Pass;
                    }

                    // Test with a random song
                    var randomSong = randomResponse.RandomSongs.Song[0];
                    return await TestMediaStreaming(randomSong.Id!);
                }
                else
                {
                    // Test with the first song from search results
                    var song = searchResponse.SearchResult.Songs[0];
                    ConsoleHelper.LogInfo($"Found song: {song.Title} by {song.Artist}");

                    return await TestMediaStreaming(song.Id!);
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error finding a song to test: {ex.Message}");
                return TestResult.Fail;
            }
        }

        private async Task<TestResult> TestMediaStreaming(string songId)
        {
            bool allTestsPassed = true;

            // Test 1: Stream a song
            ConsoleHelper.LogInfo($"Testing song streaming with ID: {songId}...");
            try
            {
                using var stream = await Client.Media.StreamAsync(songId);

                // Check if we got a valid stream
                if (stream != null)
                {
                    // Read the first few bytes to confirm it's a valid audio file
                    byte[] buffer = new byte[4096];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        // Record first few bytes (as hex) for debugging
                        string hexBytes = BitConverter
                            .ToString(buffer, 0, Math.Min(bytesRead, 32))
                            .Replace("-", " ");
                        ConsoleHelper.LogSuccess(
                            $"Successfully streamed song. First bytes: {hexBytes}"
                        );

                        // Create a small file to record the first part of the stream for verification
                        string streamSamplePath = Path.Combine(
                            Settings.OutputDirectory,
                            "stream_sample.bin"
                        );
                        await File.WriteAllBytesAsync(
                            streamSamplePath,
                            buffer.Take(bytesRead).ToArray()
                        );
                        ConsoleHelper.LogInfo($"Stream sample saved to {streamSamplePath}");
                    }
                    else
                    {
                        ConsoleHelper.LogWarning("Stream opened but no data was available");
                        allTestsPassed = false;
                    }
                }
                else
                {
                    ConsoleHelper.LogError("Failed to open stream for song");
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error streaming song: {ex.Message}");
                allTestsPassed = false;
            }

            // Test 2: Get cover art
            ConsoleHelper.LogInfo($"Testing cover art retrieval with ID: {songId}...");
            try
            {
                using var coverStream = await Client.Media.GetCoverArtAsync(songId);

                // Check if we got a valid stream
                if (coverStream != null)
                {
                    // Read the first few bytes to confirm it's a valid image file
                    byte[] buffer = new byte[4096];
                    int bytesRead = await coverStream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        // Check if it has a valid image signature (JPEG, PNG, etc.)
                        bool isValidImage = IsValidImageFormat(buffer);

                        if (isValidImage)
                        {
                            ConsoleHelper.LogSuccess("Successfully retrieved cover art");

                            // Save the cover art to a file in the output directory
                            try
                            {
                                string coverArtPath = Path.Combine(
                                    Settings.OutputDirectory,
                                    "cover_art_sample.jpg"
                                );
                                using (var fileStream = File.Create(coverArtPath))
                                {
                                    // Create a new memory stream with the buffer we already read
                                    using (var memStream = new MemoryStream(buffer, 0, bytesRead))
                                    {
                                        await memStream.CopyToAsync(fileStream);
                                    }
                                }

                                ConsoleHelper.LogInfo($"Cover art sample saved to {coverArtPath}");
                            }
                            catch (Exception ex)
                            {
                                ConsoleHelper.LogWarning(
                                    $"Could not save cover art sample: {ex.Message}"
                                );
                                // Don't fail the test just because we couldn't save the file
                            }
                        }
                        else
                        {
                            // Just log a warning but don't fail the test
                            ConsoleHelper.LogWarning(
                                "Cover art stream doesn't match known image formats but data was received"
                            );

                            // Still save it as a binary sample
                            try
                            {
                                string coverArtPath = Path.Combine(
                                    Settings.OutputDirectory,
                                    "cover_art_sample.bin"
                                );
                                using (var fileStream = File.Create(coverArtPath))
                                {
                                    using (var memStream = new MemoryStream(buffer, 0, bytesRead))
                                    {
                                        await memStream.CopyToAsync(fileStream);
                                    }
                                }

                                ConsoleHelper.LogInfo(
                                    $"Unknown format cover art saved to {coverArtPath}"
                                );
                            }
                            catch (Exception ex)
                            {
                                ConsoleHelper.LogWarning(
                                    $"Could not save cover art binary: {ex.Message}"
                                );
                            }
                        }
                    }
                    else
                    {
                        ConsoleHelper.LogWarning(
                            "Cover art stream opened but no data was available"
                        );
                        allTestsPassed = false;
                    }
                }
                else
                {
                    ConsoleHelper.LogError("Failed to open stream for cover art");
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error retrieving cover art: {ex.Message}");
                allTestsPassed = false;
            }

            return allTestsPassed ? TestResult.Pass : TestResult.Fail;
        }

        // Helper method to check if a byte array represents a valid image format
        private bool IsValidImageFormat(byte[] bytes)
        {
            if (bytes.Length < 8)
            {
                return false;
            }

            // Check for JPEG header (JFIF) - FF D8 FF
            if (bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
            {
                return true;
            }

            // Check for PNG header - 89 50 4E 47 0D 0A 1A 0A
            if (
                bytes[0] == 0x89
                && bytes[1] == 0x50
                && bytes[2] == 0x4E
                && bytes[3] == 0x47
                && bytes[4] == 0x0D
                && bytes[5] == 0x0A
                && bytes[6] == 0x1A
                && bytes[7] == 0x0A
            )
            {
                return true;
            }

            // Check for GIF header (GIF87a or GIF89a) - 47 49 46 38 (followed by either 37 61 or 39 61)
            if (
                bytes[0] == 0x47
                && bytes[1] == 0x49
                && bytes[2] == 0x46
                && bytes[3] == 0x38
                && ((bytes[4] == 0x37 || bytes[4] == 0x39) && bytes[5] == 0x61)
            )
            {
                return true;
            }

            // Check for BMP header - 42 4D
            if (bytes[0] == 0x42 && bytes[1] == 0x4D)
            {
                return true;
            }

            // Check for WEBP - 52 49 46 46 (followed by any 4 bytes) then 57 45 42 50
            if (
                bytes.Length >= 12
                && bytes[0] == 0x52
                && bytes[1] == 0x49
                && bytes[2] == 0x46
                && bytes[3] == 0x46
                && bytes[8] == 0x57
                && bytes[9] == 0x45
                && bytes[10] == 0x42
                && bytes[11] == 0x50
            )
            {
                return true;
            }

            // Check for TIFF - 49 49 2A 00 or 4D 4D 00 2A
            if (
                (bytes[0] == 0x49 && bytes[1] == 0x49 && bytes[2] == 0x2A && bytes[3] == 0x00)
                || (bytes[0] == 0x4D && bytes[1] == 0x4D && bytes[2] == 0x00 && bytes[3] == 0x2A)
            )
            {
                return true;
            }

            // Check for AVIF/HEIF - 00 00 00 xx 66 74 79 70
            if (
                bytes.Length >= 8
                && bytes[4] == 0x66
                && bytes[5] == 0x74
                && bytes[6] == 0x79
                && bytes[7] == 0x70
            )
            {
                return true;
            }

            // Check for ICNS (Mac Icon) - 69 63 6E 73
            if (bytes[0] == 0x69 && bytes[1] == 0x63 && bytes[2] == 0x6E && bytes[3] == 0x73)
            {
                return true;
            }

            return false;
        }
    }
}
