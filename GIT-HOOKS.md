# Git Hooks for SubSonicMedia

This document describes the Git hooks setup and requirements for the SubSonicMedia project.

## Prerequisites

- PowerShell 7.0 or later (required for versioning scripts)

### Installing PowerShell on Linux or macOS

PowerShell is required for the versioning scripts. Here's how to install it:

#### macOS

```bash
# Using Homebrew
brew install --cask powershell

# Verify installation
pwsh --version
```

#### Linux (Ubuntu/Debian)

```bash
# Download the package
wget https://github.com/PowerShell/PowerShell/releases/download/v7.3.6/powershell_7.3.6-1.deb_amd64.deb

# Install the package
sudo dpkg -i powershell_7.3.6-1.deb_amd64.deb
sudo apt-get install -f

# Verify installation
pwsh --version
```

For other Linux distributions, see the [official PowerShell documentation](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-linux).

## Available Git Hooks

The project includes several Git hooks to automate tasks:

1. **pre-commit**: Validates code formatting and runs quick tests
2. **commit-msg**: Ensures commit messages follow the conventional commit format
3. **post-merge**: Updates dependencies after pulling changes

## Setup

To install the Git hooks, run:

```bash
# Navigate to the repository root
cd SubSonicMedia

# Run the setup script
./scripts/setup-git-hooks.ps1
```

## Manual Execution

You can manually execute any hook by running:

```bash
pwsh ./hooks/[hook-name].ps1
```
