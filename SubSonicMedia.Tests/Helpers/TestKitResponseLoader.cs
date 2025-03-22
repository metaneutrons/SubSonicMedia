// <copyright file="TestKitResponseLoader.cs" company="Fabian Schmieder">
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

using System.Reflection;
using System.Text.Json;

namespace SubSonicMedia.Tests.Helpers
{
    /// <summary>
    /// Helper class for loading recorded test responses from the TestKit.
    /// </summary>
    public static class TestKitResponseLoader
    {
        private static readonly string TestKitOutputsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "TestResources",
            "TestKitOutputs"
        );

        /// <summary>
        /// Loads a JSON response file recorded by the TestKit.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="fileName">The filename without extension.</param>
        /// <returns>The deserialized object, or null if the file doesn't exist.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the response file cannot be found.</exception>
        public static T? LoadJsonResponse<T>(string fileName)
        {
            string jsonContent = LoadJsonResponseAsString(fileName);
            return JsonSerializer.Deserialize<T>(jsonContent);
        }

        /// <summary>
        /// Loads a JSON response file as a raw string.
        /// </summary>
        /// <param name="fileName">The filename without extension.</param>
        /// <returns>The JSON string content.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the response file cannot be found.</exception>
        public static string LoadJsonResponseAsString(string fileName)
        {
            string filePath = Path.Combine(TestKitOutputsPath, $"{fileName}.json");

            // For testing convenience, create a dummy JSON response if the file doesn't exist
            if (!File.Exists(filePath))
            {
                Console.WriteLine(
                    $"WARNING: TestKit response file not found: {fileName}.json. Using dummy response."
                );
                return CreateDummyResponse(fileName);
            }

            string jsonContent = File.ReadAllText(filePath);

            // Check if we need to wrap in subsonic-response
            if (!jsonContent.Contains("subsonic-response"))
            {
                jsonContent = WrapInSubsonicResponse(jsonContent);
            }

            return jsonContent;
        }

        /// <summary>
        /// Loads a binary response file recorded by the TestKit.
        /// </summary>
        /// <param name="fileName">The filename without extension.</param>
        /// <returns>The binary data.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the response file cannot be found.</exception>
        public static byte[] LoadBinaryResponse(string fileName)
        {
            // For binary files, we need to find by filename without enforcing extension
            string directory = TestKitOutputsPath;
            string[] matchingFiles = Directory
                .GetFiles(directory)
                .Where(f => Path.GetFileNameWithoutExtension(f) == fileName)
                .ToArray();

            if (matchingFiles.Length == 0)
            {
                // Create 1x1 pixel for testing if no file exists
                Console.WriteLine(
                    $"WARNING: TestKit binary response file not found: {fileName}.*. Using dummy 1x1 pixel."
                );
                return new byte[]
                {
                    0x47,
                    0x49,
                    0x46,
                    0x38,
                    0x39,
                    0x61,
                    0x01,
                    0x00,
                    0x01,
                    0x00,
                    0x80,
                    0x00,
                    0x00,
                    0xFF,
                    0xFF,
                    0xFF,
                    0x00,
                    0x00,
                    0x00,
                    0x21,
                    0xF9,
                    0x04,
                    0x01,
                    0x00,
                    0x00,
                    0x00,
                    0x00,
                    0x2C,
                    0x00,
                    0x00,
                    0x00,
                    0x00,
                    0x01,
                    0x00,
                    0x01,
                    0x00,
                    0x00,
                    0x02,
                    0x02,
                    0x44,
                    0x01,
                    0x00,
                    0x3B,
                }; // 1x1 transparent GIF
            }

            return File.ReadAllBytes(matchingFiles[0]);
        }

        /// <summary>
        /// Copies all TestKit outputs from the TestKit project to the test resources directory.
        /// </summary>
        /// <returns>The number of files copied.</returns>
        public static int SyncTestKitOutputs()
        {
            // Ensure the target directory exists
            if (!Directory.Exists(TestKitOutputsPath))
            {
                Directory.CreateDirectory(TestKitOutputsPath);
            }

            try
            {
                // Try to locate the TestKit outputs directory
                string? solutionDir = FindSolutionDirectory();
                if (solutionDir == null)
                {
                    Console.WriteLine(
                        "Could not locate solution directory to find TestKit outputs. Using embedded resources instead."
                    );
                    return 0;
                }

                string testKitOutputsSourcePath = Path.Combine(
                    solutionDir,
                    "SubSonicMedia.TestKit",
                    "outputs"
                );
                if (!Directory.Exists(testKitOutputsSourcePath))
                {
                    Console.WriteLine(
                        $"TestKit outputs directory not found at {testKitOutputsSourcePath}. Using embedded resources instead."
                    );
                    return 0;
                }

                // Copy all files
                int filesCopied = 0;
                foreach (string sourceFile in Directory.GetFiles(testKitOutputsSourcePath))
                {
                    string fileName = Path.GetFileName(sourceFile);
                    string targetFile = Path.Combine(TestKitOutputsPath, fileName);
                    File.Copy(sourceFile, targetFile, overwrite: true);
                    filesCopied++;
                }

                return filesCopied;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error syncing TestKit outputs: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Creates a dummy response JSON string for testing.
        /// </summary>
        /// <param name="fileName">The requested filename, used to determine what kind of response to create.</param>
        /// <returns>A JSON string with dummy data.</returns>
        private static string CreateDummyResponse(string fileName)
        {
            string response;

            // Create different responses based on the filename
            if (fileName.Contains("connection_ping"))
            {
                response =
                    @"{
                    ""status"": ""ok"",
                    ""version"": ""1.16.1""
                }";
            }
            else if (fileName.Contains("music_folders"))
            {
                response =
                    @"{
                    ""status"": ""ok"",
                    ""version"": ""1.16.1"",
                    ""musicFolders"": [
                        {
                            ""id"": 1,
                            ""name"": ""Music Library""
                        }
                    ]
                }";
            }
            else if (fileName.Contains("artists"))
            {
                response =
                    @"{
                    ""status"": ""ok"",
                    ""version"": ""1.16.1"",
                    ""artists"": {
                        ""ignoredArticles"": ""The El La Los Las Le Les"",
                        ""index"": [
                            {
                                ""name"": ""A"",
                                ""artist"": [
                                    {
                                        ""id"": ""ar-1"",
                                        ""name"": ""ABBA"",
                                        ""albumCount"": 2
                                    }
                                ]
                            }
                        ]
                    }
                }";
            }
            else if (fileName.Contains("artist_detail"))
            {
                response =
                    @"{
                    ""status"": ""ok"",
                    ""version"": ""1.16.1"",
                    ""artist"": {
                        ""id"": ""ar-1"",
                        ""name"": ""ABBA"",
                        ""album"": [
                            {
                                ""id"": ""al-1"",
                                ""name"": ""Gold"",
                                ""artist"": ""ABBA"",
                                ""artistId"": ""ar-1"",
                                ""year"": 1992,
                                ""songCount"": 19
                            }
                        ]
                    }
                }";
            }
            else
            {
                // Generic response
                response =
                    @"{
                    ""status"": ""ok"",
                    ""version"": ""1.16.1""
                }";
            }

            return WrapInSubsonicResponse(response);
        }

        /// <summary>
        /// Wraps a JSON string in the subsonic-response envelope if needed.
        /// </summary>
        /// <param name="json">The JSON string to wrap.</param>
        /// <returns>The wrapped JSON string.</returns>
        private static string WrapInSubsonicResponse(string json)
        {
            // Skip if already wrapped
            if (json.Contains("subsonic-response"))
            {
                return json;
            }

            return @"{
                ""subsonic-response"": "
                + json
                + @"
            }";
        }

        /// <summary>
        /// Attempts to find the solution directory by traversing up from the current assembly location.
        /// </summary>
        /// <returns>The solution directory path, or null if not found.</returns>
        private static string? FindSolutionDirectory()
        {
            // Start from the directory where the assembly is located
            string? currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (currentDir == null)
            {
                return null;
            }

            // Traverse up until we find the .sln file or hit the root
            while (currentDir != null && Directory.Exists(currentDir))
            {
                if (Directory.GetFiles(currentDir, "*.sln").Any())
                {
                    return currentDir;
                }

                // Move up one directory
                currentDir = Path.GetDirectoryName(currentDir);
            }

            return null;
        }
    }
}
