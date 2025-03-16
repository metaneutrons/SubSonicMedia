#!/bin/bash
# Script to analyze commits and bump version locally
# Usage: ./scripts/bump-version.sh [--apply] [--force-bump patch|minor|major]

set -e

# Parse arguments
APPLY=false
FORCE_BUMP=""

while [[ $# -gt 0 ]]; do
  case $1 in
    --apply)
      APPLY=true
      shift
      ;;
    --force-bump)
      FORCE_BUMP="$2"
      shift 2
      ;;
    *)
      echo "Unknown option: $1"
      echo "Usage: ./scripts/bump-version.sh [--apply] [--force-bump patch|minor|major]"
      exit 1
      ;;
  esac
done

# Validate force bump type if provided
if [[ -n "$FORCE_BUMP" && "$FORCE_BUMP" != "patch" && "$FORCE_BUMP" != "minor" && "$FORCE_BUMP" != "major" ]]; then
  echo "Invalid bump type: $FORCE_BUMP. Must be patch, minor, or major."
  exit 1
fi

# Get current version from Directory.Build.props
PROPS_FILE="SubSonicMedia/Directory.Build.props"
CURRENT_VERSION=$(grep -m 1 '<VersionPrefix>' "$PROPS_FILE" | sed -E 's/.*<VersionPrefix>(.*)<\/VersionPrefix>.*/\1/')
echo "Current version: $CURRENT_VERSION"

# Determine bump type from commits if not forced
if [[ -z "$FORCE_BUMP" ]]; then
  # Get all commits since last tag
  LAST_TAG=$(git describe --tags --abbrev=0 2>/dev/null || echo "")
  
  if [[ -z "$LAST_TAG" ]]; then
    # No tags yet, analyze all commits
    echo "No previous tags found, analyzing all commits"
    COMMITS=$(git log --pretty=format:"%s")
  else
    # Analyze commits since last tag
    echo "Analyzing commits since $LAST_TAG"
    COMMITS=$(git log $LAST_TAG..HEAD --pretty=format:"%s")
  fi
  
  # Check for breaking changes or feat! (major bump)
  if echo "$COMMITS" | grep -E "^(BREAKING CHANGE:|feat!:|fix!:|refactor!:)" > /dev/null; then
    BUMP_TYPE="major"
  # Check for new features (minor bump)
  elif echo "$COMMITS" | grep -E "^feat(\([^)]+\))?:" > /dev/null; then
    BUMP_TYPE="minor"
  # Default to patch for fixes, docs, etc.
  else
    BUMP_TYPE="patch"
  fi
else
  BUMP_TYPE="$FORCE_BUMP"
fi

echo "Determined bump type: $BUMP_TYPE"

# Split version into components
IFS='.' read -r MAJOR MINOR PATCH <<< "$CURRENT_VERSION"

# Bump version according to type
if [[ "$BUMP_TYPE" == "major" ]]; then
  NEW_VERSION="$((MAJOR + 1)).0.0"
elif [[ "$BUMP_TYPE" == "minor" ]]; then
  NEW_VERSION="$MAJOR.$((MINOR + 1)).0"
else
  NEW_VERSION="$MAJOR.$MINOR.$((PATCH + 1))"
fi

echo "New version will be: $NEW_VERSION"

# Apply changes if requested
if [[ "$APPLY" == "true" ]]; then
  echo "Applying version bump..."
  
  # Update version in Directory.Build.props
  sed -i '' "s|<VersionPrefix>.*</VersionPrefix>|<VersionPrefix>$NEW_VERSION</VersionPrefix>|g" "$PROPS_FILE"
  
  # If this is the first release, also remove alpha suffix
  if grep -q "<VersionSuffix>alpha</VersionSuffix>" "$PROPS_FILE"; then
    sed -i '' "s|<VersionSuffix>alpha</VersionSuffix>|<VersionSuffix></VersionSuffix>|g" "$PROPS_FILE"
  fi
  
  echo "Updated version in $PROPS_FILE"
  
  # Commit the change
  git add "$PROPS_FILE"
  git commit -m "chore: bump version to $NEW_VERSION [skip ci]"
  
  # Create tag
  git tag "v$NEW_VERSION"
  
  echo "Created commit and tag v$NEW_VERSION"
  echo "To push changes and trigger release workflow, run:"
  echo "  git push && git push --tags"
else
  echo "To apply this version bump, run with --apply flag"
fi
