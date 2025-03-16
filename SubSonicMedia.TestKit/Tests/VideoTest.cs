// <copyright file="VideoTest.cs" company="Fabian Schmieder">
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

using System.Linq;
using Spectre.Console;
using SubSonicMedia.TestKit.Helpers;
using SubSonicMedia.TestKit.Models;

namespace SubSonicMedia.TestKit.Tests
{
    /// <summary>
    /// Tests video capabilities of the Subsonic API.
    /// </summary>
    public class VideoTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public VideoTest(SubsonicClient client, AppSettings settings)
            : base(client, settings)
        {
        }

        /// <inheritdoc/>
        public override string Name => "Video Test";

        /// <inheritdoc/>
        public override string Description => "Tests video features including listing videos and retrieving video info";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            bool allTestsPassed = true;
            string videoId = null;
            
            // Test 1: Get Videos
            ConsoleHelper.LogInfo("Testing GetVideos...");
            try
            {
                var videosResponse = await Client.Video.GetVideosAsync();
                RecordTestResult(videosResponse, "videos_list");
                
                if (videosResponse.IsSuccess)
                {
                    int videoCount = videosResponse.Videos?.VideoList?.Count ?? 0;
                    ConsoleHelper.LogSuccess($"Successfully retrieved {videoCount} videos");
                    
                    if (videoCount > 0 && videosResponse.Videos?.VideoList != null)
                    {
                        var table = new Table();
                        table.AddColumn("ID");
                        table.AddColumn("Title");
                        table.AddColumn("Created");
                        table.AddColumn("Duration");
                        table.AddColumn("Bitrate");
                        
                        foreach (var video in videosResponse.Videos.VideoList.Take(5))
                        {
                            table.AddRow(
                                video.Id ?? "N/A",
                                video.Title ?? "Unknown Title",
                                video.Created?.ToString() ?? "Unknown date",
                                video.Duration.ToString(),
                                video.BitRate.ToString());
                            
                            // Store first video ID for further tests
                            if (videoId == null)
                            {
                                videoId = video.Id;
                            }
                        }
                        
                        AnsiConsole.Write(table);
                    }
                }
                else
                {
                    ConsoleHelper.LogError($"Failed to get videos: {videosResponse.Error?.Message}");
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error getting videos: {ex.Message}");
                allTestsPassed = false;
            }
            
            // Test 2: Get Video Info (only if we have a video)
            if (allTestsPassed && !string.IsNullOrEmpty(videoId))
            {
                ConsoleHelper.LogInfo($"Testing GetVideoInfo for video ID: {videoId}");
                
                try
                {
                    var videoInfoResponse = await Client.Video.GetVideoInfoAsync(videoId);
                    RecordTestResult(videoInfoResponse, "video_info");
                    
                    if (videoInfoResponse.IsSuccess)
                    {
                        ConsoleHelper.LogSuccess("Successfully retrieved video info");
                        
                        var table = new Table();
                        table.AddColumn("Property");
                        table.AddColumn("Value");
                        
                        table.AddRow("Audio Tracks", videoInfoResponse.VideoInfo.AudioTrack?.Count.ToString() ?? "0");
                        table.AddRow("Captions", videoInfoResponse.VideoInfo.Captions?.Count.ToString() ?? "0");
                        table.AddRow("Conversions", videoInfoResponse.VideoInfo.Conversion?.Count.ToString() ?? "0");
                        
                        AnsiConsole.Write(table);
                        
                        // Display audio tracks if available
                        if (videoInfoResponse.VideoInfo.AudioTrack != null && videoInfoResponse.VideoInfo.AudioTrack.Count > 0)
                        {
                            ConsoleHelper.LogInfo("Audio Tracks:");
                            
                            var tracksTable = new Table();
                            tracksTable.AddColumn("ID");
                            tracksTable.AddColumn("Language Code");
                            tracksTable.AddColumn("Name");
                            
                            foreach (var track in videoInfoResponse.VideoInfo.AudioTrack)
                            {
                                tracksTable.AddRow(
                                    track.Id.ToString(),
                                    track.LanguageCode ?? "Unknown",
                                    track.Name ?? "No name");
                            }
                            
                            AnsiConsole.Write(tracksTable);
                        }
                        
                        // Display captions if available
                        if (videoInfoResponse.VideoInfo.Captions != null && videoInfoResponse.VideoInfo.Captions.Count > 0)
                        {
                            ConsoleHelper.LogInfo("Captions:");
                            
                            var captionsTable = new Table();
                            captionsTable.AddColumn("ID");
                            captionsTable.AddColumn("Name");
                            
                            foreach (var caption in videoInfoResponse.VideoInfo.Captions)
                            {
                                captionsTable.AddRow(
                                    caption.Id.ToString(),
                                    caption.Name ?? "No name");
                            }
                            
                            AnsiConsole.Write(captionsTable);
                            
                            // Test 3: Get Video Captions if available
                            ConsoleHelper.LogInfo("Testing GetCaptions...");
                            try
                            {
                                // Try to get captions for the video
                                var captionId = videoInfoResponse.VideoInfo.Captions[0].Id.ToString();
                                
                                using var captionsStream = await Client.Video.GetCaptionsAsync(captionId, "vtt");
                                
                                // Read captions data
                                using var reader = new StreamReader(captionsStream);
                                string captionsText = await reader.ReadToEndAsync();
                                
                                // Just check if we got some data
                                if (!string.IsNullOrEmpty(captionsText))
                                {
                                    ConsoleHelper.LogSuccess("Successfully retrieved video captions");
                                    
                                    // Show a sample of the captions
                                    string sample = captionsText.Length > 200 
                                        ? captionsText.Substring(0, 200) + "..." 
                                        : captionsText;
                                    
                                    ConsoleHelper.LogInfo($"Captions sample: {sample}");
                                    
                                    // We could save the captions to a file if needed
                                    if (Settings.RecordTestResults)
                                    {
                                        string captionsPath = Path.Combine(Settings.OutputDirectory, "video_captions.vtt");
                                        File.WriteAllText(captionsPath, captionsText);
                                        ConsoleHelper.LogInfo($"Captions saved to: {captionsPath}");
                                    }
                                }
                                else
                                {
                                    ConsoleHelper.LogWarning("Retrieved empty captions data");
                                }
                            }
                            catch (Exception ex)
                            {
                                ConsoleHelper.LogWarning($"Error getting captions: {ex.Message}");
                                // Not considering this a failure as captions may not be available
                            }
                        }
                    }
                    else
                    {
                        ConsoleHelper.LogError($"Failed to get video info: {videoInfoResponse.Error?.Message}");
                        allTestsPassed = false;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.LogError($"Error getting video info: {ex.Message}");
                    allTestsPassed = false;
                }
            }
            
            return allTestsPassed ? TestResult.Pass : TestResult.Fail;
        }
    }
}