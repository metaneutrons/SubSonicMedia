#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Creates a new Git tag for GitHub CI based on the project version.
.DESCRIPTION
    This script performs all necessary pre-checks (no uncommitted files),
    extracts the version number from the project's versioning schema using GitVersion,
    and proposes the Git commands to execute after confirmation.

    NOTE: This project uses GitVersion for versioning. You may also use the
    "GitVersion Update" workflow in GitHub Actions to update version and create tags.
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

# Check for unpushed commits
Write-StepHeader -Message "Checking for unpushed commits" -Icon $icons.Git

$unpushedCommits = git log --branches --not --remotes --oneline
if ($unpushedCommits) {
    Write-Warning "You have unpushed commits:"
    $unpushedCommits | ForEach-Object { Write-Host "      $_" -ForegroundColor $colors.Muted }
} else {
    Write-Success "No unpushed commits detected"
}

# Check if we're on main branch
$currentBranch = git branch --show-current
if ($currentBranch -ne "main") {
    Write-Warning "You are not on the main branch (current: $currentBranch). This is just a warning, you can continue."
}
else {
    Write-Success "Currently on main branch"
}

# Extract version using GitVersion if available, otherwise from Directory.Build.props
Write-StepHeader -Message "Extracting version information" -Icon $icons.Version

# Check if GitVersion is installed
$gitVersionInstalled = $null -ne (Get-Command "dotnet-gitversion" -ErrorAction SilentlyContinue) -or
                       $null -ne (Get-Command "gitversion" -ErrorAction SilentlyContinue)

if ($gitVersionInstalled) {
    # Use GitVersion to get version info
    Write-Info "Running GitVersion to determine version..."
    
    try {
        $gitVersionOutput = dotnet gitversion /output json | ConvertFrom-Json
        
        $versionPrefix = $gitVersionOutput.MajorMinorPatch
        
        # Check if this is a prerelease
        if ($gitVersionOutput.PreReleaseLabel) {
            $versionSuffix = $gitVersionOutput.PreReleaseLabel
            
            # Add number if available
            if ($gitVersionOutput.PreReleaseNumber) {
                $versionSuffix += ".$($gitVersionOutput.PreReleaseNumber)"
            }
            
            # Construct full version with suffix
            $fullVersion = "$versionPrefix-$versionSuffix"
        }
        else {
            # No suffix for stable releases
            $versionSuffix = ""
            $fullVersion = $versionPrefix
        }
    }
    catch {
        Write-Warning "Error running GitVersion: $_"
        Write-Warning "Falling back to Directory.Build.props..."
        $gitVersionInstalled = $false
    }
}

# Fallback to Directory.Build.props if GitVersion not available or failed
if (-not $gitVersionInstalled) {
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
    Write-Warning "Tag v$fullVersion already exists locally!"
}

# Get the current date for the tag message
$currentDate = Get-Date -Format "yyyy-MM-dd"

# Propose git commands and handle existing tags
Write-StepHeader -Message "Proposed Git commands" -Icon $icons.Commit

$tagCommand = "git tag -a v$fullVersion -m 'Release v$fullVersion ($currentDate)'"
$pushCommand = "git push origin v$fullVersion"

# Prepare delete commands for preview if tag exists
$deleteLocalCommand = "git tag -d v$fullVersion"
$deleteRemoteCommand = "git push --delete origin v$fullVersion"

# Show commands in order of execution
Write-Host "    Command sequence:" -ForegroundColor $colors.Info
Write-Host ""

# Show tag management first if tag exists
if ($tagExists) {
    Write-Host "    $($icons.Warning) Tag v$fullVersion already exists locally" -ForegroundColor $colors.Warning
    Write-Host ""
    Write-Host "    Step 1 (if you choose to delete existing tag):" -ForegroundColor $colors.Info
    Write-Host "    $deleteLocalCommand" -ForegroundColor $colors.Primary
    Write-Host "    $deleteRemoteCommand (if tag exists remotely)" -ForegroundColor $colors.Primary
    Write-Host ""
    Write-Host "    Step 2 (create new tag):" -ForegroundColor $colors.Info
    Write-Host "    $tagCommand" -ForegroundColor $colors.Primary
    
    # Check if we need to show combined push command
    $hasUnpushedCommits = $null -ne (git log --branches --not --remotes --oneline)
    $currentBranch = git branch --show-current
    if ($hasUnpushedCommits) {
        Write-Host "    git push origin $currentBranch v$fullVersion" -ForegroundColor $colors.Primary
    } else {
        Write-Host "    $pushCommand" -ForegroundColor $colors.Primary
    }
}
else {
    # Just show commands without "Step 1" label
    $hasUnpushedCommits = $null -ne (git log --branches --not --remotes --oneline)
    $currentBranch = git branch --show-current
    
    Write-Host "    $tagCommand" -ForegroundColor $colors.Primary
    if ($hasUnpushedCommits) {
        Write-Host "    git push origin $currentBranch v$fullVersion" -ForegroundColor $colors.Primary
    } else {
        Write-Host "    $pushCommand" -ForegroundColor $colors.Primary
    }
}

Write-Host ""

# Handle uncommitted changes as a blocker
if ($hasUncommittedChanges) {
    Write-Host "  $($icons.Warning)  " -ForegroundColor $colors.Warning -NoNewline
    Write-Host "You have uncommitted changes." -ForegroundColor $colors.Warning
    Write-Host ""
    Write-Host "  Please commit or stash your changes first, then run the script again." -ForegroundColor $colors.Warning
    exit 0
}

# Handle existing tags
$needsConfirmation = $false

if ($tagExists) {
    # Ask if user wants to delete the existing tag first
    if (Get-Confirmation "Do you want to delete the existing tag before creating a new one?") {
        Write-Host "    Deleting existing tag..." -ForegroundColor $colors.Muted
        $deleteLocalResult = Invoke-Expression "git tag -d v$fullVersion 2>&1"

        if ($LASTEXITCODE -eq 0) {
            Write-Success "Local tag v$fullVersion deleted successfully"

            # Check if tag exists remotely and ask to delete it too
            $remoteTagExists = Invoke-Expression "git ls-remote --tags origin refs/tags/v$fullVersion 2>&1"
            if ($remoteTagExists) {
                if (Get-Confirmation "Tag also exists remotely. Delete remote tag as well?") {
                    Write-Host "    Deleting remote tag..." -ForegroundColor $colors.Muted
                    $deleteRemoteResult = Invoke-Expression "git push --delete origin v$fullVersion 2>&1"

                    if ($LASTEXITCODE -eq 0) {
                        Write-Success "Remote tag v$fullVersion deleted successfully"
                    }
                    else {
                        Write-Warning "Failed to delete remote tag: $deleteRemoteResult"
                        Write-Warning "Continuing with local operations only"
                    }
                }
            }

            # Tag has been deleted, no need for further confirmation
            $needsConfirmation = $false
        }
        else {
            Write-Warning "Failed to delete local tag: $deleteLocalResult"
            Write-Warning "Will attempt to force create the tag"
            # Still need confirmation for forcing
            $needsConfirmation = true
        }
    }
    else {
        # User chose not to delete, ask if they want to force create
        $needsConfirmation = true
    }
}

# Ask for final confirmation
if ($needsConfirmation) {
    if (-not (Get-Confirmation "Tag exists. Force create anyway?")) {
        Write-Info "Operation cancelled by user"
        exit 0
    }
    # If we're here, we need to force create the tag
    $tagCommand = "git tag -f -a v$fullVersion -m 'Release v$fullVersion ($currentDate)'"
}
else {
    if (-not (Get-Confirmation "Execute these commands?")) {
        Write-Info "Operation cancelled by user"
        exit 0
    }
}

# Execute the commands
Write-StepHeader -Message "Creating and pushing tag" -Icon $icons.Wait

try {
    # Check if we have unpushed commits that need to be included
    $hasUnpushedCommits = $null -ne (git log --branches --not --remotes --oneline)
    $currentBranch = git branch --show-current
    
    # Create the tag
    Write-Host "    Creating tag..." -ForegroundColor $colors.Muted
    $createOutput = Invoke-Expression "$tagCommand 2>&1"
    $createExitCode = $LASTEXITCODE

    if ($createExitCode -ne 0) {
        throw "Failed to create tag: $createOutput"
    }
    
    # Determine if we should do a combined push (commits + tag) or just tag
    if ($hasUnpushedCommits) {
        Write-Host "    Preparing combined push (commits + tag)..." -ForegroundColor $colors.Muted
        
        # Build the appropriate push command
        $combinedPushCommand = "git push origin $currentBranch v$fullVersion"
        Write-Host "    Executing: $combinedPushCommand" -ForegroundColor $colors.Muted
        
        # Execute the combined push
        $pushOutput = Invoke-Expression "$combinedPushCommand 2>&1"
        $pushExitCode = $LASTEXITCODE
    } else {
        # Just push the tag normally
        Write-Host "    Pushing tag to origin..." -ForegroundColor $colors.Muted
        Write-Host "    Executing: $pushCommand" -ForegroundColor $colors.Muted
        $pushOutput = Invoke-Expression "$pushCommand 2>&1"
        $pushExitCode = $LASTEXITCODE
    }
    
    # Print the output for verification
    if ($pushOutput) {
        Write-Host "    Push output: $pushOutput" -ForegroundColor $colors.Muted
    }

    # Check if the push failed because the tag already exists remotely
    if ($pushExitCode -ne 0) {
        if ($pushOutput -match "rejected.*already exists") {
            # Tag exists remotely but we created it locally - this is fine
            Write-Warning "Tag already exists in remote repository"
            Write-Info "Local tag was created successfully"

            # Ask if user wants to force push
            if (Get-Confirmation "Do you want to force push the tag? This will overwrite the remote tag.") {
                Write-Host "    Force pushing tag..." -ForegroundColor $colors.Muted
                
                # If we have unpushed commits, push them separately
                if ($hasUnpushedCommits) {
                    Write-Host "    First pushing commits on branch $currentBranch..." -ForegroundColor $colors.Muted
                    $pushBranchOutput = Invoke-Expression "git push origin $currentBranch 2>&1"
                    
                    if ($LASTEXITCODE -ne 0) {
                        Write-Warning "Failed to push commits: $pushBranchOutput"
                        Write-Warning "Will still attempt to push tag..."
                    } else {
                        Write-Success "Branch commits pushed successfully"
                    }
                }
                
                # Now force push the tag
                $forceOutput = Invoke-Expression "git push -f origin v$fullVersion 2>&1"
                if ($LASTEXITCODE -eq 0) {
                    # Success message for force push
                    Write-Host ""
                    Write-Host "  $($icons.Success)  " -ForegroundColor $colors.Success -NoNewline
                    Write-Host "Tag v$fullVersion successfully force-pushed to origin!" -ForegroundColor $colors.Success
                }
                else {
                    throw "Failed to force push tag: $forceOutput"
                }
            }
            else {
                Write-Info "Force push cancelled by user"
                Write-Info "Local tag was created but not pushed"
            }
        }
        else {
            # Some other push error
            throw "Failed to push tag: $pushOutput"
        }
    }
    else {
        # Verify tag exists on remote
        Write-Host "    Verifying tag on remote..." -ForegroundColor $colors.Muted
        $remoteTagCheck = Invoke-Expression "git ls-remote --tags origin refs/tags/v$fullVersion 2>&1"
        
        # Verify no unpushed commits if we had them before
        $verifyCommitsPushed = $true
        if ($hasUnpushedCommits) {
            Write-Host "    Verifying all commits were pushed..." -ForegroundColor $colors.Muted
            $remainingUnpushedCommits = git log --branches --not --remotes --oneline
            if ($remainingUnpushedCommits) {
                Write-Warning "Some commits were not pushed to the remote during the operation:"
                $remainingUnpushedCommits | ForEach-Object { Write-Host "      $_" -ForegroundColor $colors.Muted }
                $verifyCommitsPushed = $false
            }
        }
        
        if ($remoteTagCheck) {
            # Success message for normal push
            Write-Host ""
            Write-Host "  $($icons.Success)  " -ForegroundColor $colors.Success -NoNewline
            
            if ($hasUnpushedCommits) {
                if ($verifyCommitsPushed) {
                    Write-Host "Tag v$fullVersion and branch commits successfully pushed to origin!" -ForegroundColor $colors.Success
                } else {
                    Write-Host "Tag v$fullVersion was pushed, but some commits may not have been pushed!" -ForegroundColor $colors.Warning
                }
            } else {
                Write-Host "Tag v$fullVersion successfully created and pushed to origin!" -ForegroundColor $colors.Success
            }
        } else {
            Write-Warning "Tag v$fullVersion was created locally but could not be verified on remote."
            Write-Warning "The push command appeared to succeed but the tag may not exist remotely."
            Write-Host "    Remote verification output: $remoteTagCheck" -ForegroundColor $colors.Muted
        }
    }

    Write-Host ""
    Write-Host "  $($icons.Rocket)  " -ForegroundColor $colors.Secondary -NoNewline
    Write-Host "GitHub CI workflow should now be triggered." -ForegroundColor $colors.Secondary
    
    # Final check for any unpushed commits (could happen if combined push failed partially)
    $finalUnpushedCommits = git log --branches --not --remotes --oneline
    if ($finalUnpushedCommits) {
        Write-Host ""
        Write-Warning "You still have unpushed commits after tag creation:"
        $finalUnpushedCommits | ForEach-Object { Write-Host "      $_" -ForegroundColor $colors.Muted }
        
        if (Get-Confirmation "Push all remaining commits to remote now?") {
            Write-Host "    Pushing commits on branch $currentBranch to origin..." -ForegroundColor $colors.Muted
            $pushFinalOutput = Invoke-Expression "git push origin $currentBranch 2>&1"
            
            if ($LASTEXITCODE -eq 0) {
                Write-Success "All remaining commits successfully pushed to origin"
            } else {
                Write-Warning "Failed to push remaining commits: $pushFinalOutput"
                Write-Warning "You should push these commits manually"
            }
        } else {
            Write-Warning "Remember to push your commits manually with: git push origin $currentBranch"
        }
    }
    
    Write-Host ""
}
catch {
    Write-Host ""
    Write-Host "  $($icons.Fail)  " -ForegroundColor $colors.Error -NoNewline
    Write-Host "Error: $_" -ForegroundColor $colors.Error
    Write-Host ""
    exit 1
}
