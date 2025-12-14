param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

Write-Host "== .NET Unit tests ==" -ForegroundColor Cyan
dotnet test .\Elearning.DotNetTests\Elearning.Api.UnitTests\Elearning.Api.UnitTests.csproj `
  -c $Configuration `
  --logger "trx;LogFileName=unit_tests.trx" `
  --results-directory .\Elearning.TestQualityDeliverables\reports

Write-Host "== .NET Integration tests ==" -ForegroundColor Cyan
dotnet test .\Elearning.DotNetTests\Elearning.Api.IntegrationTests\Elearning.Api.IntegrationTests.csproj `
  -c $Configuration `
  --logger "trx;LogFileName=integration_tests.trx" `
  --results-directory .\Elearning.TestQualityDeliverables\reports

Write-Host "== Selenium / pytest (optional) ==" -ForegroundColor Cyan
if (Test-Path .\Elearning.SeleniumTests\.venv\Scripts\python.exe) {
    Push-Location .\Elearning.SeleniumTests
    try {
        .\.venv\Scripts\python.exe -m pytest
    }
    finally {
        Pop-Location
    }
}
else {
    Write-Host "Skipping Selenium run: .\\Elearning.SeleniumTests\\.venv not found." -ForegroundColor Yellow
    Write-Host "Create venv and install requirements first (see Elearning.SeleniumTests\\README.md)." -ForegroundColor Yellow
}

