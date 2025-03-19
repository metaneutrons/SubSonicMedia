// <copyright file="TestBase.cs" company="Fabian Schmieder">
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
using System.Diagnostics;
using System.Text.Json;
using SubSonicMedia.Exceptions;
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
            this.Client = client ?? throw new ArgumentNullException(nameof(client));
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));

            // Create output directory if it doesn't exist
            if (settings.RecordTestResults && !string.IsNullOrEmpty(settings.OutputDirectory))
            {
                Directory.CreateDirectory(settings.OutputDirectory);
            }
        }

        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the test description.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the elapsed time of the last test run in milliseconds.
        /// </summary>
        public long ElapsedMilliseconds { get; private set; }

        /// <summary>
        /// Gets the Subsonic client.
        /// </summary>
        protected SubsonicClient Client { get; }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        protected AppSettings Settings { get; }

        /// <summary>
        /// Gets the exception that occurred during the test, if any.
        /// </summary>
        public Exception? LastException { get; private set; }

        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <returns>The test result (Pass, Fail, or Skipped).</returns>
        public virtual async Task<TestResult> RunAsync()
        {
            ConsoleHelper.LogTestStart(this.Name);
            this._stopwatch.Restart();

            TestResult result = TestResult.Fail;
            this.LastException = null;

            try
            {
                result = await this.ExecuteTestAsync();
            }
            catch (Exception ex) when (this.IsFeatureUnavailable(ex))
            {
                // Handle "NotImplemented" or "Gone" responses
                string apiName = this.GetType().Name.Replace("Test", string.Empty);
                ConsoleHelper.LogWarning(
                    $"{apiName} API not implemented by this server (possibly Navidrome)"
                );
                result = TestResult.Skipped;
                this.LastException = ex;
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Test failed: {ex.Message}");
                result = TestResult.Fail;
                this.LastException = ex;
            }
            finally
            {
                this._stopwatch.Stop();
                this.ElapsedMilliseconds = this._stopwatch.ElapsedMilliseconds;
                ConsoleHelper.LogTestCompletion(this.Name, this.ElapsedMilliseconds, result);
            }

            return result;
        }

        /// <summary>
        /// Executes the test logic.
        /// </summary>
        /// <returns>The test result (Pass, Fail, or Skipped).</returns>
        protected abstract Task<TestResult> ExecuteTestAsync();

        /// <summary>
        /// Determines if an exception indicates the test should be skipped due to feature unavailability.
        /// </summary>
        /// <param name="ex">The exception to check.</param>
        /// <returns>True if the test should be skipped, false otherwise.</returns>
        protected bool IsFeatureUnavailable(Exception ex)
        {
            // Check common error messages that indicate feature unavailability
            if (
                ex.Message.Contains("not implemented", StringComparison.OrdinalIgnoreCase)
                || ex.Message.Contains("gone", StringComparison.OrdinalIgnoreCase)
                || ex.Message.Contains("NotImplemented", StringComparison.OrdinalIgnoreCase)
            )
            {
                return true;
            }

            // Check if it's a SubsonicApiException with specific error codes
            if (ex is SubsonicApiException apiEx)
            {
                // Subsonic error code 70 = "not implemented"
                if (apiEx.ErrorCode == 70)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Records a test result to a JSON file for later use in mocking.
        /// </summary>
        /// <param name="result">The object to serialize.</param>
        /// <param name="fileName">The name of the file (without extension).</param>
        protected void RecordTestResult(object result, string fileName)
        {
            if (!this.Settings.RecordTestResults)
            {
                return;
            }

            try
            {
                string path = Path.Combine(this.Settings.OutputDirectory, $"{fileName}.json");
                string json = JsonSerializer.Serialize(
                    result,
                    new JsonSerializerOptions { WriteIndented = true }
                );

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
