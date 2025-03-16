#!/usr/bin/env pwsh
# Commit message validation hook for SubSonicMedia
param(
    [Parameter(Mandatory = $true)]
    [string]$CommitMessageFile
)

# Get the commit message from the file
$commitMsg = Get-Content -Path $CommitMessageFile -Raw

# Define the pattern (using a conventional commit format)
# Format: type(scope): description
# Types: feat, fix, docs, style, refactor, test, chore, etc.
$pattern = '^(feat|fix|docs|style|refactor|test|chore|build|ci|perf|revert)(\([a-z0-9-]+\))?: .{1,100}$'

# Check if the commit message matches the pattern
if ($commitMsg -notmatch $pattern) {
    Write-Host "❌ Commit message does not follow the conventional commit format." -ForegroundColor Red
    Write-Host "Required format: <type>(optional scope): <description>" -ForegroundColor Yellow
    Write-Host "Types: feat, fix, docs, style, refactor, test, chore, build, ci, perf, revert" -ForegroundColor Yellow
    Write-Host "Example: feat(api): add support for bookmarks endpoint" -ForegroundColor Yellow
    exit 1
}

Write-Host "✅ Commit message follows the conventional commit format." -ForegroundColor Green