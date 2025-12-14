# Static Analysis Report (tool-based)

## Tools used

1) .NET build with compiler/analyzers
- Command: `dotnet build ElearningPlatform.sln -c Release -v minimal`
- Output: `Elearning.TestQualityDeliverables/static/dotnet_build_release.log`

2) Dependency vulnerability scan (NuGet)
- Commands:
  - `dotnet list Elearning.Api/Elearning.Api.csproj package --vulnerable`
  - `dotnet list Elearning.Blazor/Elearning.Blazor.csproj package --vulnerable`
- Outputs:
  - `Elearning.TestQualityDeliverables/static/dotnet_vulnerable_api.log`
  - `Elearning.TestQualityDeliverables/static/dotnet_vulnerable_blazor.log`

## Results summary

- Build warnings: 0
- Build errors: 0
- Vulnerable NuGet packages detected: 0

## Notes

These checks are "static" because they analyze the code/packages without executing runtime business scenarios.

