---
title: Configuration
---

## Environment Variables

TestKit requires a `.env` file in `SubSonicMedia.TestKit`:

- `SUBSONIC_SERVER_URL`
- `SUBSONIC_USERNAME`
- `SUBSONIC_PASSWORD`
- `API_VERSION` (e.g., 1.16.1)
- `RECORD_TEST_RESULTS` (`true` or `false`)
- `OUTPUT_DIRECTORY`
- `JUNIT_XML_OUTPUT` (`true` or `false`)

## GitVersion

Configuration in `GitVersion.yml`:

```yaml
mode: ContinuousDeployment
assembly-versioning-scheme: MajorMinorPatch
assembly-file-versioning-scheme: MajorMinorPatch
assembly-informational-format: '{InformationalVersion}'
tag-prefix: '[vV]'
branches:
  develop:
    regex: ^develop$
    label: beta
    increment: Minor
```

The `Directory.Build.props` imports the generated `GitVersion.props` for consistent versioning across projects.
