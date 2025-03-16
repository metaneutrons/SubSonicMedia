// <copyright file="TestBase.cs" company="Fabian Schmieder">
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

using System.Diagnostics;
using System.Text.Json;
using SubSonicMedia.TestKit.Helpers;
using SubSonicMedia.TestKit.Models;

namespace SubSonicMedia.TestKit.Tests
{
    /// <summary>
    /// Base class for all API tests.
    /// </summary>
    public abstract class TestBase
    {
        private readonly Stopwatch _stopwatch = new();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TestBase"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        protected TestBase(SubsonicClient client, AppSettings settings)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            
            // Create output directory if it doesn't exist
            if (settings.RecordTestResults && !string.IsNullOrEmpty(settings.OutputDirectory))
            {
                Directory.CreateDirectory(settings.OutputDirectory);
            }
        }
        
        /// <summary>
        /// Gets the Subsonic client.
        /// </summary>
        protected SubsonicClient Client { get; }
        
        /// <summary>
        /// Gets the application settings.
        /// </summary>
        protected AppSettings Settings { get; }
        
        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        public abstract string Name { get; }
        
        /// <summary>
        /// Gets the test description.
        /// </summary>
        public abstract string Description { get; }
        
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <returns>True if the test passes, false otherwise.</returns>
        public virtual async Task<bool> RunAsync()
        {
            ConsoleHelper.LogTestStart(Name);
            _stopwatch.Restart();
            
            bool success = false;
            
            try
            {
                success = await ExecuteTestAsync();
                return success;
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Test failed: {ex.Message}");
                return false;
            }
            finally
            {
                _stopwatch.Stop();
                ConsoleHelper.LogTestCompletion(Name, _stopwatch.ElapsedMilliseconds, success);
            }
        }
        
        /// <summary>
        /// Executes the test logic.
        /// </summary>
        /// <returns>True if the test passes, false otherwise.</returns>
        protected abstract Task<bool> ExecuteTestAsync();
        
        /// <summary>
        /// Records a test result to a JSON file for later use in mocking.
        /// </summary>
        /// <param name="result">The object to serialize.</param>
        /// <param name="fileName">The name of the file (without extension).</param>
        protected void RecordTestResult(object result, string fileName)
        {
            if (!Settings.RecordTestResults)
            {
                return;
            }
            
            try
            {
                string path = Path.Combine(Settings.OutputDirectory, $"{fileName}.json");
                string json = JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                
                File.WriteAllText(path, json);
                ConsoleHelper.LogInfo($"Test result recorded to {path}");
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Failed to record test result: {ex.Message}");
            }
        }
    }
}
