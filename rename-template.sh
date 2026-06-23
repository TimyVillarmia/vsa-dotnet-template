#!/usr/bin/env bash
set -euo pipefail

if [ $# -ne 1 ]; then
  echo "Usage: ./rename-template.sh <NewProjectName>"
  echo "Example: ./rename-template.sh Tigum"
  exit 1
fi

OLD_NAME="Project"
NEW_NAME="$1"

# Convert PascalCase/name to lowercase slug for Docker names
NEW_SLUG=$(echo "$NEW_NAME" | tr '[:upper:]' '[:lower:]')
OLD_SLUG=$(echo "$OLD_NAME" | tr '[:upper:]' '[:lower:]')

echo "Renaming template:"
echo "  $OLD_NAME -> $NEW_NAME"
echo "  $OLD_SLUG -> $NEW_SLUG"

# Rename file contents
find . \
  -type f \
  \( -name "*.cs" \
     -o -name "*.csproj" \
     -o -name "*.slnx" \
     -o -name "*.json" \
     -o -name "*.yml" \
     -o -name "*.yaml" \
     -o -name "*.md" \
     -o -name "*.props" \
     -o -name "*.targets" \
     -o -name "*.env" \
     -o -name ".env.example" \
     -o -name "Dockerfile" \
     -o -name "*.sh" \) \
  -not -path "./.git/*" \
  -exec sed -i "s/${OLD_NAME}/${NEW_NAME}/g" {} +

# Rename lowercase docker/database names
find . \
  -type f \
  \( -name "*.json" \
     -o -name "*.yml" \
     -o -name "*.yaml" \
     -o -name "*.md" \
     -o -name "*.env" \
     -o -name ".env.example" \) \
  -not -path "./.git/*" \
  -exec sed -i "s/${OLD_SLUG}/${NEW_SLUG}/g" {} +

# Rename project files
find . -depth -name "*${OLD_NAME}*" | while read -r path; do
  new_path=$(echo "$path" | sed "s/${OLD_NAME}/${NEW_NAME}/g")
  mv "$path" "$new_path"
done

echo "Done."
echo ""
echo "Next recommended commands:"
echo "  dotnet restore src/${NEW_NAME}.slnx"
echo "  dotnet build src/${NEW_NAME}.slnx"
echo "  cp .env.example .env"
echo "  docker compose up -d postgres seq"
