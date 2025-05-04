#!/usr/bin/env pwsh
# Pre-commit hook for SubSonicMedia

# Format code with CSharpier
# Write-Host "🎨 Running CSharpier to format code..." -ForegroundColor Cyan
# dotnet csharpier .

# Format code
# Write-Host "🎨 Formating code..." -ForegroundColor Cyan
dotnet format
# Verify that all files have GPL-3.0 headers
Write-Host "📝 Checking for GPL-3.0 headers..." -ForegroundColor Cyan
$missingHeaders = Get-ChildItem -Path ./SubSonicMedia -Include *.cs -Recurse -File |
Where-Object { $_.FullName -notlike "*/obj/*" -and $_.FullName -notlike "*/bin/*" } |
Where-Object { $_.Name -ne "SubSonicMedia.AssemblyInfo.cs" -and $_.Name -ne "SubSonicMedia.GlobalUsings.g.cs" } |
Where-Object { (Get-Content $_.FullName -Raw) -notlike "*GNU General Public License*" } |
Select-Object -ExpandProperty FullName

if ($missingHeaders) {
    Write-Host "⚠️ The following files are missing GPL-3.0 headers:" -ForegroundColor Yellow
    $missingHeaders | ForEach-Object { Write-Host "   $_" -ForegroundColor Yellow }
    Write-Host ""
    Write-Host "Running Fix-StyleCopIssues.ps1 to add headers..." -ForegroundColor Yellow
    pwsh -File "scripts/Fix-StyleCopIssues.ps1" -Fix FileHeader -SourceDirectory "./SubSonicMedia"
    Write-Host "✅ Headers added. Please review the changes." -ForegroundColor Green
}

# Check for shell scripts (.sh files) in the staged files
Write-Host "🔍 Checking for shell script files..." -ForegroundColor Cyan
$shellScripts = git diff --cached --name-only --diff-filter=ACM | Select-String -Pattern '\.sh$'

if ($shellScripts) {
    Write-Host "❌ ERROR: Shell script files detected in commit:" -ForegroundColor Red
    $shellScripts | ForEach-Object { Write-Host "   $_" -ForegroundColor Red }
    Write-Host "This project only supports PowerShell scripts (.ps1). Please convert shell scripts to PowerShell or remove them." -ForegroundColor Red
    exit 1
}

# Run formatting and style checks but don't block commits
Write-Host "📋 Running code style verification..." -ForegroundColor Cyan
dotnet build SubSonicMedia/SubSonicMedia.csproj /p:TreatWarningsAsErrors=false

# Check for potential secrets
Write-Host "🔐 Checking for potential secrets or credentials..." -ForegroundColor Cyan
# Define pattern with double quotes and proper escaping
$secretsPattern = "(password|secret|key|token|credential).*=.*[0-9a-zA-Z]{16,}"
$secrets = Get-ChildItem -Path ./SubSonicMedia -Include *.cs, *.json, *.xml -Recurse -File |
Select-String -Pattern $secretsPattern

if ($secrets) {
    Write-Host "⚠️ Potential secrets or credentials found:" -ForegroundColor Yellow
    $secrets | ForEach-Object { Write-Host "   $_" -ForegroundColor Yellow }
    Write-Host "Please verify these are not actual secrets before committing." -ForegroundColor Yellow
    exit 1
}

# Success message
Write-Host "✅ Pre-commit checks passed!" -ForegroundColor Green
