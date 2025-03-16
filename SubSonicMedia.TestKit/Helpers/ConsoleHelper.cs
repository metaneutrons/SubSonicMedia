// <copyright file="ConsoleHelper.cs" company="Fabian Schmieder">
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

using Spectre.Console;

namespace SubSonicMedia.TestKit.Helpers
{
    /// <summary>
    /// Helper class for console output with styled formatting and UTF-8 icons.
    /// </summary>
    public static class ConsoleHelper
    {
        // UTF-8 Icons
        private const string SuccessIcon = "✓";
        private const string ErrorIcon = "✗";
        private const string InfoIcon = "ℹ";
        private const string WarningIcon = "⚠";
        private const string PlayIcon = "▶";
        private const string MusicIcon = "♪";
        private const string FolderIcon = "📁";
        private const string FileIcon = "📄";
        private const string UserIcon = "👤";
        private const string SearchIcon = "🔍";
        private const string TestIcon = "🧪";
        private const string TimerIcon = "⏱";
        private const string ServerIcon = "🖥";
        private const string ApiIcon = "🔗";
        
        /// <summary>
        /// Displays the application header.
        /// </summary>
        public static void DisplayHeader()
        {
            AnsiConsole.Write(
                new FigletText("SubSonicMedia")
                    .LeftJustified()
                    .Color(Color.Green));
                    
            AnsiConsole.Write(
                new FigletText("TestKit")
                    .LeftJustified()
                    .Color(Color.Aqua));
            
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]A comprehensive testing tool for the SubSonicMedia API[/]");
            AnsiConsole.WriteLine();
        }
        
        /// <summary>
        /// Logs a success message to the console.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void LogSuccess(string message)
        {
            AnsiConsole.MarkupLine($"[green]{SuccessIcon} {message}[/]");
        }
        
        /// <summary>
        /// Logs an error message to the console.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void LogError(string message)
        {
            AnsiConsole.MarkupLine($"[red]{ErrorIcon} {message}[/]");
        }
        
        /// <summary>
        /// Logs an info message to the console.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void LogInfo(string message)
        {
            AnsiConsole.MarkupLine($"[blue]{InfoIcon} {message}[/]");
        }
        
        /// <summary>
        /// Logs a warning message to the console.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void LogWarning(string message)
        {
            AnsiConsole.MarkupLine($"[yellow]{WarningIcon} {message}[/]");
        }
        
        /// <summary>
        /// Logs a server connection message.
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        public static void LogServerConnection(string serverUrl)
        {
            AnsiConsole.MarkupLine($"[blue]{ServerIcon} Connecting to server: [underline]{serverUrl}[/][/]");
        }
        
        /// <summary>
        /// Logs an API call message.
        /// </summary>
        /// <param name="endpoint">The API endpoint being called.</param>
        public static void LogApiCall(string endpoint)
        {
            AnsiConsole.MarkupLine($"[grey]{ApiIcon} Calling API: [italic]{endpoint}[/][/]");
        }
        
        /// <summary>
        /// Logs a test start message.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        public static void LogTestStart(string testName)
        {
            AnsiConsole.MarkupLine($"[cyan]{TestIcon} Starting test: [bold]{testName}[/][/]");
        }
        
        /// <summary>
        /// Logs a test completion message with timing information.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        /// <param name="elapsedMilliseconds">The elapsed time in milliseconds.</param>
        /// <param name="success">Whether the test was successful.</param>
        public static void LogTestCompletion(string testName, long elapsedMilliseconds, bool success)
        {
            string icon = success ? SuccessIcon : ErrorIcon;
            string color = success ? "green" : "red";
            AnsiConsole.MarkupLine($"[{color}]{icon} Test completed: [bold]{testName}[/] {TimerIcon} [italic]{elapsedMilliseconds}ms[/][/]");
        }
        
        /// <summary>
        /// Creates a progress spinner for an async operation.
        /// </summary>
        /// <param name="message">The message to display while spinning.</param>
        /// <returns>A configured progress spinner.</returns>
        public static Progress CreateProgress(string message)
        {
            return AnsiConsole.Progress()
                .AutoClear(true)
                .HideCompleted(true)
                .Columns(new ProgressColumn[]
                {
                    new SpinnerColumn(),
                    new TaskDescriptionColumn(),
                    new PercentageColumn(),
                    new ElapsedTimeColumn(),
                });
        }
        
        /// <summary>
        /// Displays a table of test results.
        /// </summary>
        /// <param name="results">The dictionary of test results with test names as keys and success status as values.</param>
        public static void DisplayTestResults(Dictionary<string, bool> results)
        {
            var table = new Table();
            
            table.AddColumn("Test");
            table.AddColumn("Status");
            
            int passCount = 0;
            foreach (var (testName, success) in results)
            {
                string status = success ? $"[green]{SuccessIcon} PASS[/]" : $"[red]{ErrorIcon} FAIL[/]";
                table.AddRow(testName, status);
                
                if (success)
                {
                    passCount++;
                }
            }
            
            AnsiConsole.WriteLine();
            AnsiConsole.Write(table);
            
            int total = results.Count;
            double percentage = total > 0 ? (double)passCount / total * 100 : 0;
            
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[bold]Test Results: {passCount}/{total} passed ({percentage:F2}%)[/]");
        }
        
        /// <summary>
        /// Displays a loading bar for a specified duration.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="durationMs">The duration in milliseconds.</param>
        /// <returns>A task representing the loading operation.</returns>
        public static async Task ShowLoadingBarAsync(string message, int durationMs)
        {
            await AnsiConsole.Progress()
                .AutoClear(true)
                .Columns(new ProgressColumn[]
                {
                    new SpinnerColumn(),
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                })
                .StartAsync(async ctx =>
                {
                    var task = ctx.AddTask(message);
                    
                    // The number of steps to complete over the duration
                    const int steps = 100;
                    int delayPerStep = durationMs / steps;
                    
                    for (int i = 0; i < steps; i++)
                    {
                        task.Increment(1);
                        await Task.Delay(delayPerStep);
                    }
                });
        }
    }
}
