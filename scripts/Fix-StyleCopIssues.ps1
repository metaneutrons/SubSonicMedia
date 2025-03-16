#!/usr/bin/env pwsh
# Fix-StyleCopIssues.ps1
# Script to fix common StyleCop issues in the repository

param (
    [Parameter(Mandatory = $false)]
    [ValidateSet("FileHeader", "All")]
    [string]$Fix = "All",
    
    [Parameter(Mandatory = $false)]
    [string]$SourceDirectory = "./SubSonicMedia",
    
    [Parameter(Mandatory = $false)]
    [switch]$WhatIf
)

# Load the stylecop.json file to get the copyright text
function Get-StyleCopSettings {
    $scriptDir = $PSScriptRoot
    if (-not $scriptDir) {
        $scriptDir = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
    }
    
    $stylecopPath = Join-Path $scriptDir "../stylecop.json"
    if (-not (Test-Path $stylecopPath)) {
        Write-Error "stylecop.json not found at: $stylecopPath"
        exit 1
    }
    
    try {
        $stylecop = Get-Content -Path $stylecopPath -Raw | ConvertFrom-Json
        return $stylecop.settings
    }
    catch {
        Write-Error "Failed to parse stylecop.json: $_"
        exit 1
    }
}

# Add or fix file headers in C# files
function Fix-FileHeaders {
    param (
        [string]$Directory,
        [string]$CompanyName,
        [string]$CopyrightText,
        [switch]$WhatIf
    )
    
    Write-Host "🔍 Scanning for files missing GPL-3.0 headers in $Directory..."
    
    # Convert relative path to absolute if needed
    if (-not [System.IO.Path]::IsPathRooted($Directory)) {
        $Directory = Join-Path (Get-Location) $Directory
    }
    
    # Get all .cs files, excluding generated files and files in bin, obj folders
    $files = Get-ChildItem -Path $Directory -Include "*.cs" -Recurse -File | 
             Where-Object { 
                 $_.FullName -notlike "*/obj/*" -and 
                 $_.FullName -notlike "*/bin/*" -and
                 $_.Name -ne "AssemblyInfo.cs" -and 
                 $_.Name -ne "GlobalUsings.g.cs"
             }
    
    $fixedCount = 0
    $skippedCount = 0
    
    foreach ($file in $files) {
        $content = Get-Content -Path $file.FullName -Raw
        
        # Check if the file already has a GPL header
        if ($content -notlike "*GNU General Public License*") {
            # Generate the header
            $fileName = $file.Name
            $headerText = @"
// <copyright file="$fileName" company="$CompanyName">
// $($CopyrightText -replace "`n", "`n// ")
// </copyright>

"@
            
            if ($WhatIf) {
                Write-Host "WhatIf: Would add GPL-3.0 header to $($file.FullName)" -ForegroundColor Yellow
            } else {
                # Add the header to the file
                $updatedContent = $headerText + $content
                Set-Content -Path $file.FullName -Value $updatedContent -NoNewline
                Write-Host "✅ Added GPL-3.0 header to $($file.FullName)" -ForegroundColor Green
            }
            $fixedCount++
        }
        else {
            $skippedCount++
        }
    }
    
    if ($WhatIf) {
        Write-Host "📊 Summary: Would add headers to $fixedCount files, skip $skippedCount files (already have headers)" -ForegroundColor Cyan
    } else {
        Write-Host "📊 Summary: Added headers to $fixedCount files, skipped $skippedCount files (already have headers)" -ForegroundColor Cyan
    }
}

# Main execution flow
$settings = Get-StyleCopSettings
$companyName = $settings.documentationRules.companyName
$copyrightText = $settings.documentationRules.copyrightText

if ($Fix -eq "FileHeader" -or $Fix -eq "All") {
    Fix-FileHeaders -Directory $SourceDirectory -CompanyName $companyName -CopyrightText $copyrightText -WhatIf:$WhatIf
}

Write-Host "✨ StyleCop fixes completed!" -ForegroundColor Green