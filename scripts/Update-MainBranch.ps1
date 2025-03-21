#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Updates the main branch with the latest develop tag.
.DESCRIPTION
    This script performs all necessary pre-checks (clean working directory),
    identifies the latest tag on develop, and updates the main branch to that tag.
    It proposes the Git commands to execute after confirmation.
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
    Branch   = "🌿"
    Merge    = "🔀"
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
    Write-Host "   " + "─" * ($Message.Length + 4) -ForegroundColor $colors.Muted
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

  $($icons.Branch)  MAIN BRANCH UPDATER  $($icons.Merge)
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
    Write-Warning "You have uncommitted changes. It's recommended to commit or stash them first."
    Write-Host "      Uncommitted changes:" -ForegroundColor $colors.Muted
    $status | ForEach-Object { Write-Host "      $_" -ForegroundColor $colors.Muted }
    
    if (-not (Get-Confirmation "Continue anyway with uncommitted changes?")) {
        exit 0
    }
}
else {
    Write-Success "No uncommitted changes detected"
}

# Check current branch
$currentBranch = git branch --show-current
Write-Info "Current branch: $currentBranch"

# Find the latest tag on develop branch
Write-StepHeader -Message "Finding latest tag on develop branch" -Icon $icons.Tag

# Make sure local develop branch is up to date
git fetch origin develop 2>&1 | Out-Null
if (-not $?) {
    Write-Error "Failed to fetch latest develop branch"
}

# Find the latest tag on develop
$latestTag = git describe --tags --abbrev=0 origin/develop 2>&1
if (-not $?) {
    Write-Error "Failed to find tags on develop branch"
}

Write-Success "Latest tag on develop: $latestTag"

# Get commit info for the tag
$tagCommit = git rev-list -n 1 $latestTag 2>&1
if (-not $?) {
    Write-Error "Failed to get commit for tag $latestTag"
}

$tagCommitMessage = git log -1 --format=%s $tagCommit 2>&1
$tagCommitDate = git log -1 --format=%ad --date=short $tagCommit 2>&1

Write-Info "Tag commit: $tagCommit"
Write-Info "Commit message: $tagCommitMessage"
Write-Info "Commit date: $tagCommitDate"

# Check main branch status
Write-StepHeader -Message "Checking main branch status" -Icon $icons.Branch

# Make sure local main branch is up to date
git fetch origin main 2>&1 | Out-Null
if (-not $?) {
    Write-Error "Failed to fetch latest main branch"
}

# Get the current commit on main
$mainCommit = git rev-parse origin/main 2>&1
$mainCommitMessage = git log -1 --format=%s origin/main 2>&1
$mainCommitDate = git log -1 --format=%ad --date=short origin/main 2>&1

Write-Info "Main branch commit: $mainCommit"
Write-Info "Commit message: $mainCommitMessage"
Write-Info "Commit date: $mainCommitDate"

# Check relationship between main and the tag commit
$isAncestor = $false
git merge-base --is-ancestor $mainCommit $tagCommit 2>&1 | Out-Null
if ($?) {
    $isAncestor = $true
    Write-Info "Main branch is an ancestor of the tag commit (fast-forward possible)"
}
else {
    Write-Warning "Main branch is not an ancestor of the tag commit (merge will be required)"
}

# Propose commands
Write-StepHeader -Message "Proposed Git commands" -Icon $icons.Commit

# Check if we need to switch to main branch
$switchRequired = $currentBranch -ne "main"
$switchCommand = "git checkout main"

# Prepare update commands
$fetchCommand = "git fetch --all --prune --tags"
$resetCommand = "git reset --hard $latestTag"
$pushCommand = "git push origin main"

# Show commands in order of execution
Write-Host "    Command sequence:" -ForegroundColor $colors.Info
Write-Host ""

if ($switchRequired) {
    Write-Host "    Step 1 (switch to main branch):" -ForegroundColor $colors.Info
    Write-Host "    $switchCommand" -ForegroundColor $colors.Primary
    Write-Host ""
    Write-Host "    Step 2 (update references):" -ForegroundColor $colors.Info
}
else {
    Write-Host "    Step 1 (update references):" -ForegroundColor $colors.Info
}

Write-Host "    $fetchCommand" -ForegroundColor $colors.Primary
Write-Host ""

if ($switchRequired) {
    Write-Host "    Step 3 (update main to tag):" -ForegroundColor $colors.Info
}
else {
    Write-Host "    Step 2 (update main to tag):" -ForegroundColor $colors.Info
}

Write-Host "    $resetCommand" -ForegroundColor $colors.Primary
Write-Host ""

if ($switchRequired) {
    Write-Host "    Step 4 (push changes):" -ForegroundColor $colors.Info
}
else {
    Write-Host "    Step 3 (push changes):" -ForegroundColor $colors.Info
}

Write-Host "    $pushCommand" -ForegroundColor $colors.Primary
Write-Host ""

# Get final confirmation
if (-not (Get-Confirmation "Execute these commands to update main to $latestTag?")) {
    Write-Info "Operation cancelled by user"
    exit 0
}

# Execute the commands
Write-StepHeader -Message "Updating main branch" -Icon $icons.Wait

try {
    # Switch to main if needed
    if ($switchRequired) {
        Write-Host "    Switching to main branch..." -ForegroundColor $colors.Muted
        $switchOutput = Invoke-Expression "$switchCommand 2>&1"
        if ($LASTEXITCODE -ne 0) {
            throw "Failed to switch to main branch: $switchOutput"
        }
        Write-Success "Switched to main branch"
    }

    # Fetch all updates
    Write-Host "    Updating references..." -ForegroundColor $colors.Muted
    $fetchOutput = Invoke-Expression "$fetchCommand 2>&1"
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to fetch updates: $fetchOutput"
    }
    Write-Success "References updated successfully"

    # Reset main to the tag
    Write-Host "    Updating main to tag $latestTag..." -ForegroundColor $colors.Muted
    $resetOutput = Invoke-Expression "$resetCommand 2>&1"
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to update main to tag: $resetOutput"
    }
    Write-Success "Main branch updated to $latestTag successfully"

    # Push the changes
    Write-Host "    Pushing changes to origin..." -ForegroundColor $colors.Muted
    $pushOutput = Invoke-Expression "$pushCommand 2>&1"
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to push changes: $pushOutput"
    }

    # Success message
    Write-Host ""
    Write-Host "  $($icons.Success)  " -ForegroundColor $colors.Success -NoNewline
    Write-Host "Main branch successfully updated to $latestTag and pushed to origin!" -ForegroundColor $colors.Success
    Write-Host ""
    Write-Host "  $($icons.Rocket)  " -ForegroundColor $colors.Secondary -NoNewline
    Write-Host "Main branch is now at the same commit as tag $latestTag" -ForegroundColor $colors.Secondary
    Write-Host ""
}
catch {
    Write-Host ""
    Write-Host "  $($icons.Fail)  " -ForegroundColor $colors.Error -NoNewline
    Write-Host "Error: $_" -ForegroundColor $colors.Error
    Write-Host ""
    exit 1
}