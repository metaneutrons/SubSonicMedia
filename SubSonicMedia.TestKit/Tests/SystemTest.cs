// <copyright file="SystemTest.cs" company="Fabian Schmieder">
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
using SubSonicMedia.TestKit.Helpers;
using SubSonicMedia.TestKit.Models;

namespace SubSonicMedia.TestKit.Tests
{
    /// <summary>
    /// Tests system capabilities of the Subsonic API.
    /// </summary>
    public class SystemTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public SystemTest(SubsonicClient client, AppSettings settings)
            : base(client, settings)
        {
        }

        /// <inheritdoc/>
        public override string Name => "System Test";

        /// <inheritdoc/>
        public override string Description => "Tests system features including ping, license, and scan status";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            bool allTestsPassed = true;
            
            // Test 1: Ping
            ConsoleHelper.LogInfo("Testing Ping...");
            try
            {
                var pingResponse = await Client.System.PingAsync();
                RecordTestResult(pingResponse, "system_ping");
                
                if (pingResponse.IsSuccess)
                {
                    ConsoleHelper.LogSuccess($"Successfully pinged server, version: {pingResponse.Version}");
                }
                else
                {
                    ConsoleHelper.LogError($"Failed to ping server: {pingResponse.Error?.Message}");
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error pinging server: {ex.Message}");
                allTestsPassed = false;
            }
            
            // Test 2: Get License
            ConsoleHelper.LogInfo("Testing GetLicense...");
            try
            {
                var licenseResponse = await Client.System.GetLicenseAsync();
                RecordTestResult(licenseResponse, "system_license");
                
                if (licenseResponse.IsSuccess)
                {
                    ConsoleHelper.LogSuccess("Successfully retrieved license information");
                    
                    var table = new Table();
                    table.AddColumn("Property");
                    table.AddColumn("Value");
                    
                    table.AddRow("Valid", licenseResponse.License.Valid.ToString());
                    table.AddRow("Email", licenseResponse.License.Email ?? "N/A");
                    table.AddRow("License Version", licenseResponse.License.LicenseVersion ?? "N/A");
                    table.AddRow("Trial", licenseResponse.License.Trial ?? "N/A");
                    
                    if (licenseResponse.License.Expires.HasValue)
                    {
                        table.AddRow("Expires", licenseResponse.License.Expires.Value.ToString());
                    }
                    
                    AnsiConsole.Write(table);
                }
                else
                {
                    ConsoleHelper.LogError($"Failed to get license: {licenseResponse.Error?.Message}");
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error getting license: {ex.Message}");
                allTestsPassed = false;
            }
            
            // Test 3: Get Scan Status
            ConsoleHelper.LogInfo("Testing GetScanStatus...");
            try
            {
                var scanStatusResponse = await Client.System.GetScanStatusAsync();
                RecordTestResult(scanStatusResponse, "system_scan_status");
                
                if (scanStatusResponse.IsSuccess)
                {
                    ConsoleHelper.LogSuccess("Successfully retrieved scan status");
                    
                    var table = new Table();
                    table.AddColumn("Property");
                    table.AddColumn("Value");
                    
                    table.AddRow("Scanning", scanStatusResponse.ScanStatus.Scanning.ToString());
                    table.AddRow("Count", scanStatusResponse.ScanStatus.Count.ToString());
                    
                    if (!string.IsNullOrEmpty(scanStatusResponse.ScanStatus.Folder))
                    {
                        table.AddRow("Folder", scanStatusResponse.ScanStatus.Folder);
                    }
                    
                    AnsiConsole.Write(table);
                }
                else
                {
                    ConsoleHelper.LogError($"Failed to get scan status: {scanStatusResponse.Error?.Message}");
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error getting scan status: {ex.Message}");
                allTestsPassed = false;
            }
            
            // Test 4: Start Scan (optional - only if we want to trigger a scan)
            bool runScanTest = false; // Set to true to test scanning
            
            if (runScanTest && allTestsPassed)
            {
                ConsoleHelper.LogInfo("Testing StartScan...");
                try
                {
                    var startScanResponse = await Client.System.StartScanAsync();
                    
                    if (startScanResponse.IsSuccess)
                    {
                        ConsoleHelper.LogSuccess("Successfully started media library scan");
                        
                        // Check status
                        await Task.Delay(2000); // Wait briefly
                        var checkStatusResponse = await Client.System.GetScanStatusAsync();
                        
                        if (checkStatusResponse.IsSuccess)
                        {
                            ConsoleHelper.LogInfo($"Scan in progress: {checkStatusResponse.ScanStatus.Scanning}");
                            
                            if (checkStatusResponse.ScanStatus.Scanning)
                            {
                                ConsoleHelper.LogInfo($"Items scanned so far: {checkStatusResponse.ScanStatus.Count}");
                                
                                if (!string.IsNullOrEmpty(checkStatusResponse.ScanStatus.Folder))
                                {
                                    ConsoleHelper.LogInfo($"Currently scanning folder: {checkStatusResponse.ScanStatus.Folder}");
                                }
                            }
                        }
                        else
                        {
                            ConsoleHelper.LogError($"Failed to check scan status: {checkStatusResponse.Error?.Message}");
                        }
                    }
                    else
                    {
                        ConsoleHelper.LogError($"Failed to start scan: {startScanResponse.Error?.Message}");
                        allTestsPassed = false;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.LogError($"Error starting scan: {ex.Message}");
                    allTestsPassed = false;
                }
            }
            
            return allTestsPassed ? TestResult.Pass : TestResult.Fail;
        }
    }
}