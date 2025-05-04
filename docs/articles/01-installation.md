---
title: Installation
---

Install the SubSonicMedia library via NuGet.

## Package Manager

```powershell
Install-Package SubSonicMedia
```

## .NET CLI

```bash
dotnet add package SubSonicMedia
```

## PackageReference (csproj)

```xml
<PackageReference Include="SubSonicMedia" Version="x.y.z" />
```

## TestKit

To validate against a real Subsonic server:

```bash
cd SubSonicMedia.TestKit
cp .env.example .env
```

Configure `.env` with:

```env
SUBSONIC_SERVER_URL=https://your-subsonic-server.com
SUBSONIC_USERNAME=your-username
SUBSONIC_PASSWORD=your-password
API_VERSION=1.16.1
RECORD_TEST_RESULTS=true
OUTPUT_DIRECTORY=./TestResults
JUNIT_XML_OUTPUT=true
```

Run tests:

```bash
dotnet run
```
