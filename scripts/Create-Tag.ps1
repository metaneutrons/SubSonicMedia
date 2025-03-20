#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Creates a new Git tag for GitHub CI based on the project version.
.DESCRIPTION
    This script performs all necessary pre-checks (no uncommitted files),
    extracts the version number from the project's versioning schema,
    and proposes the Git commands to execute after confirmation.
.NOTES
    Author: Fabian Schmieder
    Date:   $(Get-Date -Format "yyyy-MM-dd")
#>

# Set strict mode and error handling
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# Define fancy UTF-8 icons
$icons = @{
    Check    = "✅"
    Cross    = "❌"
    Error    = "💔"
    Warning  = "⚠️"
    Info     = "ℹ️"
    Tag      = "🏷️"
    Git      = "🔄"
    Rocket   = "🚀"
    Question = "❓"
    Success  = "🎉"
    Fail     = "💥"
    Wait     = "⏳"
    Lock     = "🔒"
    Unlock   = "🔓"
    Calendar = "📅"
    Version  = "📊"
    Commit   = "📝"
}

# Define color theme
$colors = @{
    Primary   = "Cyan"
    Secondary = "Magenta"
    Success   = "Green"
    Error     = "Red"
    Warning   = "Yellow"
    Info      = "Blue"
    Muted     = "DarkGray"
}

# Helper functions
function Write-StepHeader {
    param([string]$Message, [string]$Icon)

    Write-Host ""
    Write-Host "  $Icon  " -ForegroundColor $colors.Primary -NoNewline
    Write-Host $Message -ForegroundColor $colors.Primary
    Write-Host "  " + "─" * ($Message.Length + 4) -ForegroundColor $colors.Muted
}

function Write-StepResult {
    param([string]$Message, [string]$Icon, [string]$Color)

    Write-Host "    $Icon  " -ForegroundColor $Color -NoNewline
    Write-Host $Message -ForegroundColor $Color
}

function Write-Success {
    param([string]$Message)
    Write-StepResult -Message $Message -Icon $icons.Check -Color $colors.Success
}

function Write-Error {
    param([string]$Message)
    Write-StepResult -Message $Message -Icon $icons.Cross -Color $colors.Error
    exit 1
}

function Write-Info {
    param([string]$Message)
    Write-StepResult -Message $Message -Icon $icons.Info -Color $colors.Info
}

function Write-Warning {
    param([string]$Message)
    Write-StepResult -Message $Message -Icon $icons.Warning -Color $colors.Warning
}

function Get-Confirmation {
    param([string]$Message)

    Write-Host ""
    Write-Host "  $($icons.Question)  " -ForegroundColor $colors.Warning -NoNewline
    Write-Host $Message -ForegroundColor $colors.Warning -NoNewline

    $confirmation = Read-Host " [y/N]"
    return $confirmation -eq "y"
}

# Print banner
function Show-Banner {
    $banner = @"

  _____       _     _____             _      __  __          _ _       
 / ____|     | |   / ____|           (_)    |  \/  |        | (_)      
| (___  _   _| |__| (___   ___  _ __  _  ___| \  / | ___  __| |_  __ _ 
 \___ \| | | | '_ \\___ \ / _ \| '_ \| |/ __| |\/| |/ _ \/ _` | |/ _` |
 ____) | |_| | |_) |___) | (_) | | | | | (__| |  | |  __/ (_| | | (_| |
|_____/ \__,_|_.__/_____/ \___/|_| |_|_|\___|_|  |_|\___|\__,_|_|\__,_|
                                                                        
  $($icons.Tag)  TAG GENERATOR  $($icons.Rocket)
  ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

"@
    Write-Host $banner -ForegroundColor $colors.Secondary
}

# Main script execution starts here
Show-Banner

# Check if we're in the git repository root
Write-StepHeader -Message "Checking repository status" -Icon $icons.Git

$repoRoot = git rev-parse --show-toplevel 2>$null
if (-not $?) {
    Write-Error "Not in a git repository"
}

# Change to the repository root
Set-Location $repoRoot
Write-Success "Working in repository root: $repoRoot"

# Check for uncommitted changes
$status = git status --porcelain
if ($status) {
    Write-Error "You have uncommitted changes. Please commit or stash them before creating a tag."
}
Write-Success "No uncommitted changes detected"

# Check if we're on main branch
$currentBranch = git branch --show-current
if ($currentBranch -ne "main") {
    Write-Warning "You are not on the main branch (current: $currentBranch)"
    if (-not (Get-Confirmation "Continue anyway?")) {
        exit 0
    }
}
else {
    Write-Success "Currently on main branch"
}

# Extract version from Directory.Build.props
Write-StepHeader -Message "Extracting version information" -Icon $icons.Version

$buildPropsPath = Join-Path $repoRoot "SubSonicMedia" "Directory.Build.props"
if (-not (Test-Path $buildPropsPath)) {
    Write-Error "Directory.Build.props not found at $buildPropsPath"
}

[xml]$buildProps = Get-Content $buildPropsPath
$versionPrefix = $buildProps.Project.PropertyGroup.VersionPrefix
$versionSuffix = $buildProps.Project.PropertyGroup.VersionSuffix

if (-not $versionPrefix) {
    Write-Error "Could not extract VersionPrefix from Directory.Build.props"
}

# Construct the full version string
$fullVersion = $versionPrefix
if ($versionSuffix) {
    $fullVersion += "-$versionSuffix"
}

Write-Success "Version extracted: $fullVersion"

# Check if tag already exists
$existingTags = git tag -l "v$fullVersion"
if ($existingTags) {
    Write-Warning "Tag v$fullVersion already exists!"
    if (-not (Get-Confirmation "Create tag anyway?")) {
        exit 0
    }
}

# Get the current date for the tag message
$currentDate = Get-Date -Format "yyyy-MM-dd"

# Propose git commands
Write-StepHeader -Message "Proposed Git commands" -Icon $icons.Commit

$tagCommand = "git tag -a v$fullVersion -m 'Release v$fullVersion ($currentDate)'"
$pushCommand = "git push origin v$fullVersion"

Write-Host "    The following commands will be executed:" -ForegroundColor $colors.Info
Write-Host ""
Write-Host "    $tagCommand" -ForegroundColor $colors.Primary
Write-Host "    $pushCommand" -ForegroundColor $colors.Primary
Write-Host ""

# Ask for confirmation
if (-not (Get-Confirmation "Execute these commands?")) {
    Write-Info "Operation cancelled by user"
    exit 0
}

# Execute the commands
Write-StepHeader -Message "Creating and pushing tag" -Icon $icons.Wait

try {
    # Create the tag
    Write-Host "    Creating tag..." -ForegroundColor $colors.Muted
    Invoke-Expression $tagCommand
    if (-not $?) { throw "Failed to create tag" }

    # Push the tag
    Write-Host "    Pushing tag to origin..." -ForegroundColor $colors.Muted
    Invoke-Expression $pushCommand
    if (-not $?) { throw "Failed to push tag" }

    # Success message
    Write-Host ""
    Write-Host "  $($icons.Success)  " -ForegroundColor $colors.Success -NoNewline
    Write-Host "Tag v$fullVersion successfully created and pushed to origin!" -ForegroundColor $colors.Success
    Write-Host ""
    Write-Host "  $($icons.Rocket)  " -ForegroundColor $colors.Secondary -NoNewline
    Write-Host "GitHub CI workflow should now be triggered." -ForegroundColor $colors.Secondary
    Write-Host ""
}
catch {
    Write-Host ""
    Write-Host "  $($icons.Fail)  " -ForegroundColor $colors.Error -NoNewline
    Write-Host "Error: $_" -ForegroundColor $colors.Error
    Write-Host ""
    exit 1
}
