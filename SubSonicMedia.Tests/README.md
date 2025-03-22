# SubSonicMedia.Tests

This project contains unit tests for the SubSonicMedia library using xUnit, FluentAssertions, and WireMock.Net for API mocking.

## Overview

The test project is designed to work alongside the existing `SubSonicMedia.TestKit` integration tests. While the TestKit performs integration testing against a real Subsonic API server, this project focuses on isolated unit tests using mocked responses.

Key components:

- **WireMockServerFixture**: Creates a mock Subsonic API server for testing
- **TestKitResponseLoader**: Loads recorded responses from the TestKit for consistency
- **Unit Tests**: Tests individual components in isolation with predictable responses

## Setup

Before running the tests, you need to sync the TestKit outputs to the test resources directory:

1. Run the TestKit with recording enabled:
   ```
   cd SubSonicMedia.TestKit
   RECORD_TEST_RESULTS=true dotnet run
   ```

2. Run the tests:
   ```
   cd SubSonicMedia.Tests
   dotnet test
   ```

The test project will automatically copy the recorded test responses from the TestKit to the test resources directory. If no TestKit recordings are available, the tests will use fallback dummy responses.

## Test Structure

- **Core**: Tests for core functionality like connection and authentication
- **Clients**: Tests for the various API client implementations
- **Fixtures**: Test fixtures like the WireMock server
- **Helpers**: Helper classes for testing

## Current Test Coverage

- **Connection Tests**: Testing basic connectivity, auth failures, and API version compatibility
- **Browsing Tests**: Testing music folder listing, artist browsing, and directory navigation

## Adding New Tests

1. Record the responses using the TestKit (preferred) or create mock responses
2. Create a new test class for the client you want to test
3. Use the WireMockServerFixture to mock the API, with the path format `rest/{endpoint}.view`
4. Write test methods using xUnit and FluentAssertions

## Guidelines

- Follow the same naming conventions as the rest of the codebase
- Add XML documentation to all public methods
- Use FluentAssertions for clear and expressive assertions
- Mock only the necessary endpoints for each test
- Include GPL license headers in all files
- Use IDisposable with the Reset() method to clean up after tests
- Use [Collection("TestInit")] to ensure test initialization is performed

## Implementation Notes

- The WireMockServerFixture uses dynamic port allocation to avoid conflicts
- TestKitResponseLoader provides fallback responses when real recordings aren't available
- Response formats must match the Subsonic API XML schema but in JSON format
- Tests are isolated from each other by resetting the WireMock server between test methods