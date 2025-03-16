// <copyright file="TestRunner.cs" company="Fabian Schmieder">
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
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            
            // Register all tests
            RegisterTests();
        }
        
        /// <summary>
        /// Runs all registered tests.
        /// </summary>
        /// <returns>True if all tests pass, false otherwise.</returns>
        public async Task<bool> RunAllTestsAsync()
        {
            ConsoleHelper.LogInfo("Starting test suite execution...");
            _totalStopwatch.Restart();
            
            var results = new Dictionary<string, bool>();
            int passCount = 0;
            
            foreach (var test in _tests)
            {
                bool success = await test.RunAsync();
                results[test.Name] = success;
                
                if (success)
                {
                    passCount++;
                }
                
                // Add a small separator between tests
                Console.WriteLine();
            }
            
            _totalStopwatch.Stop();
            
            // Display summary
            ConsoleHelper.DisplayTestResults(results);
            Console.WriteLine();
            ConsoleHelper.LogInfo($"Total execution time: {_totalStopwatch.ElapsedMilliseconds}ms");
            
            // Determine overall success
            bool allPassed = passCount == _tests.Count;
            if (allPassed)
            {
                ConsoleHelper.LogSuccess("ALL TESTS PASSED");
            }
            else
            {
                ConsoleHelper.LogError($"TESTS FAILED: {_tests.Count - passCount} of {_tests.Count} tests failed");
            }
            
            return allPassed;
        }
        
        /// <summary>
        /// Runs a specific test by name.
        /// </summary>
        /// <param name="testName">The name of the test to run.</param>
        /// <returns>True if the test passes, false otherwise or if the test is not found.</returns>
        public async Task<bool> RunTestAsync(string testName)
        {
            var test = _tests.FirstOrDefault(t => t.Name.Equals(testName, StringComparison.OrdinalIgnoreCase));
            
            if (test == null)
            {
                ConsoleHelper.LogError($"Test '{testName}' not found");
                return false;
            }
            
            return await test.RunAsync();
        }
        
        /// <summary>
        /// Gets the names of all registered tests.
        /// </summary>
        /// <returns>A list of test names.</returns>
        public IReadOnlyList<string> GetTestNames()
        {
            return _tests.Select(t => t.Name).ToList();
        }
        
        private void RegisterTests()
        {
            // Core functionality tests
            _tests.Add(new ConnectionTest(_client, _settings));
            _tests.Add(new BrowsingTest(_client, _settings));
            _tests.Add(new SearchTest(_client, _settings));
            _tests.Add(new MediaTest(_client, _settings));
            
            // More tests can be added here
        }
    }
}
