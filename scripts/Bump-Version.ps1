#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Analyzes commit messages and bumps version according to semantic versioning rules.
.DESCRIPTION
    This script analyzes Git commit messages since the last tag and determines the appropriate
    version bump (major, minor, patch) based on conventional commit format. It updates the
    version in Directory.Build.props but does not create commits or tags.

    Use Create-Tag.ps1 to create a tag after bumping the version.
.PARAMETER Apply
    If specified, applies the version change to Directory.Build.props.
.PARAMETER ForceBump
    Forces a specific bump type (patch, minor, major) instead of analyzing commits.
.PARAMETER Stage
    Specifies a stage transition (alpha, beta, release) for the version.
.EXAMPLE
    .\scripts\Bump-Version.ps1
    # Analyzes commits and suggests a version bump
.EXAMPLE
    .\scripts\Bump-Version.ps1 -Apply
    # Analyzes commits and applies the version bump to Directory.Build.props
.EXAMPLE
    .\scripts\Bump-Version.ps1 -ForceBump minor -Apply
    # Forces a minor version bump and applies it
.EXAMPLE
    .\scripts\Bump-Version.ps1 -Stage beta -Apply
    # Transitions the current version to beta stage
#>

param (
    [switch]$Apply = $false,
    [ValidateSet("patch", "minor", "major", "")]
    [string]$ForceBump = "",
    [ValidateSet("alpha", "beta", "release", "")]
    [string]$Stage = ""
)

# Ensure we stop on errors
$ErrorActionPreference = "Stop"

# Define fancy UTF-8 icons
$icons = @{
    Check    = "✅"
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
    Major    = "💥"
    Minor    = "✨"
    Patch    = "🔧"
    Alpha    = "🧪"
    Beta     = "🧩"
    Release  = "🚀"
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
    Major     = "Red"
    Minor     = "Yellow"
    Patch     = "Green"
    Alpha     = "Magenta"
    Beta      = "Blue"
    Release   = "Green"
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
    Write-StepResult -Message $Message -Icon $icons.Error -Color $colors.Error
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

  $($icons.Version)  VERSION BUMPER  $($icons.Rocket)
  ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

"@
    Write-Host $banner -ForegroundColor $colors.Secondary
}

# Main script execution starts here
Show-Banner

# Props file path
$PropsFile = "SubSonicMedia\Directory.Build.props"

# Get current version from Directory.Build.props
[xml]$PropsXml = Get-Content $PropsFile
$CurrentVersion = $PropsXml.Project.PropertyGroup.VersionPrefix
$CurrentSuffix = $PropsXml.Project.PropertyGroup.VersionSuffix

Write-StepHeader -Message "Current Version Information" -Icon $icons.Info
Write-Info "Version: $CurrentVersion"
if ($CurrentSuffix) {
    Write-Info "Stage: $CurrentSuffix"
}
else {
    Write-Info "Stage: release (no suffix)"
}

# Handle stage transition if specified
if (-not [string]::IsNullOrEmpty($Stage)) {
    Write-StepHeader -Message "Stage Transition" -Icon $icons.Calendar

    $stageIcon = $icons.Alpha
    $stageColor = $colors.Alpha

    if ($Stage -eq "beta") {
        $stageIcon = $icons.Beta
        $stageColor = $colors.Beta
    }
    elseif ($Stage -eq "release") {
        $stageIcon = $icons.Release
        $stageColor = $colors.Release
    }

    Write-StepResult -Message "Transitioning to $Stage stage" -Icon $stageIcon -Color $stageColor

    # Check if we're already at the requested stage
    if ($Stage -eq "release" -and [string]::IsNullOrEmpty($CurrentSuffix)) {
        Write-Warning "Already at release stage (no suffix)"
    }
    elseif ($Stage -eq $CurrentSuffix) {
        Write-Warning "Already at $Stage stage"
    }

    # Validate stage transitions
    if ($CurrentSuffix -eq "alpha" -and $Stage -eq "release") {
        if (-not (Get-Confirmation "Are you sure you want to jump directly from alpha to release? It's usually better to go through beta first.")) {
            exit 0
        }
    }
}

# Determine bump type from commits if not forced
if ([string]::IsNullOrEmpty($ForceBump)) {
    Write-StepHeader -Message "Analyzing Git Commits" -Icon $icons.Git

    # Get all commits since last tag
    try {
        $LastTag = git describe --tags --abbrev=0 2>$null
    }
    catch {
        $LastTag = ""
    }

    if ([string]::IsNullOrEmpty($LastTag)) {
        # No tags yet, analyze all commits
        Write-Info "No previous tags found, analyzing all commits"
        $Commits = git log --pretty=format:"%s"
    }
    else {
        # Analyze commits since last tag
        Write-Info "Analyzing commits since $LastTag"
        $Commits = git log "$LastTag..HEAD" --pretty=format:"%s"
    }

    # Check for breaking changes or feat! (major bump)
    if ($Commits -match "^(BREAKING CHANGE:|feat!:|fix!:|refactor!:)") {
        $BumpType = "major"
        Write-StepResult -Message "Found breaking changes - Major bump required" -Icon $icons.Major -Color $colors.Major
    }
    # Check for new features (minor bump)
    elseif ($Commits -match "^feat(\([^)]+\))?:") {
        $BumpType = "minor"
        Write-StepResult -Message "Found new features - Minor bump required" -Icon $icons.Minor -Color $colors.Minor
    }
    # Default to patch for fixes, docs, etc.
    else {
        $BumpType = "patch"
        Write-StepResult -Message "Found fixes or other changes - Patch bump required" -Icon $icons.Patch -Color $colors.Patch
    }
}
else {
    $BumpType = $ForceBump
    $bumpIcon = $icons.Patch
    $bumpColor = $colors.Patch

    if ($BumpType -eq "minor") {
        $bumpIcon = $icons.Minor
        $bumpColor = $colors.Minor
    }
    elseif ($BumpType -eq "major") {
        $bumpIcon = $icons.Major
        $bumpColor = $colors.Major
    }

    Write-StepHeader -Message "Version Bump" -Icon $icons.Version
    Write-StepResult -Message "Forced $BumpType bump" -Icon $bumpIcon -Color $bumpColor
}

# Split version into components
$VersionParts = $CurrentVersion.Split('.')
$Major = [int]$VersionParts[0]
$Minor = [int]$VersionParts[1]
$Patch = [int]$VersionParts[2]

# Bump version according to type
if ($BumpType -eq "major") {
    $NewVersion = "$($Major + 1).0.0"
}
elseif ($BumpType -eq "minor") {
    $NewVersion = "$Major.$($Minor + 1).0"
}
else {
    $NewVersion = "$Major.$Minor.$($Patch + 1)"
}

# Determine new suffix based on stage parameter
$NewSuffix = $CurrentSuffix
if (-not [string]::IsNullOrEmpty($Stage)) {
    $NewSuffix = $Stage
    if ($Stage -eq "release") {
        $NewSuffix = ""
    }
}

# Handle automatic suffix removal for major/minor bumps
if (($BumpType -eq "major" || $BumpType -eq "minor") -and -not [string]::IsNullOrEmpty($NewSuffix) -and [string]::IsNullOrEmpty($Stage)) {
    if (Get-Confirmation "Remove '$NewSuffix' suffix for $BumpType version bump?") {
        $NewSuffix = ""
    }
}

Write-StepHeader -Message "Version Update" -Icon $icons.Version
Write-Info "Current: $CurrentVersion$(if ($CurrentSuffix) { "-$CurrentSuffix" })"

$versionChangeIcon = $icons.Patch
$versionChangeColor = $colors.Patch
if ($BumpType -eq "minor") {
    $versionChangeIcon = $icons.Minor
    $versionChangeColor = $colors.Minor
}
elseif ($BumpType -eq "major") {
    $versionChangeIcon = $icons.Major
    $versionChangeColor = $colors.Major
}

Write-StepResult -Message "New: $NewVersion$(if ($NewSuffix) { "-$NewSuffix" })" -Icon $versionChangeIcon -Color $versionChangeColor

# Apply changes if requested
if ($Apply) {
    Write-StepHeader -Message "Applying Changes" -Icon $icons.Wait

    # Update version in Directory.Build.props
    $PropsXml.Project.PropertyGroup.VersionPrefix = $NewVersion

    # Update suffix based on stage or automatic removal
    $PropsXml.Project.PropertyGroup.VersionSuffix = $NewSuffix
    
    # Update all hardcoded version values to match
    $PropsXml.Project.PropertyGroup.AssemblyVersion = "$NewVersion.0"
    $PropsXml.Project.PropertyGroup.FileVersion = "$NewVersion.0"
    $PropsXml.Project.PropertyGroup.InformationalVersion = $NewVersion
    $PropsXml.Project.PropertyGroup.Version = $NewVersion

    # Save the changes
    $PropsXml.Save((Resolve-Path $PropsFile))

    Write-Success "Updated version in $PropsFile"

    # Display the full version string
    $fullVersion = $NewVersion
    if ($NewSuffix) {
        $fullVersion = "$NewVersion-$NewSuffix"
    }

    Write-StepHeader -Message "Next Steps" -Icon $icons.Rocket
    Write-Info "Version bumped to $fullVersion"
    Write-Info "To create a tag for this version, run:"
    Write-Host "    ./scripts/Create-Tag.ps1" -ForegroundColor $colors.Primary
    Write-Info "To commit the changes:"
    Write-Host "    git add $PropsFile" -ForegroundColor $colors.Primary
    Write-Host "    git commit -m \"chore: bump version to $fullVersion [skip ci]\"" -ForegroundColor $colors.Primary
}
else {
    # Preview mode - just show what would happen
    Write-StepHeader -Message "Preview Only" -Icon $icons.Info
    Write-Info "This was just a preview. To apply this version bump, run with -Apply parameter:"

    # Build the command line with appropriate parameters
    $applyCmd = "./scripts/Bump-Version.ps1 -Apply"

    # Add force bump parameter if specified
    if (-not [string]::IsNullOrEmpty($ForceBump)) {
        $applyCmd += " -ForceBump $ForceBump"
    }

    # Add stage parameter if specified
    if (-not [string]::IsNullOrEmpty($Stage)) {
        $applyCmd += " -Stage $Stage"
    }

    # Display the command
    Write-Host "    $applyCmd" -ForegroundColor $colors.Primary
}
