#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Conditionally runs the SubSonicMedia TestKit based on .env file existence.
.DESCRIPTION
    This script checks for the existence of a .env file in the SubSonicMedia.TestKit 
    directory and only runs tests if the configuration exists. This prevents test 
    failures in environments without proper test configuration.
.PARAMETER PreCommit
    If specified, runs in pre-commit hook mode with additional messaging.
.EXAMPLE
    .\scripts\Run-Tests.ps1
    # Runs tests if .env exists, silently skips otherwise
.EXAMPLE
    .\scripts\Run-Tests.ps1 -PreCommit
    # Runs tests with pre-commit messaging for git hooks
#>

param(
    [switch]$PreCommit = $false
)

# Define paths
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent $scriptPath
$testKitDir = Join-Path -Path $projectRoot -ChildPath "SubSonicMedia.TestKit"
$envFilePath = Join-Path -Path $testKitDir -ChildPath ".env"

# Check if the .env file exists
if (Test-Path $envFilePath) {
    Write-Host "📋 Running SubSonicMedia TestKit..." -ForegroundColor Cyan
    
    # Navigate to the TestKit directory
    Push-Location $testKitDir
    
    try {
        # Run the tests and capture the exit code
        dotnet run
        $exitCode = $LASTEXITCODE
        
        # Check the exit code
        if ($exitCode -eq 0) {
            Write-Host "✅ All tests passed or skipped successfully!" -ForegroundColor Green
        } else {
            Write-Host "❌ Tests failed!" -ForegroundColor Red
            Write-Host "🔍 Check the test output above for details." -ForegroundColor Yellow
            
            if ($PreCommit) {
                Write-Host "⚠️ To bypass tests temporarily, rename your .env file to .env.disabled" -ForegroundColor Yellow
            }
            
            # Return to the original directory
            Pop-Location
            exit $exitCode
        }
    } catch {
        Write-Host "❌ Error running tests: $_" -ForegroundColor Red
        Pop-Location
        exit 1
    }
    
    # Return to the original directory
    Pop-Location
} else {
    Write-Host "⏭️ Skipping SubSonicMedia tests - no .env file found in TestKit." -ForegroundColor Yellow
    Write-Host "ℹ️ To enable tests, create a .env file in the SubSonicMedia.TestKit directory." -ForegroundColor Gray
}

# Return success if we made it this far
if ($PreCommit) {
    Write-Host "✅ Pre-commit check completed successfully" -ForegroundColor Green
}
exit 0