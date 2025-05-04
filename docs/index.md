# SubSonic Media

SubSonicMedia is a comprehensive .NET client library for the [Subsonic API](https://www.subsonic.org/pages/api.jsp).

## ✨ Features

- Full implementation of [Subsonic API](https://www.subsonic.org/pages/api.jsp) v1.16.1
- Strongly-typed response models
- Interface-based architecture
- Async support throughout
- Auto-generated documentation via DocFX

## 🚀 Installation

```bash
dotnet add package SubSonicMedia
```

## 🧪 TestKit Integration

The TestKit validates the library against a real Subsonic server:

1. Copy the example environment file:

   ```bash
   cd SubSonicMedia.TestKit
   cp .env.example .env
   ```

2. Configure `.env`:

   ```env
   SUBSONIC_SERVER_URL=https://your-subsonic-server.com
   SUBSONIC_USERNAME=your-username
   SUBSONIC_PASSWORD=your-password
   API_VERSION=1.16.1
   RECORD_TEST_RESULTS=true
   OUTPUT_DIRECTORY=./TestResults
   JUNIT_XML_OUTPUT=true
   ```

3. Run the TestKit:

   ```bash
   dotnet run
   ```

## 🤝 Contributing

Contributions are welcome! Please see [here](articles/10-contributing.md) for guidelines.
