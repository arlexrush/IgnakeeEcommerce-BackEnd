# Upgrade Options — EcommerceSolution

Assessment: 4 projects, all currently targeting net7.0, with package updates and API compatibility issues identified during assessment.

## Strategy

### Upgrade Strategy
A small modern .NET solution with a simple dependency graph and limited migration risk, so the upgrade can be completed in a single atomic pass.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade all projects simultaneously in a single pass. |
| Top-Down | Upgrade the entry-point application first and keep shared libraries buildable through an incremental phase. |

## Project Structure

### Project Approach
The solution already uses SDK-style projects, so no structural migration is required.

| Value | Description |
|-------|-------------|
| **In-place** (selected) | Keep the current project layout and update the target framework in place. |

## Compatibility

### Unsupported Packages
The assessment surfaced a small number of package updates needed for .NET 8 compatibility, so the upgrade will resolve them inline.

| Value | Description |
|-------|-------------|
| **Resolve Inline** (selected) | Update incompatible packages directly in the same task. |
| Defer Resolution | Defer package replacement work to a later follow-up task. |

### Unsupported API Handling
The assessment detected a small number of compatibility issues that should be fixed directly in the upgrade pass.

| Value | Description |
|-------|-------------|
| **Fix Inline** (selected) | Resolve API-level migration issues during the main upgrade task. |
| Defer Complex Changes | Leave complex API replacements for a later follow-up task. |

## Modernization

### Nullable Reference Types
The solution already enables nullable reference types in the projects, so no change is needed.

| Value | Description |
|-------|-------------|
| **Leave Disabled** (selected) | Keep nullable reference types unchanged because the projects already have them enabled. |
| Enable Nullable Reference Types | Add nullable support to projects that do not already enable it. |
