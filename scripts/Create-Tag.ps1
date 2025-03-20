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
$hasUncommittedChanges = $false
if ($status) {
    $hasUncommittedChanges = $true
    Write-Warning "You have uncommitted changes. It's recommended to commit or stash them before creating a tag."
    Write-Host "      Uncommitted changes:" -ForegroundColor $colors.Muted
    $status | ForEach-Object { Write-Host "      $_" -ForegroundColor $colors.Muted }
    Write-Host ""
}
else {
    Write-Success "No uncommitted changes detected"
}

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

# Check existing release tags
Write-StepHeader -Message "Analyzing existing release tags" -Icon $icons.Version

# Get all release tags (starting with 'v')
$releaseTags = @(git tag -l "v*" | Sort-Object)

# Check if we have any release tags
if ($releaseTags.Count -eq 0) {
    Write-Info "No existing release tags found"
}
else {
    # Display latest tags
    $latestTags = $releaseTags | Select-Object -Last 5
    Write-Info "Latest 5 release tags:"
    foreach ($tag in $latestTags) {
        Write-Host "      $tag" -ForegroundColor $colors.Muted
    }

    # Check for version pattern consistency
    $versionPattern = '^v\d+\.\d+\.\d+(-[a-zA-Z0-9\.]+)?$'
    $inconsistentTags = @($releaseTags | Where-Object { $_ -notmatch $versionPattern })

    if ($inconsistentTags.Count -gt 0) {
        Write-Warning "Found $($inconsistentTags.Count) tags with inconsistent version format:"
        foreach ($tag in $inconsistentTags) {
            Write-Host "      $tag" -ForegroundColor $colors.Warning
        }
    }

    # Check for version sequence
    $versionNumbers = @($releaseTags | Where-Object { $_ -match $versionPattern } | ForEach-Object {
            if ($_ -match 'v(\d+)\.(\d+)\.(\d+)') {
                [PSCustomObject]@{
                    Tag      = $_
                    Major    = [int]$Matches[1]
                    Minor    = [int]$Matches[2]
                    Patch    = [int]$Matches[3]
                    Original = $_
                }
            }
        }) | Sort-Object Major, Minor, Patch

    # Find gaps in version sequence
    $previousVersion = $null
    $gapsFound = $false

    foreach ($version in $versionNumbers) {
        if ($null -ne $previousVersion) {
            # Check for unexpected jumps
            if ($version.Major - $previousVersion.Major -gt 1) {
                if (-not $gapsFound) {
                    Write-Warning "Potential gaps in version sequence:"
                    $gapsFound = $true
                }
                Write-Host "      Gap between $($previousVersion.Original) and $($version.Original)" -ForegroundColor $colors.Warning
            }
            elseif ($version.Major -eq $previousVersion.Major -and $version.Minor - $previousVersion.Minor -gt 1) {
                if (-not $gapsFound) {
                    Write-Warning "Potential gaps in version sequence:"
                    $gapsFound = $true
                }
                Write-Host "      Gap between $($previousVersion.Original) and $($version.Original)" -ForegroundColor $colors.Warning
            }
        }
        $previousVersion = $version
    }

    if (-not $gapsFound) {
        Write-Success "Version sequence appears consistent"
    }
}

# Check if current tag already exists
$tagExists = git tag -l "v$fullVersion"
if ($tagExists) {
    Write-Warning "Tag v$fullVersion already exists!"
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

# Handle uncommitted changes as a blocker
if ($hasUncommittedChanges) {
    Write-Host "  $($icons.Warning)  " -ForegroundColor $colors.Warning -NoNewline
    Write-Host "You have uncommitted changes." -ForegroundColor $colors.Warning
    Write-Host ""
    Write-Host "  Commands that would have been executed:" -ForegroundColor $colors.Info
    Write-Host "  $tagCommand" -ForegroundColor $colors.Muted
    Write-Host "  $pushCommand" -ForegroundColor $colors.Muted
    Write-Host ""
    Write-Host "  Please commit or stash your changes first, then run the script again." -ForegroundColor $colors.Warning
    exit 0
}

# Determine if we need confirmation (only for existing tags now)
$needsConfirmation = $false
$confirmationReason = ""

if ($tagExists) {
    $needsConfirmation = $true
    $confirmationReason = "Tag already exists"
}

# Ask for confirmation
if ($needsConfirmation) {
    if (-not (Get-Confirmation "$confirmationReason. Create tag anyway?")) {
        Write-Info "Operation cancelled by user"
        exit 0
    }
} else {
    if (-not (Get-Confirmation "Execute these commands?")) {
        Write-Info "Operation cancelled by user"
        exit 0
    }
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
