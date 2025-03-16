#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Analyzes commit messages and bumps version according to semantic versioning rules.
.DESCRIPTION
    This script analyzes Git commit messages since the last tag and determines the appropriate
    version bump (major, minor, patch) based on conventional commit format. It can update the
    version in Directory.Build.props and create a Git tag.
.PARAMETER Apply
    If specified, applies the version change, creates a commit, and creates a tag.
.PARAMETER ForceBump
    Forces a specific bump type (patch, minor, major) instead of analyzing commits.
.EXAMPLE
    .\scripts\Bump-Version.ps1
    # Analyzes commits and suggests a version bump
.EXAMPLE
    .\scripts\Bump-Version.ps1 -Apply
    # Analyzes commits, applies the version bump, and creates a tag
.EXAMPLE
    .\scripts\Bump-Version.ps1 -ForceBump minor -Apply
    # Forces a minor version bump, applies it, and creates a tag
#>

param (
    [switch]$Apply = $false,
    [ValidateSet("patch", "minor", "major", "")]
    [string]$ForceBump = ""
)

# Ensure we stop on errors
$ErrorActionPreference = "Stop"

# Props file path
$PropsFile = "SubSonicMedia\Directory.Build.props"

# Get current version from Directory.Build.props
[xml]$PropsXml = Get-Content $PropsFile
$CurrentVersion = $PropsXml.Project.PropertyGroup.VersionPrefix
Write-Host "Current version: $CurrentVersion"

# Determine bump type from commits if not forced
if ([string]::IsNullOrEmpty($ForceBump)) {
    # Get all commits since last tag
    try {
        $LastTag = git describe --tags --abbrev=0 2>$null
    }
    catch {
        $LastTag = ""
    }
    
    if ([string]::IsNullOrEmpty($LastTag)) {
        # No tags yet, analyze all commits
        Write-Host "No previous tags found, analyzing all commits"
        $Commits = git log --pretty=format:"%s"
    }
    else {
        # Analyze commits since last tag
        Write-Host "Analyzing commits since $LastTag"
        $Commits = git log "$LastTag..HEAD" --pretty=format:"%s"
    }
    
    # Check for breaking changes or feat! (major bump)
    if ($Commits -match "^(BREAKING CHANGE:|feat!:|fix!:|refactor!:)") {
        $BumpType = "major"
    }
    # Check for new features (minor bump)
    elseif ($Commits -match "^feat(\([^)]+\))?:") {
        $BumpType = "minor"
    }
    # Default to patch for fixes, docs, etc.
    else {
        $BumpType = "patch"
    }
}
else {
    $BumpType = $ForceBump
}

Write-Host "Determined bump type: $BumpType"

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

Write-Host "New version will be: $NewVersion"

# Apply changes if requested
if ($Apply) {
    Write-Host "Applying version bump..."
    
    # Update version in Directory.Build.props
    $PropsXml.Project.PropertyGroup.VersionPrefix = $NewVersion
    
    # If this is the first release, also remove alpha suffix
    if ($PropsXml.Project.PropertyGroup.VersionSuffix -eq "alpha") {
        $PropsXml.Project.PropertyGroup.VersionSuffix = ""
    }
    
    # Save the changes
    $PropsXml.Save((Resolve-Path $PropsFile))
    
    Write-Host "Updated version in $PropsFile"
    
    # Commit the change
    git add $PropsFile
    git commit -m "chore: bump version to $NewVersion [skip ci]"
    
    # Create tag
    git tag "v$NewVersion"
    
    Write-Host "Created commit and tag v$NewVersion"
    Write-Host "To push changes and trigger release workflow, run:"
    Write-Host "  git push; git push --tags"
}
else {
    Write-Host "To apply this version bump, run with -Apply parameter"
}
