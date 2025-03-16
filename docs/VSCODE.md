# VS Code Setup for SubSonicMedia

This document describes the VS Code setup for building the SubSonicMedia library and running/debugging the TestKit.

## Features

- Build tasks for the entire solution, library, and TestKit
- Debug configurations for running and debugging the TestKit
- Recommended extensions for .NET development
- Code formatting and style enforcement
- Editor settings optimized for .NET development

## Build Tasks

Press `Ctrl+Shift+B` (or `Cmd+Shift+B` on macOS) to access the build tasks:

- **build**: Builds the entire solution
- **build-library**: Builds only the SubSonicMedia library
- **build-testkit**: Builds only the TestKit
- **publish**: Publishes the SubSonicMedia library
- **watch**: Watches for changes and automatically rebuilds the TestKit

## Running and Debugging

Press `F5` to start debugging the TestKit. Two debug configurations are available:

- **Run TestKit**: Runs the TestKit with standard settings
- **Debug TestKit**: Runs the TestKit with enhanced debugging (disables Just My Code)

## Recommended Extensions

The workspace recommends several extensions that enhance the development experience:

- C# Dev Kit and C# extension for .NET development
- CSharpier for code formatting
- .NET Test Explorer for running tests
- Code Spell Checker for catching typos
- EditorConfig for consistent coding styles
- GitHub Actions for CI/CD workflow editing

## Code Style and Formatting

The project uses:

- EditorConfig for consistent code style
- StyleCop for code analysis
- CSharpier for code formatting

Code will be automatically formatted on save according to the project's style rules.
