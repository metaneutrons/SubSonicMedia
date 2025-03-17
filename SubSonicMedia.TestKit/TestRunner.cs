// <copyright file="TestRunner.cs" company="Fabian Schmieder">
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

using System.Diagnostics;
using SubSonicMedia.TestKit.Helpers;
using SubSonicMedia.TestKit.Models;
using SubSonicMedia.TestKit.Tests;

namespace SubSonicMedia.TestKit
{
    /// <summary>
    /// Manages and executes test cases for the SubSonic API.
    /// </summary>
    public class TestRunner
    {
        private readonly SubsonicClient _client;
        private readonly AppSettings _settings;
        private readonly List<TestBase> _tests = new();
        private readonly Stopwatch _totalStopwatch = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TestRunner"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client to use for tests.</param>
        /// <param name="settings">The application settings.</param>
        public TestRunner(SubsonicClient client, AppSettings settings)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));

            // Register all tests
            this.RegisterTests();
        }

        /// <summary>
        /// Runs all registered tests.
        /// </summary>
        /// <returns>True if all tests pass or are skipped, false if any test fails.</returns>
        public async Task<bool> RunAllTestsAsync()
        {
            ConsoleHelper.LogInfo("Starting test suite execution...");
            this._totalStopwatch.Restart();

            var results = new Dictionary<string, TestResult>();
            int passCount = 0;
            int skipCount = 0;
            int failCount = 0;

            foreach (var test in this._tests)
            {
                TestResult result = await test.RunAsync();
                results[test.Name] = result;

                switch (result)
                {
                    case TestResult.Pass:
                        passCount++;
                        break;
                    case TestResult.Skipped:
                        skipCount++;
                        break;
                    case TestResult.Fail:
                        failCount++;

                        // If fail-fast is enabled, stop on the first failure
                        if (this._settings.FailFast)
                        {
                            ConsoleHelper.LogWarning(
                                "Fail-fast mode enabled, stopping tests after first failure."
                            );
                            // Exit the loop early
                            goto EndTests;
                        }

                        break;
                }

                // Add a small separator between tests
                Console.WriteLine();
            }

            EndTests:
            this._totalStopwatch.Stop();

            // Display summary
            ConsoleHelper.DisplayTestResults(results);
            Console.WriteLine();
            ConsoleHelper.LogInfo(
                $"Total execution time: {this._totalStopwatch.ElapsedMilliseconds}ms"
            );

            // Calculate statistics excluding skipped tests
            int totalRun = passCount + failCount;

            // Generate JUnit XML report if enabled
            if (this._settings.JUnitXmlOutput)
            {
                JUnitReportHelper.GenerateJUnitXmlReport(
                    results,
                    this._tests,
                    this._totalStopwatch.ElapsedMilliseconds,
                    this._settings.OutputDirectory
                );
            }

            // Determine overall success - skip does not count as failure
            bool allPassed = failCount == 0;
            if (allPassed && skipCount == 0)
            {
                ConsoleHelper.LogSuccess("ALL TESTS PASSED");
            }
            else if (allPassed)
            {
                ConsoleHelper.LogSuccess($"ALL EXECUTED TESTS PASSED ({skipCount} SKIPPED)");
            }
            else
            {
                ConsoleHelper.LogError(
                    $"TESTS FAILED: {failCount} of {totalRun} executed tests failed"
                );
                if (skipCount > 0)
                {
                    ConsoleHelper.LogWarning($"{skipCount} tests were skipped");
                }
            }

            return allPassed;
        }

        /// <summary>
        /// Runs a specific test by name.
        /// </summary>
        /// <param name="testName">The name of the test to run.</param>
        /// <returns>True if the test passes or is skipped, false if it fails or is not found.</returns>
        public async Task<bool> RunTestAsync(string testName)
        {
            var test = this._tests.FirstOrDefault(t =>
                t.Name.Equals(testName, StringComparison.OrdinalIgnoreCase)
            );

            if (test == null)
            {
                ConsoleHelper.LogError($"Test '{testName}' not found");
                return false;
            }

            // Start timing for this test run
            this._totalStopwatch.Restart();

            // Run the test
            TestResult result = await test.RunAsync();

            // Stop timing
            this._totalStopwatch.Stop();

            // Generate JUnit XML report for single test if enabled
            if (this._settings.JUnitXmlOutput)
            {
                var results = new Dictionary<string, TestResult> { { test.Name, result } };
                JUnitReportHelper.GenerateJUnitXmlReport(
                    results,
                    new[] { test },
                    this._totalStopwatch.ElapsedMilliseconds,
                    this._settings.OutputDirectory
                );
            }

            // Return true for Pass or Skipped, false only for Fail
            return result != TestResult.Fail;
        }

        /// <summary>
        /// Gets the names of all registered tests.
        /// </summary>
        /// <returns>A list of test names.</returns>
        public IReadOnlyList<string> GetTestNames()
        {
            return this._tests.Select(t => t.Name).ToList();
        }

        private void RegisterTests()
        {
            // Core functionality tests
            this._tests.Add(new ConnectionTest(this._client, this._settings));
            this._tests.Add(new BrowsingTest(this._client, this._settings));
            this._tests.Add(new SearchTest(this._client, this._settings));
            this._tests.Add(new MediaTest(this._client, this._settings));
            this._tests.Add(new PlaylistTest(this._client, this._settings));

            // Additional client tests
            this._tests.Add(new BookmarkTest(this._client, this._settings));
            this._tests.Add(new ChatTest(this._client, this._settings));
            this._tests.Add(new JukeboxTest(this._client, this._settings));
            this._tests.Add(new PodcastTest(this._client, this._settings));
            this._tests.Add(new RadioTest(this._client, this._settings));
            this._tests.Add(new SystemTest(this._client, this._settings));
            this._tests.Add(new UserTest(this._client, this._settings));
            this._tests.Add(new VideoTest(this._client, this._settings));
        }
    }
}
