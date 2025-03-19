#!/usr/bin/env pwsh
# Fix-StyleCopIssues.ps1
# Script to fix common StyleCop issues in the repository
#
# Fixes the following StyleCop issues:
# - SA1633: Missing or invalid file header XML
# - SA1512: Single-line comments should not be followed by blank line

param (
    [Parameter(Mandatory = $false)]
    [ValidateSet("FileHeader", "CommentSpacing", "All")]
    [string]$Fix = "All",
    
    [Parameter(Mandatory = $false)]
    [string]$SourceDirectory = ".",
    
    [Parameter(Mandatory = $false)]
    [switch]$WhatIf,
    
    [Parameter(Mandatory = $false)]
    [switch]$VerboseOutput
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

# Add or fix file headers in C# files (SA1633)
function Repair-FileHeaders {
    param (
        [string]$Directory,
        [string]$CompanyName,
        [string]$CopyrightText,
        [switch]$WhatIf,
        [switch]$VerboseOutput
    )
    
    Write-Host "🔍 Scanning for files missing headers in $Directory..."
    
    # Convert relative path to absolute if needed
    if (-not [System.IO.Path]::IsPathRooted($Directory)) {
        $Directory = Join-Path (Get-Location) $Directory
    }
    
    # Get all .cs files
    $files = Get-ChildItem -Path $Directory -Include "*.cs" -Recurse -File
    
    $fixedCount = 0
    $skippedCount = 0
    
    foreach ($file in $files) {
        $content = Get-Content -Path $file.FullName -Raw
        $fileName = $file.Name
        
        # Check if the file already has a valid header
        if (-not ($content -match [regex]::Escape("// <copyright file=`"$fileName`" company="))) {
            # Generate the header
            $headerText = @"
// <copyright file="$fileName" company="$CompanyName">
// $($CopyrightText -replace "`n", "`n// ")
// </copyright>

"@
            
            if ($WhatIf) {
                Write-Host "WhatIf: Would add header to $($file.FullName)" -ForegroundColor Yellow
                if ($VerboseOutput) {
                    Write-Host "  Header to add:" -ForegroundColor Gray
                    Write-Host $headerText -ForegroundColor Gray
                }
            } else {
                # Add the header to the file
                $updatedContent = $headerText + $content
                Set-Content -Path $file.FullName -Value $updatedContent -NoNewline
                Write-Host "✅ Added header to $($file.FullName)" -ForegroundColor Green
            }
            $fixedCount++
        }
        else {
            if ($Verbose) {
                Write-Host "⏩ Skipping $($file.FullName) - already has header" -ForegroundColor Gray
            }
            $skippedCount++
        }
    }
    
    if ($WhatIf) {
        Write-Host "📊 Summary: Would add headers to $fixedCount files, skip $skippedCount files (already have headers)" -ForegroundColor Cyan
    } else {
        Write-Host "📊 Summary: Added headers to $fixedCount files, skipped $skippedCount files (already have headers)" -ForegroundColor Cyan
    }
    
    return $fixedCount
}

# Fix comment spacing issues (SA1512)
function Repair-CommentSpacing {
    param (
        [string]$Directory,
        [switch]$WhatIf,
        [switch]$VerboseOutput
    )
    
    Write-Host "🔍 Scanning for comment spacing issues in $Directory..."
    
    # Convert relative path to absolute if needed
    if (-not [System.IO.Path]::IsPathRooted($Directory)) {
        $Directory = Join-Path (Get-Location) $Directory
    }
    
    # Get all .cs files
    $files = Get-ChildItem -Path $Directory -Include "*.cs" -Recurse -File
    
    $fixedCount = 0
    $skippedCount = 0
    
    foreach ($file in $files) {
        $lines = Get-Content -Path $file.FullName
        $modified = $false
        $newLines = @()
        
        for ($i = 0; $i -lt $lines.Count; $i++) {
            $currentLine = $lines[$i]
            
            # Check if current line is a comment and next line is blank
            if ($i -lt $lines.Count - 1 -and 
                $currentLine -match '^\s*\/\/(?!\/)'  -and 
                $lines[$i + 1] -match '^\s*$') {
                
                # Add the current line (comment)
                $newLines += $currentLine
                
                # Skip the next line (blank line)
                $i++
                $modified = $true
                
                if ($VerboseOutput) {
                    Write-Host "  Fixed SA1512: Removed blank line after comment at line $($i) in $($file.Name)" -ForegroundColor Gray
                }
            } else {
                # Add the current line as is
                $newLines += $currentLine
            }
        }
        
        if ($modified) {
            if ($WhatIf) {
                Write-Host "WhatIf: Would fix comment spacing in $($file.FullName)" -ForegroundColor Yellow
            } else {
                Set-Content -Path $file.FullName -Value $newLines
                Write-Host "✅ Fixed comment spacing in $($file.FullName)" -ForegroundColor Green
            }
            $fixedCount++
        } else {
            if ($Verbose) {
                Write-Host "⏩ No comment spacing issues in $($file.FullName)" -ForegroundColor Gray
            }
            $skippedCount++
        }
    }
    
    if ($WhatIf) {
        Write-Host "📊 Summary: Would fix comment spacing in $fixedCount files, skip $skippedCount files (no issues)" -ForegroundColor Cyan
    } else {
        Write-Host "📊 Summary: Fixed comment spacing in $fixedCount files, skipped $skippedCount files (no issues)" -ForegroundColor Cyan
    }
    
    return $fixedCount
}

# Main execution flow
$settings = Get-StyleCopSettings
$companyName = $settings.documentationRules.companyName
$copyrightText = $settings.documentationRules.copyrightText

$totalFixed = 0

if ($Fix -eq "FileHeader" -or $Fix -eq "All") {
    $totalFixed += Repair-FileHeaders -Directory $SourceDirectory -CompanyName $companyName -CopyrightText $copyrightText -WhatIf:$WhatIf -VerboseOutput:$VerboseOutput
}

if ($Fix -eq "CommentSpacing" -or $Fix -eq "All") {
    $totalFixed += Repair-CommentSpacing -Directory $SourceDirectory -WhatIf:$WhatIf -VerboseOutput:$VerboseOutput
}

# Delete the bash script if it exists
$bashScriptPath = Join-Path (Get-Location) "fix-stylecop-issues.sh"
if (Test-Path $bashScriptPath) {
    if ($WhatIf) {
        Write-Host "WhatIf: Would remove bash script: $bashScriptPath" -ForegroundColor Yellow
    } else {
        Remove-Item -Path $bashScriptPath -Force
        Write-Host "🗑️ Removed bash script: $bashScriptPath" -ForegroundColor Green
    }
}

Write-Host "✨ StyleCop fixes completed! Total files fixed: $totalFixed" -ForegroundColor Green
Write-Host "" 
Write-Host "Usage examples:" -ForegroundColor Cyan
Write-Host "  pwsh -File scripts/Fix-StyleCopIssues.ps1 -Fix All" -ForegroundColor Gray
Write-Host "  pwsh -File scripts/Fix-StyleCopIssues.ps1 -Fix FileHeader -SourceDirectory ./SubSonicMedia" -ForegroundColor Gray
Write-Host "  pwsh -File scripts/Fix-StyleCopIssues.ps1 -Fix CommentSpacing -WhatIf" -ForegroundColor Gray