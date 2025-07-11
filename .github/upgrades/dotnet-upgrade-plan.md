# .NET 9.0 Upgrade Plan

## Execution Steps

1. Validate that an .NET 9.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 9.0 upgrade.
3. Upgrade Control\Diagram.NET.csproj
4. Upgrade TestForm\TestForm.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|

### Project upgrade details

#### Control\Diagram.NET.csproj modifications

Project properties changes:
  - Convert project file to SDK-style format
  - Target framework should be changed from `.NETFramework,Version=v4.8` to `net9.0-windows`

#### TestForm\TestForm.csproj modifications

Project properties changes:
  - Convert project file to SDK-style format
  - Target framework should be changed from `.NETFramework,Version=v4.8` to `net9.0-windows`
