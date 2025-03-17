# Check-ShellScripts.ps1
# Check for shell script files in the repository

param (
    [switch]$Remove = $false
)

# Find all .sh files in the repository (excluding .git directory)
$shellScripts = Get-ChildItem -Path $PSScriptRoot\.. -Recurse -Filter "*.sh" -File |
Where-Object { $_.FullName -notlike "*\.git\*" } |
Select-Object -ExpandProperty FullName

if ($shellScripts.Count -eq 0) {
    Write-Host "✅ No shell script files found in the repository." -ForegroundColor Green
    exit 0
}

Write-Host "⚠️ Found $($shellScripts.Count) shell script file(s) in the repository:" -ForegroundColor Yellow
$shellScripts | ForEach-Object { Write-Host "   - $_" -ForegroundColor Yellow }

if ($Remove) {
    Write-Host "🗑️  Removing shell script files..." -ForegroundColor Yellow
    $shellScripts | ForEach-Object {
        Remove-Item -Path $_ -Force
        Write-Host "   - Removed: $_" -ForegroundColor Red
    }
    Write-Host "✅ All shell script files have been removed." -ForegroundColor Green
}
else {
    Write-Host ""
    Write-Host "ℹ️  To remove these files, run: ./scripts/Check-ShellScripts.ps1 -Remove" -ForegroundColor Cyan
    exit 1
}
