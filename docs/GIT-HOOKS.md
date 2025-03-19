# Git Hooks for SubSonicMedia

This document describes the Git hooks setup and requirements for the SubSonicMedia project. The project uses [Husky.Net](https://alirezanet.github.io/Husky.Net/) to manage Git hooks, with hooks implemented in PowerShell to align with our PowerShell-only scripting policy.

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

## Available Hooks

1. **pre-commit**: Runs before each commit
   - Formats code with CSharpier
   - Checks for missing GPL-3.0 headers in C# files
   - Prevents shell scripts (.sh) from being committed
   - Runs code style verification
   - Checks for potential secrets or credentials

2. **pre-push**: Runs before pushing to remote
   - Runs a full build with warnings as errors
   - Runs StyleCop check with warnings as errors

3. **commit-msg**: Validates commit messages
   - Ensures commit messages follow the conventional commit format
   - Format: `<type>[(scope)]: <description>`
   - Types: feat, fix, docs, style, refactor, test, chore, build, ci, perf, revert

## Testing the Hooks

### Testing pre-commit hook

```pwsh
# Test CSharpier formatting
dotnet csharpier . --check

# Test shell script detection
touch test.sh
git add test.sh
git commit -m "test: add shell script"  # Should fail
rm test.sh
```

### Testing pre-push hook

```pwsh
# Introduce a StyleCop warning/error
# Example: Remove a required XML comment from a public method
git add .
git commit -m "test: introduce style error"
git push  # Should fail
git reset HEAD~1  # Undo the test commit
```

### Testing commit-msg hook

```pwsh
# Test with invalid message format
git commit -m "adding something"  # Should fail

# Test with valid message format
git commit -m "feat(api): add test feature"  # Should pass (if you have changes to commit)
git reset HEAD~1  # Undo the test commit
```

## Shell Script Scanner

We've also added a PowerShell script to scan for any shell scripts in the repository:

```pwsh
# Check for shell scripts
./scripts/Check-ShellScripts.ps1

# Automatically remove any shell scripts found
./scripts/Check-ShellScripts.ps1 -Remove
```

## Implementation Details

The hooks are implemented as follows:

1. The main functionality is in PowerShell scripts (.ps1)
2. Thin shell script wrappers call the PowerShell scripts to maintain compatibility with Git
3. All PowerShell scripts use the `#!/usr/bin/env pwsh` shebang for cross-platform compatibility
