#!/usr/bin/env pwsh
# PowerShell implementation of Husky script runner

# Function for debug messages
function Debug-Husky {
    param([string]$Message)
    if ($env:HUSKY_DEBUG -eq "1") {
        Write-Host "husky (debug) - $Message"
    }
}

# Get hook name from calling script
$hookName = [System.IO.Path]::GetFileName($MyInvocation.PSCommandPath)
Debug-Husky "starting $hookName..."

# Skip hook if HUSKY env var is set to 0 or false
if ($env:HUSKY -eq "0" -or $env:HUSKY -eq "false") {
    Debug-Husky "HUSKY env variable is set to 0 or false, skipping hook"
    exit 0
}

# Source user config if it exists
$huskyrcPath = Join-Path $HOME ".huskyrc.ps1"
if (Test-Path $huskyrcPath) {
    Debug-Husky "sourcing ~/.huskyrc.ps1"
    . $huskyrcPath
}

# Execute the hook script
try {
    # Run the hook script with the same arguments
    & $PSCommandPath @args
    if ($LASTEXITCODE -ne 0) {
        Write-Host "husky - $hookName hook exited with code $LASTEXITCODE (error)" -ForegroundColor Red
        exit $LASTEXITCODE
    }
} catch {
    Write-Host "husky - $hookName hook encountered an error: $_" -ForegroundColor Red
    exit 1
}