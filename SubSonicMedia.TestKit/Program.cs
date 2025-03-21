// <copyright file="Program.cs" company="Fabian Schmieder">
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
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using SubSonicMedia;
using SubSonicMedia.TestKit;
using SubSonicMedia.TestKit.Helpers;
using SubSonicMedia.TestKit.Models;

// Display application header
ConsoleHelper.DisplayHeader();

// Load configuration
var appSettings = LoadConfiguration();
if (appSettings == null)
{
    Environment.Exit(1);
    return;
}

// Validate configuration
if (string.IsNullOrEmpty(appSettings.ServerUrl))
{
    ConsoleHelper.LogError("Server URL is not configured. Please check your .env file.");
    Environment.Exit(1);
    return;
}

if (string.IsNullOrEmpty(appSettings.Username) || string.IsNullOrEmpty(appSettings.Password))
{
    ConsoleHelper.LogError(
        "Authentication credentials are not configured. Please check your .env file."
    );
    Environment.Exit(1);
    return;
}

// Ensure output directory exists
if (appSettings.RecordTestResults && !string.IsNullOrEmpty(appSettings.OutputDirectory))
{
    Directory.CreateDirectory(appSettings.OutputDirectory);
    ConsoleHelper.LogInfo($"Test results will be recorded to {appSettings.OutputDirectory}");
}

// Initialize Subsonic client
ConsoleHelper.LogInfo($"Initializing Subsonic client for {appSettings.ServerUrl}");
var connectionInfo = new SubSonicMedia.Models.SubsonicConnectionInfo
{
    ServerUrl = appSettings.ServerUrl,
    Username = appSettings.Username,
    Password = appSettings.Password,
    ApiVersion = appSettings.ApiVersion ?? VersionInfo.SubsonicApiVersion,
};
var client = new SubsonicClient(connectionInfo);

// Display connection information
ConsoleHelper.LogServerConnection(appSettings.ServerUrl);
ConsoleHelper.LogInfo($"Authenticated as: {appSettings.Username}");
ConsoleHelper.LogInfo("Response format: JSON");

// Create test runner
var testRunner = new TestRunner(client, appSettings);

// Parse command line arguments
string[] commandArgs = Environment.GetCommandLineArgs().Skip(1).ToArray();
if (commandArgs.Length > 0)
{
    // Run specific test if specified
    if (commandArgs[0].Equals("test", StringComparison.OrdinalIgnoreCase) && commandArgs.Length > 1)
    {
        var testName = commandArgs[1];
        ConsoleHelper.LogInfo($"Running specific test: {testName}");
        var result = await testRunner.RunTestAsync(testName);

        // Exit with appropriate code (0 for pass/skip, 1 for fail)
        // The RunTestAsync method returns false for both "test failed" and "test not found"
        // But it already logs the appropriate error message
        Environment.Exit(result ? 0 : 1);

        return;
    }

    // List available tests
    if (commandArgs[0].Equals("list", StringComparison.OrdinalIgnoreCase))
    {
        var testNames = testRunner.GetTestNames();
        ConsoleHelper.LogInfo("Available tests:");
        foreach (var name in testNames)
        {
            Console.WriteLine($" - {name}");
        }

        return;
    }

    // Display help
    if (
        commandArgs[0].Equals("help", StringComparison.OrdinalIgnoreCase)
        || commandArgs[0] == "--help"
        || commandArgs[0] == "-h"
    )
    {
        DisplayHelp();
        return;
    }

    // Unknown command
    ConsoleHelper.LogError($"Unknown command: {commandArgs[0]}");
    DisplayHelp();
    Environment.Exit(1);
    return;
}

// Run all tests by default
ConsoleHelper.LogInfo("Running all tests...");
bool allTestsPassed = await testRunner.RunAllTestsAsync();

// Exit with appropriate code
// 0 - All tests passed or skipped (no failures)
// 1 - One or more tests failed
// 2 - Other errors (should be used if the test runner itself had an issue)
Environment.Exit(allTestsPassed ? 0 : 1);

// Helper method to load configuration
AppSettings? LoadConfiguration()
{
    try
    {
        // Get the location of the executing assembly
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

        if (string.IsNullOrEmpty(assemblyDirectory))
        {
            ConsoleHelper.LogError("Could not determine executing assembly directory.");
            return null;
        }

        // Set up configuration with .env file support
        var config = new ConfigurationBuilder()
            .SetBasePath(assemblyDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .AddDotEnvFile(".env", optional: true) // Changed to true to avoid null reference
            .Build();

        // Map configuration to AppSettings
        var settings = new AppSettings
        {
            ServerUrl = config["SUBSONIC_SERVER_URL"] ?? string.Empty,
            Username = config["SUBSONIC_USERNAME"] ?? string.Empty,
            Password = config["SUBSONIC_PASSWORD"] ?? string.Empty,
            ApiVersion = config["SUBSONIC_API_VERSION"] ?? VersionInfo.SubsonicApiVersion, // Set a default API version
            RecordTestResults = bool.TryParse(config["RECORD_TEST_RESULTS"], out bool record)
                ? record
                : true,
            OutputDirectory =
                config["OUTPUT_DIRECTORY"] ?? Path.Combine(assemblyDirectory, "TestResults"),
            FailFast = bool.TryParse(config["FAIL_FAST"], out bool failFast) ? failFast : false,
            JUnitXmlOutput = bool.TryParse(config["JUNIT_XML_OUTPUT"], out bool junitOutput)
                ? junitOutput
                : false,
        };

        return settings;
    }
    catch (Exception ex)
    {
        ConsoleHelper.LogError($"Error loading configuration: {ex.Message}");
        return null;
    }
}

// Helper method to display help information
void DisplayHelp()
{
    AnsiConsole.WriteLine("SubSonicMedia TestKit - Command Line Interface");
    AnsiConsole.WriteLine();
    AnsiConsole.WriteLine("Usage:");
    AnsiConsole.WriteLine("  dotnet run                    Run all tests");
    AnsiConsole.WriteLine("  dotnet run test <test-name>   Run a specific test");
    AnsiConsole.WriteLine("  dotnet run list               List all available tests");
    AnsiConsole.WriteLine("  dotnet run help               Display this help message");
    AnsiConsole.WriteLine();
    AnsiConsole.WriteLine("Configuration:");
    AnsiConsole.WriteLine("  Settings are loaded from the .env file in the application directory.");
    AnsiConsole.WriteLine("  See .env.example for required settings.");
    AnsiConsole.WriteLine();
    AnsiConsole.WriteLine("Environment Variables:");
    AnsiConsole.WriteLine("  SUBSONIC_SERVER_URL      URL of the Subsonic server (required)");
    AnsiConsole.WriteLine("  SUBSONIC_USERNAME        Username for authentication (required)");
    AnsiConsole.WriteLine("  SUBSONIC_PASSWORD        Password for authentication (required)");
    AnsiConsole.WriteLine(
        $"  SUBSONIC_API_VERSION     API version to use (optional, defaults to {VersionInfo.SubsonicApiVersion})"
    );
    AnsiConsole.WriteLine(
        "  RECORD_TEST_RESULTS      Whether to record test results (optional, defaults to true)"
    );
    AnsiConsole.WriteLine("  OUTPUT_DIRECTORY         Directory to save test results (optional)");
    AnsiConsole.WriteLine(
        "  FAIL_FAST                Exit on first test failure (optional, defaults to false)"
    );
    AnsiConsole.WriteLine(
        "  JUNIT_XML_OUTPUT         Generate JUnit XML report (optional, defaults to false)"
    );
}
