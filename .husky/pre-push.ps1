#!/usr/bin/env pwsh
# Pre-push hook for SubSonicMedia

# Run a full build to catch any compilation errors
Write-Host "🏗️ Building project..." -ForegroundColor Cyan
dotnet build SubSonicMedia.sln /p:TreatWarningsAsErrors=true

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed! Fix the errors before pushing." -ForegroundColor Red
    exit 1
}

# Run StyleCop check with warnings as errors
Write-Host "🔍 Running StyleCop check..." -ForegroundColor Cyan
dotnet build SubSonicMedia/SubSonicMedia.csproj /p:TreatWarningsAsErrors=true

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ StyleCop check failed! Fix the style issues before pushing." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Pre-push checks passed!" -ForegroundColor Green
